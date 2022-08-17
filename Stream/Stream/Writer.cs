using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
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

        public void WriteText(string text, bool gzipp)
        {
            if (gzipp)
            {
                byte[] textBytes = Encoding.ASCII.GetBytes(text);
                using (var compressor = new GZipStream(stream, CompressionLevel.SmallestSize, true))
                {
                    compressor.Write(textBytes, 0, textBytes.Length);
                }

                return;
            }

            using (StreamWriter reader = new (stream, Encoding.Default, 512, true))
            {
                reader.Write(text);
            }
        }
    }
}
