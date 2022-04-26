using Xunit;

namespace JsonSecondVersion
{
    public class RangeFacts
    {
        [Fact]
        public void RangeIsGivenACharacterOutsideTheIntervalShouldReturnFalse()
        {
            const char start = 'a';
            const char end = 'f';
            const string text = "1ab";
            Range range = new Range(start, end);
            Assert.False(range.Match(text));
        }

        [Fact]
        public void RangeIsGivenAcharacterInsideTheIntervalShouldReturnTrue()
        {
            const char start = 'a';
            const char end = 'f';
            const string text = "abc";
            Range range = new Range(start, end);
            Assert.True(range.Match(text));
        }
    }
}