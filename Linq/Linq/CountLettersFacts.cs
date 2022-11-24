using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Linq
{
    public class CountLettersFacts
    {
        [Fact]
        public void CountCalledShouldReturnNoOfVowelsAndConsonants()
        {
            Assert.Equal((3, 5), new CountLetters("fhAg ;yyou").Count());
        }

        [Fact]
        public void GetSingleCharacterIsCalledShouldReturnFirstCharacterThatDoesntRepeat()
        {
            Assert.Equal('c', new CountLetters("aibcbaii").GetSingleCharacter());
        }

        [Fact]
        public void ConvertIsCalledShouldConvertStringToNumber()
        {
            Assert.Equal(123, new CountLetters("123").Convert());
        }

        [Fact]
        public void ConvertIsCalledPhraseContainsNonDigitsShouldThrowException()
        {
            Assert.Throws<ArgumentException>(() => new CountLetters("12g").Convert());
        }
    }
}
