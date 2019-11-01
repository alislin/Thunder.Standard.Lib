/* Ceated by Ya Lin. 2019/7/1 14:18:05 */

using System;
using System.Collections.Generic;
using System.Text;

namespace Thunder.Standard.Lib.Model
{
    /// <summary>
    /// 选项
    /// </summary>
    public class SelectOption
    {
        public string Text { get; set; }
        public string Value { get; set; }
        public string Group { get; set; }
        public bool Selected { get; set; }
        public object Object { get; set; }

        public T Get<T>()
        {
            return (T)Object;
        }

        public Guid GetGuid()
        {
            return Guid.Parse(Value);
        }

        public int GetInt()
        {
            return Convert.ToInt32(Value);
        }

        public double GetDouble()
        {
            return Convert.ToDouble(Value);
        }

        public bool GetBool()
        {
            return Convert.ToBoolean(Value);

        }
    }

    public class SelectOption<T>:SelectOption
    {
        public T Item { get; set; }
    }
}
