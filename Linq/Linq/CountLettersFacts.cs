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
        public void CountVocalsIsCalledShouldReturnNoOfVocals()
        {
            Assert.Equal(3, new CountLetters("fhAgou").CountVocals());
        }

        [Fact]
        public void CountConsonantsIsCalledShouldReturnNoOfConsonants()
        {
            Assert.Equal(3, new CountLetters("aeiowaAyIjO").CountConsonants());
        }

        [Fact]
        public void GetSingleCharacterIsCalledShouldReturnFirstCharacterThatDoesntRepeat()
        {
            Assert.Equal('c', new CountLetters("aibcbaii").GetSingleCharacter());
        }

        [Fact]
        public void GetSingleCharacterIsCalledAllCharactersRepeatShouldReturnNullCharacter()
        {
            Assert.Equal('\0', new CountLetters("aibcbacii").GetSingleCharacter());
        }
    }
}
