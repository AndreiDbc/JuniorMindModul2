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
            if (!phrase.All(char.IsDigit) && !phrase.StartsWith("-"))
            {
                throw new ArgumentException("String may only contain digits.");
            }

            return phrase.StartsWith("-")
                ? phrase[1..].Aggregate(0, (number, character) => number * 10 + character - '0') * -1
                : phrase.Aggregate(0, (number, character) => number * 10 + character - '0');
        }

        public int GetMostApparitionsOfCharacter()
        {
            return phrase.Select(character => phrase.Intersect(new List<char> { character }, EqualityComparer<char>.Default).Count(toCompare => toCompare == character)).Max();
        }
    }
}
