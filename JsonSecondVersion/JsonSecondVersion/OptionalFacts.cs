using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace JsonSecondVersion
{
    public class OptionalFacts
    {
        [Fact]
        public void OptionalIsGivenAMatchShouldReturnTrue()
        {
            var a = new Optional(new Character('a'));
            var sign = new Optional(new Character('-'));
            Assert.True(a.Match("abc").Success());
            Assert.True(sign.Match("-123").Success());
        }

        [Fact]
        public void OptionalIsGivenAMatchShouldReturnRemainingText()
        {
            var a = new Optional(new Character('a'));
            var sign = new Optional(new Character('-'));
            Assert.Equal("bc", a.Match("abc").RemainingText());
            Assert.Equal("123", sign.Match("-123").RemainingText());
        }

        [Fact]
        public void OptionalIsGivenANonMatchShouldReturnTrue()
        {
            var a = new Optional(new Character('a'));
            var sign = new Optional(new Character('-'));
            Assert.True(a.Match("bc").Success());
            Assert.True(sign.Match("123").Success());
        }

        [Fact]
        public void OptionalIsGivenANonMatchShouldReturnText()
        {
            var a = new Optional(new Character('a'));
            var sign = new Optional(new Character('-'));
            Assert.Equal("bc", a.Match("bc").RemainingText());
            Assert.Equal("123", sign.Match("123").RemainingText());
        }
    }
}
