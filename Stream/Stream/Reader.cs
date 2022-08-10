using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Str
{
    class Reader
    {
        readonly Stream stream;

        public Reader(Stream stream)
        {
            this.stream = stream;
        }

        public string ReadText()
        {
            using (StreamReader reader = new (stream, Encoding.Default, true, 512, true))
            {
                return reader.ReadToEnd();
            }
        }
    }
}
