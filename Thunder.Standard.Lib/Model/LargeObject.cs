using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Thunder.Standard.Lib.IO;
using Thunder.Standard.Lib.Extension;
using System.Diagnostics;

namespace Thunder.Standard.Lib.Model
{
    public class LargeObject : LargeObject<LZ4>
    {
        public LargeObject()
        {
        }

        public LargeObject(object obj) : this(obj,10)
        {
        }

        public LargeObject(object obj, int level) : base(obj, level)
        {
        }
    }

    public class LargeObject<TEncoder> : ILargeObject where TEncoder:ICompress,new()
    {
        public string Type { get; set; }
        public string BufferString { get; set; }
        public EncoderType EncoderType { get; set; }
        private byte[] Buffer => System.Convert.FromBase64String(BufferString);

        public LargeObject() {  }
        public LargeObject(object obj) : this(obj, 0) { }
        public LargeObject(object obj, int level) 
        {
            EncoderType = (new TEncoder()).Type;
            var type = obj.GetType();
            Type = type.FullName;
            var s = obj.ToJson();
            var buf = Encoding.UTF8.GetBytes(s);
            var result = buf.Encode<TEncoder>(level);
            BufferString = System.Convert.ToBase64String(result);
        }

        public T Convert<T>()
        {
            byte[] buf = null;
            switch (EncoderType)
            {
                case EncoderType.SharpZip:
                    buf = Buffer.Decode<SharpZip>();
                    break;
                case EncoderType.GZip:
                    buf = Buffer.Decode<GZip>();
                    break;
                case EncoderType.LZ4:
                    buf = Buffer.Decode<LZ4>();
                    break;
                case EncoderType.None:
                    buf = Buffer.Decode<NoneCompress>();
                    break;
                case EncoderType.Custom:
                default:
                    buf = Buffer.Decode<TEncoder>();
                    break;
            }

            var s = Encoding.UTF8.GetString(buf);
            var result = s.FromJson<T>();
            return result;
        }

    }

    public static class LargeObjectExtension
    {
        public static LargeObject<TEncoder> ToLargeObject<TEncoder>(this object obj, int level=10) where TEncoder : ICompress, new()
        {
            var lg = new LargeObject<TEncoder>(obj,level);
            return lg;
        }

        public static T LoadFromLargeObject<T, TEncoder>(this LargeObject<TEncoder> lg) where TEncoder : ICompress, new()
        {
            return lg.Convert<T>();
        }

        public static LargeObject ToLargeObject(this object obj, int level = 10)
        {
            var lg = new LargeObject(obj, level);
            return lg;
        }

        public static T LoadFromLargeObject<T>(this ILargeObject lg)
        {
            return lg.Convert<T>();
        }

    }
}
