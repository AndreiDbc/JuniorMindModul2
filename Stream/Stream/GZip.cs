using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Str
{
    class GZip
    {
        private readonly Stream stream;

        public GZip(Stream stream)
        {
            this.stream = stream;
        }

        public void Compress()
        {
            Reader reader = new Reader(stream);
            byte[] inputBytes = Encoding.UTF8.GetBytes(reader.ReadText());
            using (var ms = new GZipStream(stream, CompressionMode.Compress, true))
            {
                stream.Seek(0, SeekOrigin.Begin);
                ms.Write(inputBytes, 0, inputBytes.Length);
            }
        }
    }
}
