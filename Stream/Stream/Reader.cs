using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
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

        public string ReadText(bool gzip)
        {
            if (gzip)
            {
                return GetGZipText();
            }

            using (StreamReader reader = new (stream, Encoding.Default, true, 512, true))
            {
                return reader.ReadToEnd();
            }
        }

        public string GetGZipText()
        {
            using (Stream gstream = new GZipStream(stream, CompressionMode.Decompress, true))
            {
                using (StreamReader reader = new (gstream, Encoding.Default, true, 512))
                {
                    return reader.ReadToEnd();
                }
            }
        }
    }
}
