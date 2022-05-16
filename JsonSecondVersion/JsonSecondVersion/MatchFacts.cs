using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace JsonSecondVersion
{
    public class MatchFacts
    {
        [Fact]
        public void MatchIsGivenOneCharacterShouldReturnTrue()
        {
            Match match = new Match("c");
            Assert.True(match.Success());
        }

        [Fact]
        public void MatchIsGivenMultipleCharactersShouldReturnFalse()
        {
            Match match = new Match("csada");
            Assert.False(match.Success());
        }

        [Fact]
        public void MatchIsGivenAStringShouldReturnRemainingCharacters()
        {
            Match match = new Match("csada");
            Assert.Equal("sada", match.RemainingText());
        }
    }
}
