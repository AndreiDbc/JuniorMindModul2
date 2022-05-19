using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace JsonSecondVersion
{
    public class RangeFacts
    {
        [Fact]
        public void TextLengthIsGivenACharacterOutsideTheIntervalShouldReturnFalse()
        {
            const char start = 'a';
            const char end = 'f';
            const string text = "1ab";
            Range range = new Range(start, end);
            Assert.Equal("1ab", range.Match(text).RemainingText(text));
        }

        [Fact]
        public void TextLengthIsGivenAcharacterInsideTheIntervalShouldReturnTrue()
        {
            const char start = 'a';
            const char end = 'f';
            const string text = "abc";
            Range range = new Range(start, end);
            Assert.Equal("bc", range.Match(text).RemainingText(text));
        }
    }
}