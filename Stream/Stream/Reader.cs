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
            using (StreamReader reader = gzip
                ? new (new GZipStream(stream, CompressionMode.Decompress, true), leaveOpen: true)
                : new (stream, leaveOpen: true))
            {
                return reader.ReadToEnd();
            }
        }
    }
}
