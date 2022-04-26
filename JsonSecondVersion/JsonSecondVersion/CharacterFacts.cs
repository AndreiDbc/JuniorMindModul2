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
        public void ChracterIsGivenCCharToMatchShouldReturnTrue()
        {
            Character character = new Character('c');
            const string text = "csd";
            Assert.True(character.Match(text));
        }

        [Fact]
        public void ChracterIsGivenARandomCharToMatchShouldReturnFalse()
        {
            Character character = new Character('a');
            const string text = "hsd";
            Assert.False(character.Match(text));
        }
    }
}
