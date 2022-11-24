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
            (int vowels, int consonants) result = (0, 0);
            result = phrase.ToLower().Where(character => char.IsLetter(character))
                .Aggregate(result, (_, character) => "euioa".Any(toCompare => toCompare.Equals(character))
            ? (result.vowels++, result.consonants)
            : (result.vowels, result.consonants++));

            return result;
        }

        public char GetSingleCharacter()
        {
            return phrase.First(value => phrase.Count(toCompare => value == toCompare) == 1);
        }
    }
}
