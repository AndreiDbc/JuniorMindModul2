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
        public void TextLengthIsGivenACharacterOutsideTheIntervalShouldReturnFailedMatch()
        {
            const char start = 'a';
            const char end = 'f';
            const string text = "1ab";
            Range range = new Range(start, end);
            Assert.Equal("1ab", range.Match(text).RemainingText());
        }

        [Fact]
        public void TextLengthIsGivenACharacterInsideTheIntervalShouldReturnSuccessMatch()
        {
            const char start = 'a';
            const char end = 'f';
            const string text = "abc";
            Range range = new Range(start, end);
            Assert.Equal("bc", range.Match(text).RemainingText());
        }

        [Fact]
        public void SuccessIsGivenAMatchShouldReturnSuccessMatch()
        {
            Range range = new Range('a', 'f');
            Assert.True(range.Match("cdf").Success());
            Assert.Equal("df", range.Match("cdf").RemainingText());
        }

        [Fact]
        public void SuccessIsGivenANonMatchShouldReturnFailedMatch()
        {
            Range range = new Range('a', 'f');
            Assert.False(range.Match("523").Success());
            Assert.Equal("523", range.Match("523").RemainingText());
        }
    }
}