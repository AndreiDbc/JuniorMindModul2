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
        public void OneOrMoreIsGivenARangeShouldReturnRemainingText()
        {
            var digits = new OneOrMore(new Range('0', '9'));
            Assert.Equal("ab123", digits.Match("12345ab123").RemainingText());
        }

        [Fact]
        public void OneOrMoreIsGivenARangeShouldReturnFalse()
        {
            var digits = new OneOrMore(new Range('0', '9'));
            Assert.False(digits.Match("a2345ab123").Success());
        }

        [Fact]
        public void OneOrMoreIsGivenATextShouldReturnRemainingText()
        {
            var text = new OneOrMore(new Text("as"));
            Assert.Equal("avg", text.Match("asasasavg").RemainingText());
        }

        [Fact]
        public void OneOrMoreIsGivenATextShouldReturnTrue()
        {
            var text = new OneOrMore(new Text("as"));
            Assert.True(text.Match("asasasavg").Success());
        }
    }
}
