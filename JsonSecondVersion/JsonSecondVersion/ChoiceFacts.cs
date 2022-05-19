using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace JsonSecondVersion
{
    public class ChoiceFacts
    {
        [Fact]
        public void TextLengthIsGivenACharacterAndARangeThatMatchShouldReturnSuccesMatch()
        {
            var digit = new Choice(new Character('0'), new Range('1', '9'));
            Assert.Equal("12", digit.Match("012").RemainingText("012"));
        }

        [Fact]
        public void TextLengthIsGivenACharacterAndARangeThatDontMatchShouldReturnFailedMatch()
        {
            var digit = new Choice(new Character('0'), new Range('1', '9'));
            Assert.Equal("dab", digit.Match("dab").RemainingText("dab"));
        }

        [Fact]
        public void TextLengthIsGivenACharacterAndTwoRangesThatMatchShouldReturnSuccesMatch()
        {
            var digit = new Choice(new Character('0'), new Range('1', '9'));
            var hex = new Choice(digit, new Range('a', 'f'), new Range('A', 'F'));
            Assert.Equal("fb", hex.Match("afb").RemainingText("afb"));
        }
    }
}
