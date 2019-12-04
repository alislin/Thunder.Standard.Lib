using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZeroLib.DebugLog
{
    public class Log : ILog
    {
        private static Log m_log = new Log();
        public event EventHandler<LogMessage> NewLog;
        public int MaxCount { get; set; } = 1000;
        public List<string> Items { get; private set; } = new List<string>();
        public string File { get; private set; } = "Logs.txt";
        public Log(string logfile="")
        {
            if (!string.IsNullOrWhiteSpace(logfile))
            {
                File = logfile;
            }
            
        }

        public void Write(string log)
        {
            MaxCount = MaxCount > 1 ? MaxCount : 1000;
            MaxCount = MaxCount < 100000 ? MaxCount : 1000;
            var s = $"[{DateTime.Now.ToString("yyyyMMdd HH:mm:ss")}] {log}";
            System.Diagnostics.Debug.WriteLine(s);
            try
            {
                System.IO.File.AppendAllText(File, s + "\r\n");
            }
            catch (Exception ex)
            {
                Items.Add(ex.Message);
            }
            Items.Add(s);
            if (Items.Count>MaxCount)
            {
                var k = Items.Count - MaxCount + 10;
                k = k < Items.Count ? k : Items.Count;
                Items.RemoveRange(Items.Count - k, k);
            }
            NewLog?.Invoke(this, new LogMessage { Message = s, Time = DateTime.Now });
        }

        public static Log Current => m_log;
    }

    public class LogMessage : EventArgs
    {
        public string Message { get; set; }
        public DateTime Time { get; set; }
    }
}
