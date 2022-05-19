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
        public void TextLengthIsGivenCCharToMatchShouldReturnSuccessMatch()
        {
            Character character = new Character('c');
            const string text = "csd";
            Assert.Equal("sd", character.Match(text).RemainingText(text));
        }

        [Fact]
        public void TextLengthIsGivenARandomCharToMatchShouldReturnFailedMatch()
        {
            Character character = new Character('a');
            const string text = "has";
            Assert.Equal("has", character.Match(text).RemainingText(text));
        }
    }
}
