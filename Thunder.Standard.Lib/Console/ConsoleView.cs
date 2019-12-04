using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ZeroLib.ConsoleView
{
    public class ConsoleView : IConsoleView,IDisposable
    {
        /// <summary>
        /// 结构化信息
        /// </summary>
        private List<IViewItem> Items ;
        /// <summary>
        /// 显示主屏
        /// </summary>
        private List<string> PrimaryView;
        /// <summary>
        /// 生成显示信息
        /// </summary>
        private List<string> SecorndView;
        /// <summary>
        /// 显示线程
        /// </summary>
        private Task viewTask;
        private bool RunFlag;
        public int Width { get; private set; } = 80;
        public ConsoleView(int width = 80)
        {
            Width = width > 40 ? width : 80;
            Init();
        }

        private void Init()
        {
            //初始化屏幕
            Items = new List<IViewItem>();
            PrimaryView = new List<string>();
            SecorndView = new List<string>();
            Console.CursorVisible = false;

            viewTask = Task.Run(() =>
            {
                RunFlag = true;
                while (RunFlag)
                {
                    Update();
                    Thread.Sleep(40);
                }
            });
        }

        

        /// <summary>
        /// 创建显示对象
        /// </summary>
        /// <param name="title"></param>
        /// <returns>对象 id</returns>
        public IViewItem Add(string title)
        {
            var view=new ViewItem { Id = Guid.NewGuid(), Title = title };
            Items.Add(view);
            return view;
        }

        public void Remove(Guid id)
        {
            var m = Items.FirstOrDefault(x => x.Id == id);
            if (m != null)
            {
                Items.Remove(m);
            }
        }

        private void Update()
        {
            var list = new List<string>();
            foreach (var item in Items)
            {
                list.AddRange(item.ToList(Width));
                list.Add("");
            }
            SecorndView = list;
            var lineList = new List<LineUpdateView>();
            for (int i = 0; i < SecorndView.Count; i++)
            {
                var line = LineCheck(PrimaryView, SecorndView, i, Width);
                if (line != null)
                {
                    lineList.Add(line);
                }
            }

            //屏幕刷新
            foreach (var item in lineList)
            {
                Console.SetCursorPosition(item.Left, item.Top);
                Console.Write(item.Content);
            }

            PrimaryView.Clear();
            PrimaryView.AddRange(SecorndView.ToArray());
        }

        private LineUpdateView LineCheck(List<string> primaryView, List<string> secorndView, int i,int width)
        {
            var s ="";
            s = string.IsNullOrEmpty(secorndView[i]) ? s.PadRight(width) : secorndView[i].PadRight(width);
            //return new LineUpdateView { Left = 0, Top = i, Content = s };
            if (primaryView.Count > i)
            {
                if (primaryView[i] != secorndView[i])
                {
                    return new LineUpdateView { Left = 0, Top = i, Content = s };
                }
            }
            else
            {
                return new LineUpdateView { Left = 0, Top = i, Content = s };
            }
            return null;
        }

        public void Dispose()
        {
            RunFlag = false;
            viewTask.Wait();
            Console.CursorVisible = true;
        }
    }

    class LineUpdateView
    {
        /// <summary>
        /// 光标位置
        /// </summary>
        public int Left { get; set; }
        public int Top { get; set; }
        /// <summary>
        /// 更新内容
        /// </summary>
        public string Content { get; set; }
    }

    public class ViewItem : IViewItem
    {
        /// <summary>
        /// Id
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 信息
        /// </summary>
        public string Info { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public int Index { get; set; }
        /// <summary>
        /// 进度信息
        /// </summary>
        public ProcessBar TotalProcess { get; set; }
        /// <summary>
        /// 子进度
        /// </summary>
        public List<ProcessBar> ProcessItems { get; set; } = new List<ProcessBar>();
        /// <summary>
        /// 添加子进程
        /// </summary>
        /// <param name="title">标题</param>
        /// <returns></returns>
        public Guid Add(string title)
        {
            var id = Guid.NewGuid();
            var index = ProcessItems.OrderBy(x => x.Index).FirstOrDefault(x => x.Finish)?.Index ?? ProcessItems.Count;
            if (index<ProcessItems.Count)
            {
                ProcessItems.RemoveAt(index);
            }
            ProcessItems.Add(new ProcessBar { Title = title, Id = id, Index = index });
            
            return id;
        }
        /// <summary>
        /// 移除子进程
        /// </summary>
        /// <param name="id"></param>
        public void Remove(Guid id)
        {
            var m = ProcessItems.FirstOrDefault(x => x.Id == id);
            if (m != null)
            {
                m.Finish = true;
            }
        }

        /// <summary>
        /// 更新子进程
        /// </summary>
        /// <param name="id"></param>
        /// <param name="title"></param>
        /// <param name="value"></param>
        /// <param name="info"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        public void Update(Guid id,string title,int value,string info,int min=0,int max = 100)
        {
            var m = ProcessItems.FirstOrDefault(x => x.Id == id);
            if (m != null)
            {
                m.Title = title;
                m.Value = value;
                m.Info = info;
                m.Min = min;
                m.Max = max;
            }
        }

        public List<string> ToList(int width = 80)
        {
            var list = new List<string>();

            list.Add(Title);
            list.Add(Info);
            foreach (var item in ProcessItems)
            {
                list.AddRange(item.ToList(width));
            }

            return list;
        }
    }


    class ConsoleItemTheme
    {
        public object BgColor { get; set; }
        public object TitleColor { get; set; }
    }

    public class ProcessBar
    {
        public Guid Id { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 信息
        /// </summary>
        public string Info { get; set; }
        /// <summary>
        /// 最小值
        /// </summary>
        public int Min { get; set; } = 0;
        /// <summary>
        /// 最大值
        /// </summary>
        public int Max { get; set; } = 100;
        /// <summary>
        /// 当前值
        /// </summary>
        public int Value { get; set; } = 0;
        /// <summary>
        /// 位置索引
        /// </summary>
        public int Index { get; set; }
        /// <summary>
        /// 完成状态(为true时，可以被清理)
        /// </summary>
        public bool Finish { get; set; } = false;
        public char ProssMark { get; set; } = '=';

        public List<string> ToList(int width = 80)
        {
            var list = new List<string>();

            list.Add(Title);
            list.Add(Info);
            //[=================                     ] 50.0%
            //[======================================] 100.0%
            var len = width - 10;
            var p = Value * 100.0 / (Max - Min);
            var plen = (int)(p * len / 100);
            try
            {
                var m = "";
                list.Add($"[{m.PadRight(plen, ProssMark)}{m.PadRight(len - plen)}] {p.ToString("F1")}%");

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return list;
        }
    }
}
