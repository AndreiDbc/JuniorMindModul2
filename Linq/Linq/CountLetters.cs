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
            result = phrase.ToLower().Where(char.IsLetter)
                .Aggregate(result, (_, character) => "euioa".Contains(character)
            ? (++result.vowels, result.consonants)
            : (result.vowels, ++result.consonants));

            return result;
        }

        public char GetSingleCharacter()
        {
            return phrase.First(value => phrase.Count(toCompare => value == toCompare) == 1);
        }

        public int Convert()
        {
            if (!phrase.All(char.IsDigit))
            {
                throw new ArgumentException("String may only contain digits.");
            }

            int result = 0;
            return phrase.Aggregate(result, (_, character) => result = result * 10 + character - '0');
        }
    }
}
