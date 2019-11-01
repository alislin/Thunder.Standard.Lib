using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Thunder.Standard.Lib.IO
{
    /// <summary>
    /// —πÀı/Ω‚—πÀı∑Ω∑®
    /// </summary>
    public static class CompressExt
    {
        public static byte[] Encode<T>(this byte[] src, int level = 0) where T : ICompress, new()
            => (new T()).Encode(src, level);

        public static Stream Encode<T>(this Stream src, int level = 0) where T : ICompress, new()
            => (new T()).Encode(src, level);

        public static byte[] Decode<T>(this byte[] src) where T : ICompress, new()
            => (new T()).Decode(src);

        public static Stream Decode<T>(this Stream src) where T : ICompress, new()
            => (new T()).Decode(src);

        public static byte[] Encode(this byte[] src, int level = 0)
            => src.Encode<GZip>(level);

        public static Stream Encode(this Stream src, int level = 0) 
            => src.Encode<GZip>(level);

        public static byte[] Decode(this byte[] src) 
            => src.Decode<GZip>();

        public static Stream Decode(this Stream src) 
            => src.Decode<GZip>();

    }
}
