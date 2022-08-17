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

        public void WriteText(string text, bool gzip)
        {
            using (StreamWriter writer = gzip
                ? new (new GZipStream(stream, CompressionLevel.SmallestSize, true), leaveOpen: true)
                : new (stream, leaveOpen: true))
            {
                writer.Write(text);
            }
        }
    }
}
