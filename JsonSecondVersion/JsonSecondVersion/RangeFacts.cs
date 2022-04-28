using Xunit;

namespace JsonSecondVersion
{
    public class RangeFacts
    {
        [Fact]
        public void SuccessIsGivenACharacterOutsideTheIntervalShouldReturnFalse()
        {
            const char start = 'a';
            const char end = 'f';
            const string text = "1ab";
            Range range = new Range(start, end);
            Assert.False(range.Success(text));
        }

        [Fact]
        public void SuccessIsGivenAcharacterInsideTheIntervalShouldReturnTrue()
        {
            const char start = 'a';
            const char end = 'f';
            const string text = "abc";
            Range range = new Range(start, end);
            Assert.True(range.Success(text));
        }

        [Fact]
        public void RemainingTextIsGivenAcharacterInsideTheIntervalShouldReturnRange()
        {
            const char start = 'a';
            const char end = 'f';
            const string text = "abc";
            Range range = new Range(start, end);
            Assert.Equal("bc", range.RemainingText(text));
        }

        [Fact]
        public void RemainingTextIsGivenAcharacterOutsideTheIntervalShouldReturnText()
        {
            const char start = 'a';
            const char end = 'f';
            const string text = "lbc";
            Range range = new Range(start, end);
            Assert.Equal("lbc", range.RemainingText(text));
        }
    }
}