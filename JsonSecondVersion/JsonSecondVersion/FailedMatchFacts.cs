using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace JsonSecondVersion
{
    public class FailedMatchFacts
    {
        [Fact]
        public void RemainingTextIsGivenAStringShouldReturnFullText()
        {
            FailedMatch text = new FailedMatch("ASDF");
            Assert.Equal("ASDF", text.RemainingText("ASDF"));
        }

        [Fact]
        public void SuccessIsGivenANonMatchShouldReturnFalse()
        {
            FailedMatch text = new FailedMatch("ASD");
            Assert.False(text.Success("SD"));
        }

        [Fact]
        public void SuccessIsGivenAMatchShouldReturnTrue()
        {
            FailedMatch text = new FailedMatch("ASD");
            Assert.True(text.Success("ASD"));
        }
    }
}
