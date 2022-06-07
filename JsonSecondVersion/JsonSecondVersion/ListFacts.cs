using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace JsonSecondVersion
{
    public class ListFacts
    {
        [Fact]
        public void ListIsGivenAcharacterAndARangeShouldReturnTrue()
        {
            var a = new List(new Range('0', '9'), new Character(','));
            Assert.True(a.Match("1,2,3").Success());
        }

        [Fact]
        public void ListIsGivenAcharacterAndARangeShouldReturnRemainingText()
        {
            var a = new List(new Range('0', '9'), new Character(','));
            Assert.Equal("", a.Match("1,2,3").RemainingText());
        }

        [Fact]
        public void ListIsGivenDigitsWhiteSpaceAndSeparatorShouldReturnRemainingText()
        {
            var digits = new OneOrMore(new Range('0', '9'));
            var whitespace = new Many(new Any(" \r\n\t"));
            var separator = new Sequence(whitespace, new Character(';'), whitespace);
            var list = new List(digits, separator);
            Assert.Equal("", list.Match("1; 22  ;\n 333 \t; 22").RemainingText());
        }

        [Fact]
        public void ListIsGivenDigitsWhiteSpaceAndSeparatorShouldReturnTrue()
        {
            var digits = new OneOrMore(new Range('0', '9'));
            var whitespace = new Many(new Any(" \r\n\t"));
            var separator = new Sequence(whitespace, new Character(';'), whitespace);
            var list = new List(digits, separator);
            Assert.True(list.Match("1; 22  ;\n 333 \t; 22").Success());
        }

        [Fact]
        public void ListIsEmptyShouldReturnTrue()
        {
            var digits = new OneOrMore(new Range('0', '9'));
            var whitespace = new Many(new Any(" \r\n\t"));
            var separator = new Sequence(whitespace, new Character(';'), whitespace);
            var list = new List(digits, separator);
            Assert.True(list.Match("").Success());
        }

        [Fact]
        public void ListIsGivenTwoConsecutiveSeparatorsShouldReturnRemainingText()
        {
            var digits = new OneOrMore(new Range('0', '9'));
            var whitespace = new Many(new Any(" \r\n\t"));
            var separator = new Sequence(whitespace, new Character(';'), whitespace);
            var list = new List(digits, separator);
            Assert.Equal("; 333 \t; 22", list.Match("1; 22 ;; 333 \t; 22").RemainingText());
        }

        [Fact]
        public void ListIsGivenTwoConsecutiveNumbersShouldReturnRemainingText()
        {
            var digits = new OneOrMore(new Range('0', '9'));
            var whitespace = new Many(new Any(" \r\n\t"));
            var separator = new Sequence(whitespace, new Character(';'), whitespace);
            var list = new List(digits, separator);
            Assert.Equal(" 333 \t; 22", list.Match("1; 22 333 \t; 22").RemainingText());
        }
    }
}
