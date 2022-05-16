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
            Assert.True(character.Match(text).Success());
        }

        [Fact]
        public void SuccessIsGivenARandomCharToMatchShouldReturnFalse()
        {
            Character character = new Character('a');
            const string text = "has";
            Assert.False(character.Match(text).Success());
        }

        [Fact]
        public void RemainingTextIsGivenCCharToMatchShouldReturnEmptyString()
        {
            Character character = new Character('c');
            const string text = "csd";
            Assert.Equal("", character.Match(text).RemainingText());
        }

        [Fact]
        public void RemainingTextIsGivenARandomCharToMatchShouldReturnRange()
        {
            Character character = new Character('n');
            const string text = "bfg";
            Assert.Equal("fg", character.Match(text).RemainingText());
        }
    }
}
