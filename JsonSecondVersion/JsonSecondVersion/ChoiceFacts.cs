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
            Assert.True(choice.Match("1as"));
        }

        [Fact]
        public void ChoiceIsGivenACharacterAndARangeThatDontMatchShouldReturnFalse()
        {
            Character character = new Character('b');
            Range range = new Range('1', '9');
            Choice choice = new Choice(character, range);
            Assert.False(choice.Match("1as"));
        }

        [Fact]
        public void ChoiceIsGivenACharacterAndTwoRangesThatMatchShouldReturnTrue()
        {
            Character character = new Character('2');
            Range range = new Range('1', '9');
            var hex = new Choice(character, new Choice(new Range('a', 'f'), new Range('A', 'F')));
            Choice choice = new Choice(character, range);
            Assert.True(choice.Match("278"));
        }

        [Fact]
        public void ChoiceIsGivenACharacterAndTwoRangesThatDontMatchShouldReturnFalse()
        {
            Character character = new Character('0');
            Range range = new Range('1', '9');
            var hex = new Choice(character, new Choice(new Range('a', 'f'), new Range('A', 'F')));
            Choice choice = new Choice(character, range);
            Assert.False(choice.Match("h78"));
        }
    }
}
