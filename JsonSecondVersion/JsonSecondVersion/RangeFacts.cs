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
        public void MatchIsGivenACharacterOutsideTheIntervalShouldReturnFalse()
        {
            const char start = 'a';
            const char end = 'f';
            const string text = "1ab";
            Range range = new Range(start, end);
            Assert.False(range.Match(text).Success());
        }

        [Fact]
        public void SuccessIsGivenAcharacterInsideTheIntervalShouldReturnTrue()
        {
            const char start = 'a';
            const char end = 'f';
            const string text = "abc";
            Range range = new Range(start, end);
            Assert.True(range.Match(text).Success());
        }

        [Fact]
        public void RemainingTextIsGivenAcharacterInsideTheIntervalShouldReturnEmptyString()
        {
            const char start = 'a';
            const char end = 'f';
            const string text = "abc";
            Range range = new Range(start, end);
            Assert.Equal("", range.Match(text).RemainingText());
        }

        [Fact]
        public void RemainingTextIsGivenAcharacterOutsideTheIntervalShouldReturnRange()
        {
            const char start = 'a';
            const char end = 'f';
            const string text = "lbc";
            Range range = new Range(start, end);
            Assert.Equal("bc", range.Match(text).RemainingText());
        }
    }
}