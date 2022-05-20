using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace JsonSecondVersion
{
    public class SuccessMatchFacts
    {
        [Fact]
        public void TextLengthIsGivenOneCharacterShouldReturnEmptyString()
        {
            SuccessMatch match = new SuccessMatch("c");
            Assert.Equal("", match.RemainingText("c"));
        }

        [Fact]
        public void TextLengthIsGivenMultipleCharactersShouldReturnRange()
        {
            SuccessMatch match = new SuccessMatch("csada");
            Assert.Equal("sada", match.RemainingText("csada"));
        }

        [Fact]
        public void SuccessIsGivenANonMatchShouldReturnFalse()
        {
            SuccessMatch text = new SuccessMatch("ASD");
            Assert.True(text.Success("SD"));
        }

        [Fact]
        public void SuccessIsGivenAMatchShouldReturnTrue()
        {
            SuccessMatch text = new SuccessMatch("ASD");
            Assert.False(text.Success("ASD"));
        }
    }
}
