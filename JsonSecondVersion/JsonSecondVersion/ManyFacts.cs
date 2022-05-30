using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace JsonSecondVersion
{
    public class ManyFacts
    {
        [Fact]
        public void ManyIsGivenAMatchShouldReturnTrue()
        {
            var a = new Many(new Character('a'));
            Assert.True(a.Match("aaaabc").Success());
        }

        [Fact]
        public void ManyIsGivenAMatchShouldReturnRemainingText()
        {
            var a = new Many(new Character('a'));
            Assert.Equal("bc", a.Match("aaaabc").RemainingText());
        }

        [Fact]
        public void ManyIsGivenARangeShouldReturnRemainingText()
        {
            var digits = new Many(new Range('0', '9'));
            Assert.Equal("ab123", digits.Match("12345ab123").RemainingText());
        }

        [Fact]
        public void ManyIsGivenARangeShouldReturnTrue()
        {
            var digits = new Many(new Range('0', '9'));
            Assert.True(digits.Match("12345ab123").Success());
        }

        [Fact]
        public void ManyIsGivenAEmptyStringShouldReturnEmptyStringInterface()
        {
            var digits = new Many(new Range('0', '9'));
            Assert.Equal("", digits.Match("").RemainingText());
        }

        [Fact]
        public void ManyIsGivenAEmptyStringShouldReturnTrue()
        {
            var digits = new Many(new Range('0', '9'));
            Assert.True(digits.Match("").Success());
        }
    }
}
