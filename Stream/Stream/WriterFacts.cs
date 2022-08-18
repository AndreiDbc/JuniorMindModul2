using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Str
{
    public class WriterFacts
    {
        [Fact]
        public void WriteTextIsCalledShouldReturnText()
        {
            var stream = new MemoryStream();
            var writer = new Writer(stream);
            writer.WriteText("Junior Mind", false, false);
            stream.Seek(0, SeekOrigin.Begin);
            var reader = new Reader(stream);
            Assert.Equal("Junior Mind", reader.ReadText(false, false));
        }

        [Fact]
        public void WriteTextIsCalledStreamIsGzippedShouldReturnText()
        {
            var stream = new MemoryStream();
            var writer = new Writer(stream);
            writer.WriteText("abc", true, false);
            stream.Seek(0, SeekOrigin.Begin);
            var reader = new Reader(stream);
            Assert.Equal("abc", reader.ReadText(true, false));
        }

        [Fact]
        public void WriteTextIsCalledStreamIsCryptedShouldReturnText()
        {
            var stream = new MemoryStream();
            var writer = new Writer(stream);
            writer.WriteText("abc", false, true);
            stream.Seek(0, SeekOrigin.Begin);
            var reader = new Reader(stream);
            Assert.Equal("abc", reader.ReadText(false, true));
        }
    }
}