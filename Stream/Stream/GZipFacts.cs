using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Str
{
     public class GZipFacts
     {
        [Fact]
        public void CompressIsCalledShouldReturnCompressedStream()
        {
            var stream = new MemoryStream();
            var writer = new Writer(stream);
            writer.WriteText();
            GZip test = new GZip(stream);
            test.Compress();
            stream.Seek(0, SeekOrigin.Begin);
            Reader reader = new Reader(stream);
            Assert.Equal("", reader.ReadText());
        }
     }
}
