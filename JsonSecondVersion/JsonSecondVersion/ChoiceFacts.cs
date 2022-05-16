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
        public void SuccessIsGivenACharacterAndARangeThatMatchShouldReturnTrue()
        {
            var digit = new Choice(new Character('0'), new Range('1', '9'));
            Assert.True(digit.Match("012").Success());
        }

        [Fact]
        public void SuccessIsGivenACharacterAndARangeThatDontMatchShouldReturnFalse()
        {
            var digit = new Choice(new Character('0'), new Range('1', '9'));
            Assert.False(digit.Match("dab").Success());
        }

        [Fact]
        public void SuccessIsGivenACharacterAndTwoRangesThatMatchShouldReturnTrue()
        {
            var digit = new Choice(new Character('0'), new Range('1', '9'));
            var hex = new Choice(digit, new Range('a', 'f'), new Range('A', 'F'));
            Assert.True(hex.Match("afb").Success());
        }

        [Fact]
        public void SuccessIsGivenACharacterAndTwoRangesThatDontMatchShouldReturnFalse()
        {
            var digit = new Choice(new Character('0'), new Range('1', '9'));
            var hex = new Choice(digit, new Range('a', 'f'), new Range('A', 'F'));
            Assert.False(hex.Match("vfb").Success());
        }

        [Fact]
        public void RemainingTextIsGivenACharacterAndTwoRangesThatMatchShouldReturnEmptyRange()
        {
            var digit = new Choice(new Character('0'), new Range('1', '9'));
            var hex = new Choice(digit, new Range('a', 'f'), new Range('A', 'F'));
            Assert.Equal("v", hex.Match("1bv").RemainingText());
        }

        [Fact]
        public void RemainingTextIsGivenACharacterAndTwoRangesThatDontMatchShouldReturnRange()
        {
            var digit = new Choice(new Character('0'), new Range('1', '9'));
            var hex = new Choice(digit, new Range('a', 'f'), new Range('A', 'F'));
            Assert.Equal("278", hex.Match("v278").RemainingText());
        }
    }
}
