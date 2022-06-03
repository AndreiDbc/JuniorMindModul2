using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace JsonSecondVersion
{
    public class OneOrMoreFacts
    {
        [Fact]
        public void OneOrMoreIsGivenARangeShouldReturnTrue()
        {
            var a = new OneOrMore(new Range('0', '9'));
            Assert.True(a.Match("123").Success());
        }

        [Fact]
        public void OneOrMoreIsGivenARangeShouldReturnEmptyString()
        {
            var a = new OneOrMore(new Range('0', '9'));
            Assert.Equal("", a.Match("123").RemainingText());
        }

        [Fact]
        public void OneOrMoreIsGivenACharacterShouldReturnRemainingText()
        {
            var digits = new Many(new Range('0', '9'));
            Assert.Equal("ab", digits.Match("12345ab123").RemainingText());
        }
    }
}
