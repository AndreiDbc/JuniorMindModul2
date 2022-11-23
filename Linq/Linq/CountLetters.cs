using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linq
{
    class CountLetters
    {
        private readonly string phrase;

        public CountLetters(string phrase)
        {
            this.phrase = phrase;
        }

        public (int vowels, int consonants) Count()
        {
            return (phrase.Count(letter => "EeUuIiOoAa".Any(toCompare => toCompare.Equals(letter))),
            phrase.Count(letter => "QqWwRrTtYyPpSsDdFfGgHhJjKkLlZzXxCcVvBbNnMm".Any(toCompare => toCompare.Equals(letter))));
        }

        public char GetSingleCharacter()
        {
            return phrase.First(value => phrase.Count(toCompare => value == toCompare) == 1);
        }
    }
}
