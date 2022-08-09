using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stream
{
    class Writer
    {
        readonly string path;

        public Writer(string path)
        {
            this.path = path;
        }

        public void WriteText()
        {
            using StreamWriter sr = File.AppendText(path);
            sr.Write("Junior Mind");
            sr.Close();
        }
    }
}
