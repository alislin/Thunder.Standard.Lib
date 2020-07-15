using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Thunder.Standard.Lib.Extension
{
    public static class JsonZExt
    {
        public static string ToJsonZ(this object obj)
        {
            var namelist = obj.ProperName(null);
            var json = obj.ToJson();
            foreach (var item in namelist)
            {
                json = json.Replace($"\"{item.Key}\"", $"\"{item.Value}\"");
            }
            var r = new JsonZ { Key = namelist, Obj = json };
            return r.ToJson();
        }
        public static T FromJsonZ<T>(this string json)
        {
            var jsonz = json.FromJson<JsonZ>();
            foreach (var item in jsonz.Key)
            {
                jsonz.Obj=jsonz.Obj.Replace($"\"{item.Value}\"", $"\"{item.Key}\"");
            }
            return jsonz.Obj.FromJson<T>();
        }
        public static Dictionary<string, string> ProperName(this object obj,Dictionary<string,string> dic)
        {
            if (dic == null)
            {
                dic = new Dictionary<string, string>();
            }
            if (obj == null)
            {
                return new Dictionary<string, string>();
            }
            var type = obj.GetType();
            if (type.IsValueType)
            {
                if (!dic.Keys.Contains(type.Name))
                {
                    dic.Add(type.Name, "");
                }
            }
            else
            {
                var members = type.GetMembers();
                foreach (var member in members)
                {
                    if (member.MemberType == MemberTypes.Property)
                    {
                        if (!dic.Keys.Contains(member.Name))
                        {
                            dic.Add(member.Name, "");
                        }
                        var property = (PropertyInfo)member;
                        
                    }
                    else if (member.MemberType== MemberTypes.Field)
                    {

                    }
                }
            }
            throw new NotImplementedException();
        }
    }

    public class JsonZ
    {
        public Dictionary<string, string> Key { get; set; }
        public string Obj { get; set; }
    }

}
