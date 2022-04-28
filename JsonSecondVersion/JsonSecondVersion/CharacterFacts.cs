using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace JsonSecondVersion
{
    public class CharacterFacts
    {
        [Fact]
        public void SuccessIsGivenCCharToMatchShouldReturnTrue()
        {
            Character character = new Character('c');
            const string text = "csd";
            Assert.True(character.Success(text));
        }

        [Fact]
        public void SuccessIsGivenARandomCharToMatchShouldReturnFalse()
        {
            Character character = new Character('a');
            const string text = "hsd";
            Assert.False(character.Success(text));
        }

        [Fact]
        public void RemainingTextIsGivenCCharToMatchShouldReturnRange()
        {
            Character character = new Character('c');
            const string text = "csd";
            Assert.Equal("sd", character.RemainingText(text));
        }

        [Fact]
        public void RemainingTextIsGivenARandomCharToMatchShouldReturnText()
        {
            Character character = new Character('n');
            const string text = "csd";
            Assert.Equal(text, character.RemainingText(text));
        }
    }
}