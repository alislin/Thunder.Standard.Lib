using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Thunder.Standard.Lib.IO;
using Thunder.Standard.Lib.Extension;
using System.Diagnostics;

namespace Thunder.Standard.Lib.Model.SharpZip
{
    public class LargeObject : LargeObject<IO.SharpZip>
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

}
