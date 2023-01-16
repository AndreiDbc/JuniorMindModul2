using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Linq
{
    public class StringExtensionsFacts
    {
        [Fact]
        public void CountWordsIsCalledShouldReturnListOfWords()
        {
            const string text = "a a f s.a g s";
            var result = text.CountWords();
            Assert.Equal(3, result.ElementAt(0).Count);
            Assert.Equal("a", result.ElementAt(0).Word);
            Assert.Equal(2, result.ElementAt(1).Count);
            Assert.Equal("s", result.ElementAt(1).Word);
            Assert.Equal(1, result.ElementAt(2).Count);
            Assert.Equal("f", result.ElementAt(2).Word);
            Assert.Equal(1, result.ElementAt(3).Count);
            Assert.Equal("g", result.ElementAt(3).Word);
        }

        [Fact]
        public void CountWordsIsCalledNoOfWordsIsThreeShouldReturnListOfWords()
        {
            const string text = "a a f s.a g s";
            var result = text.CountWords(3);
            Assert.Equal(3, result.ElementAt(0).Count);
            Assert.Equal("a", result.ElementAt(0).Word);
            Assert.Equal(2, result.ElementAt(1).Count);
            Assert.Equal("s", result.ElementAt(1).Word);
            Assert.Equal(1, result.ElementAt(2).Count);
            Assert.Equal("f", result.ElementAt(2).Word);
            Assert.Equal(3, result.Count());
        }
    }
}
