using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace JsonSecondVersion
{
    public class TextFacts
    {
        [Fact]
        public void TextIsGivenAMatchShouldReturnTrue()
        {
            var text1 = new Text("false");
            var text2 = new Text("falseX");
            Assert.True(text1.Match("false").Success());
            Assert.True(text1.Match("falseX").Success());
        }

        [Fact]
        public void TextIsGivenAMatchShouldReturnRemainingText()
        {
            var text1 = new Text("false");
            var text2 = new Text("falseX");
            Assert.Equal("", text1.Match("false").RemainingText());
            Assert.Equal("X", text1.Match("falseX").RemainingText());
        }
    }
}
