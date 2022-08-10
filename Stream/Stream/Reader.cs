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
            string result = "";
            using (StreamReader sr = new StreamReader(path))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    result = line + "\n";
                }
            }

            return result[0..^1];
        }
    }
}
