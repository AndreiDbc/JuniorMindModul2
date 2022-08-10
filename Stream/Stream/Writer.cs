using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Str
{
    class Writer
    {
        readonly Stream stream;

        public Writer(Stream stream)
        {
            this.stream = stream;
        }

        public void WriteText()
        {
            using (StreamWriter reader = new (stream, Encoding.Default, 512, true))
            {
                reader.Write("Junior Mind");
            }
        }
    }
}
