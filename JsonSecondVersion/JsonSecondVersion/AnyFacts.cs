using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace JsonSecondVersion
{
    public class AnyFacts
    {
        [Fact]
        public void AnyIsGivenEToMatchShouldReturnTrue()
        {
            var e = new Any("eE");
            Assert.True(e.Match("ea").Success());
            Assert.True(e.Match("Ea").Success());
        }

        [Fact]
        public void AnyIsGivenEToMatchShouldReturnRemainingText()
        {
            var e = new Any("eE");
            Assert.Equal("a", e.Match("ea").RemainingText());
            Assert.Equal("a", e.Match("Ea").RemainingText());
        }

        [Fact]
        public void AnyIsGivenPlusAndMinusSIgnsToMatchShouldReturnTrue()
        {
            var sign = new Any("+-");
            Assert.True(sign.Match("+3").Success());
            Assert.True(sign.Match("-2").Success());
        }

        [Fact]
        public void AnyIsGivenPlusAndMinusSIgnsToMatchShouldReturnRemainingText()
        {
            var sign = new Any("+-");
            Assert.Equal("3", sign.Match("+3").RemainingText());
            Assert.Equal("2", sign.Match("-2").RemainingText());
        }
    }
}
