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
    }
}
