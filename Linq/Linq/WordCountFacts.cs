using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Linq
{
    public class WordCountFacts
    {
        [Fact]
        public void AnInstanceOfWordCountIsCreatedShouldReturnObject()
        {
            var test = new WordCount() { Word = "as", Count = 1 };
            Assert.Equal("as", test.Word);
            Assert.Equal(1, test.Count);
        }
    }
}
