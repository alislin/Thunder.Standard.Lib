using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thunder.Standard.Lib.Extension
{
    public static class JsonExtension
    {
        private static JsonSerializerSettings settings;
        static JsonExtension()
        {
            IsoDateTimeConverter datetimeConverter = new IsoDateTimeConverter();
            datetimeConverter.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";

            settings = new JsonSerializerSettings();
            settings.MissingMemberHandling = Newtonsoft.Json.MissingMemberHandling.Ignore;
            settings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
            settings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            settings.Converters.Add(datetimeConverter);
        }

        public static string ToJson(this object obj, Formatting format= Formatting.None)
        {
            return JsonConvert.SerializeObject(obj, format, settings);
        }

        public static T FromJson<T>(this string json)
        {
            return JsonConvert.DeserializeObject<T>(json, settings);
        }
    }
}
