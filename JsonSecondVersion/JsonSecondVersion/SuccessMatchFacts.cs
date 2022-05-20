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
            Assert.Equal("c", match.RemainingText());
        }

        [Fact]
        public void TextLengthIsGivenMultipleCharactersShouldReturnRange()
        {
            SuccessMatch match = new SuccessMatch("csada");
            Assert.Equal("csada", match.RemainingText());
        }

        [Fact]
        public void SuccessIsGivenANonMatchShouldReturnFalse()
        {
            SuccessMatch text = new SuccessMatch("ASD");
            Assert.True(text.Success());
        }
    }
}
