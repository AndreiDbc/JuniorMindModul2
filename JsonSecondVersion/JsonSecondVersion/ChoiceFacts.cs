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
        public void ChoiceIsGivenACharacterAndARangeThatMatchShouldReturnTrue()
        {
            Character character = new Character('1');
            Range range = new Range('1', '9');
            Choice choice = new Choice(character, range);
            Assert.True(choice.Success("1as"));
        }

        [Fact]
        public void ChoiceIsGivenACharacterAndARangeThatDontMatchShouldReturnFalse()
        {
            Character character = new Character('b');
            Range range = new Range('1', '9');
            Choice choice = new Choice(character, range);
            Assert.False(choice.Success("1as"));
        }

        [Fact]
        public void SuccessIsGivenACharacterAndTwoRangesThatMatchShouldReturnTrue()
        {
            Character character = new Character('2');
            Range range = new Range('1', '9');
            var hex = new Choice(character, new Choice(new Range('a', 'f'), new Range('A', 'F')));
            Choice choice = new Choice(character, range);
            Assert.True(choice.Success("278"));
        }

        [Fact]
        public void SuccessIsGivenACharacterAndTwoRangesThatDontMatchShouldReturnFalse()
        {
            Character character = new Character('0');
            Range range = new Range('1', '9');
            var hex = new Choice(character, new Choice(new Range('a', 'f'), new Range('A', 'F')));
            Choice choice = new Choice(character, range);
            Assert.False(choice.Success("h78"));
        }

        [Fact]
        public void SuccessIsGivenACharacterAndTwoRangesThatMatchShouldReturnRange()
        {
            Character character = new Character('1');
            Range range = new Range('1', '9');
            var hex = new Choice(character, new Choice(new Range('a', 'f'), new Range('A', 'F')));
            Choice choice = new Choice(character, range);
            Assert.Equal("ab", choice.RemainingText("1ab"));
        }

        [Fact]
        public void SuccessIsGivenACharacterAndTwoRangesThatDontMatchShouldReturnText()
        {
            Character character = new Character('0');
            Range range = new Range('1', '9');
            var hex = new Choice(character, new Choice(new Range('a', 'f'), new Range('A', 'F')));
            Choice choice = new Choice(character, range);
            Assert.Equal("278", choice.RemainingText("278"));
        }
    }
}
