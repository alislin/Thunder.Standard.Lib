using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using Thunder.Standard.Lib.Model;

namespace Thunder.Standard.Lib.Extension
{
    public static class EnumExtension
    {
        public static string ToDescriptionString(this Enum val)
        {
            DescriptionAttribute[] attributes = (DescriptionAttribute[])val.GetType().GetField(val.ToString()).GetCustomAttributes(typeof(DescriptionAttribute), false);
            return attributes.Length > 0 ? attributes[0].Description : val.ToString();
        }

        public static TValue ToEnum<TValue>(this int value)
        {
            return (TValue)Enum.ToObject(typeof(TValue), value);
        }

        public static TValue ToEnum<TValue>(this string value)
        {
            return (TValue)Enum.Parse(typeof(TValue), value);
        }

        public static List<SelectOption> ToSelect<TValue>(this TValue val)
        {
            var s = Enum.GetValues(typeof(TValue));
            var list = new List<SelectOption>();
            foreach (var item in s)
            {
                var value = (int)item;
                TValue v = value.ToEnum<TValue>();
                list.Add(new SelectOption { Value = value.ToString(), Object=v, Text = v.ToString() });
            }
            return list;
        }
    }
}
