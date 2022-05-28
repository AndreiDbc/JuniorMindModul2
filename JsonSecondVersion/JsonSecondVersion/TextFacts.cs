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
            var text = new Text("false");
            Assert.True(text.Match("false").Success());
            Assert.True(text.Match("falseX").Success());
        }

        [Fact]
        public void TextIsGivenAMatchShouldReturnRemainingText()
        {
            var text = new Text("false");
            Assert.Equal("", text.Match("false").RemainingText());
            Assert.Equal("X", text.Match("falseX").RemainingText());
        }
    }
}
