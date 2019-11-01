using System.IO;

namespace Thunder.Standard.Lib.IO
{
    /// <summary>
    /// 不压缩,仅复制(特殊环境,无法使用其他压缩算法时)
    /// </summary>
    public class NoneCompress : ICompress
    {
        public EncoderType Type => EncoderType.None;

        public byte[] Decode(byte[] src)
        {
            var result = new byte[src.Length];
            src.CopyTo(result, 0);
            return result;
        }

        public Stream Decode(Stream src)
        {
            var result = new MemoryStream();
            src.CopyTo(result);
            return result;
        }

        public byte[] Encode(byte[] src, int level = 0)
        {
            var result = new byte[src.Length];
            src.CopyTo(result, 0);
            return result;
        }

        public Stream Encode(Stream src, int level = 0)
        {
            var result = new MemoryStream();
            src.CopyTo(result);
            return result;
        }
    }
}
