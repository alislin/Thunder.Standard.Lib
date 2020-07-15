using ICSharpCode.SharpZipLib.BZip2;
using System;
using System.IO;

namespace Thunder.Standard.Lib.IO
{
    /// <summary>
    /// SharpZipLib 库
    /// </summary>
    public class SharpZip : ICompress
    {
        public EncoderType Type => EncoderType.SharpZip;

        public byte[] Decode(byte[] src)
        {
            byte[] result = null;
            MemoryStream m_msBZip2 = null;
            BZip2InputStream m_isBZip2 = null;
            try
            {
                m_msBZip2 = new MemoryStream(src);
                // read final uncompressed string size stored in first 4 bytes
                //
                using (BinaryReader reader = new BinaryReader(m_msBZip2, System.Text.Encoding.ASCII))
                {
                    Int32 size = reader.ReadInt32();

                    m_isBZip2 = new BZip2InputStream(m_msBZip2);
                    byte[] bytesUncompressed = new byte[size];
                    m_isBZip2.Read(bytesUncompressed, 0, bytesUncompressed.Length);
                    m_isBZip2.Close();
                    m_msBZip2.Close();

                    result = bytesUncompressed;

                    reader.Close();
                }
            }
            finally
            {
                if (m_isBZip2 != null)
                {
                    m_isBZip2.Dispose();
                }
                if (m_msBZip2 != null)
                {
                    m_msBZip2.Dispose();
                }
            }
            return result;
        }

        public Stream Decode(Stream src)
        {
            throw new System.NotImplementedException();
        }

        public byte[] Encode(byte[] src, int level = 0)
        {
            MemoryStream m_msBZip2 = null;
            BZip2OutputStream m_osBZip2 = null;
            byte[] result = null;
            try
            {
                m_msBZip2 = new MemoryStream();
                Int32 size = src.Length;
                // Prepend the compressed data with the length of the uncompressed data (firs 4 bytes)
                //
                using (BinaryWriter writer = new BinaryWriter(m_msBZip2, System.Text.Encoding.ASCII))
                {
                    writer.Write(size);
                    m_osBZip2 = new BZip2OutputStream(m_msBZip2);
                    m_osBZip2.Write(src, 0, src.Length);

                    m_osBZip2.Close();
                    result = m_msBZip2.ToArray();
                    m_msBZip2.Close();

                    writer.Close();
                }
            }
            finally
            {
                if (m_osBZip2 != null)
                {
                    m_osBZip2.Dispose();
                }
                if (m_msBZip2 != null)
                {
                    m_msBZip2.Dispose();
                }
            }
            return result;
        }

        public Stream Encode(Stream src, int level = 0)
        {
            throw new System.NotImplementedException();
        }
    }
}
