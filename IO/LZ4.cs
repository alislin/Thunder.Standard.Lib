using System;
using System.IO;
using K4os.Compression.LZ4;
using K4os.Compression.LZ4.Streams;
using Thunder.Standard.Lib.Extension;

namespace Thunder.Standard.Lib.IO
{
    /// <summary>
    /// LZ4压缩库
    /// </summary>
    public class LZ4 : ICompress
    {
        public EncoderType Type => EncoderType.LZ4;

        public byte[] Decode(byte[] src)
        {
            var MaxLength = 100 * 1024 * 1024;
            var len = src.Length * 255;
            len = len > MaxLength ? MaxLength : len;
            var target = new byte[len];
            var decoded = LZ4Codec.Decode(src, 0, src.Length, target, 0, target.Length);
            if (decoded <= 0)
            {
                return null;
            }
            var result = new byte[decoded];
            Array.Copy(target, 0, result, 0, result.Length);
            return result;
        }

        public Stream Decode(Stream src)
        {
            var target = LZ4Stream.Decode(src);
            return target;
        }

        public byte[] Encode(byte[] src, int level = 0)
        {
            var lz = GetLevel(level);
            var target= new byte[LZ4Codec.MaximumOutputSize(src.Length)];
            var encodedLength = LZ4Codec.Encode(src, 0, src.Length, target, 0, target.Length,lz);
            var result = new byte[encodedLength];
            Array.Copy(target, 0, result, 0, result.Length);
            return result;
        }

        public Stream Encode(Stream src, int level = 0)
        {
            var lz = GetLevel(level);
            var target = LZ4Stream.Encode(src, lz);
            return target;
        }

        private LZ4Level GetLevel(int level)
        {
            var l = LZ4Level.L00_FAST;
            l = level.ToEnum<LZ4Level>();
            return l;
        }
    }
}
