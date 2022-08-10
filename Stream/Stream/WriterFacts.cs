using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Str
{
    public class WriterFacts
    {
        [Fact]
        public void WriteTextIsCalledSHouldModifyTextWithinFile()
        {
            const string path = @"C:\Users\Andrei\Desktop\Example2.txt";
            Writer writer = new Writer(path);
            writer.WriteText();
            Assert.Equal("Junior Mind", System.IO.File.ReadAllText(path));
        }
    }
}
