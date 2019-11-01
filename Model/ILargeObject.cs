using System;
using System.Collections.Generic;
using System.Text;
using Thunder.Standard.Lib.IO;

namespace Thunder.Standard.Lib.Model
{
    public interface ILargeObject
    {
        string Type { get; set; }
        string BufferString { get; set; }
        EncoderType EncoderType { get; set; }

        T Convert<T>();
    }
}
