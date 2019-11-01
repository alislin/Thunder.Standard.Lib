using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Thunder.Standard.Lib.IO
{
    public interface ICompress
    {
        EncoderType Type { get; }

        byte[] Encode(byte[] src, int level = 0);
        byte[] Decode(byte[] src);

        Stream Encode(Stream src, int level = 0);
        Stream Decode(Stream src);
    }
}
