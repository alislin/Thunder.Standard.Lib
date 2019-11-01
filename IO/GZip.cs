using System;
using System.IO;
using System.IO.Compression;

namespace Thunder.Standard.Lib.IO
{
    public class GZip : ICompress
    {
        public EncoderType Type => EncoderType.GZip;

        public byte[] Decode(byte[] src)
        {
            byte[] buf = null;
            if (src == null)
            {
                return buf;
            }
            try
            {
                MemoryStream input = new MemoryStream(src, 0, src.Length);
                GZipStream gzip = new GZipStream(input, CompressionMode.Decompress, true);

                MemoryStream output = new MemoryStream();
                byte[] buff = new byte[4096];
                int read = -1;
                read = gzip.Read(buff, 0, buff.Length);
                while (read > 0)
                {
                    output.Write(buff, 0, read);
                    read = gzip.Read(buff, 0, buff.Length);
                }
                gzip.Close();
                buf = output.ToArray();
                output.Close();
                input.Close();
            }
            catch (Exception)
            {
                buf = null;
            }
            return buf;
        }

        public Stream Decode(Stream src)
        {
            GZipStream gzip = new GZipStream(src, CompressionMode.Decompress, true);
            return gzip;
        }

        public byte[] Encode(byte[] src, int level = 0)
        {
            byte[] buf = null;
            if (src == null)
            {
                return buf;
            }
            try
            {
                MemoryStream ms = new MemoryStream();
                GZipStream zipstream = new GZipStream(ms, CompressionMode.Compress, true);
                zipstream.Write(src, 0, src.Length);
                zipstream.Close();
                zipstream.Dispose();
                buf = ms.ToArray();
            }
            catch (Exception)
            {
                buf = null;
            }
            return buf;
        }

        public Stream Encode(Stream src, int level = 0)
        {
            GZipStream gzip = new GZipStream(src, CompressionMode.Compress, true);
            return gzip;
        }
    }
}
