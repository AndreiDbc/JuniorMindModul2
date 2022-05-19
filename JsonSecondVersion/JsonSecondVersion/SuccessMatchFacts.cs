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
        public void TextLengthIsGivenOneCharacterShouldReturnTrue()
        {
            SuccessMatch match = new SuccessMatch("c");
            Assert.Equal("", match.RemainingText("c"));
        }

        [Fact]
        public void TextLengthIsGivenMultipleCharactersShouldReturnFalse()
        {
            SuccessMatch match = new SuccessMatch("csada");
            Assert.Equal("sada", match.RemainingText("csada"));
        }
    }
}
