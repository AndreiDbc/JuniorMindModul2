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
            Assert.Equal("12", digit.Match("012").RemainingText());
        }

        [Fact]
        public void TextLengthIsGivenACharacterAndARangeThatDontMatchShouldReturnFailedMatch()
        {
            var digit = new Choice(new Character('0'), new Range('1', '9'));
            Assert.Equal("dab", digit.Match("dab").RemainingText());
        }

        [Fact]
        public void TextLengthIsGivenACharacterAndTwoRangesThatMatchShouldReturnSuccesMatch()
        {
            var digit = new Choice(new Character('0'), new Range('1', '9'));
            var hex = new Choice(digit, new Range('a', 'f'), new Range('A', 'F'));
            Assert.Equal("fb", hex.Match("afb").RemainingText());
        }

        [Fact]
        public void SuccessIsGivenAMatchShouldReturnSuccessMatch()
        {
            var digit = new Choice(new Character('0'), new Range('1', '9'));
            var hex = new Choice(digit, new Range('a', 'f'), new Range('A', 'F'));
            Assert.True(hex.Match("afb").Success());
            Assert.Equal("fb", hex.Match("afb").RemainingText());
        }

        [Fact]
        public void SuccessIsGivenANonMatchShouldReturnFailedMatch()
        {
            var digit = new Choice(new Character('0'), new Range('1', '9'));
            var hex = new Choice(digit, new Range('a', 'f'), new Range('A', 'F'));
            Assert.False(hex.Match("lfb").Success());
            Assert.Equal("lfb", hex.Match("lfb").RemainingText());
        }
    }
}