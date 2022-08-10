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
            using StreamWriter sr = File.AppendText(path);
            sr.Write("Junior Mind");
            sr.Close();
        }
    }
}
