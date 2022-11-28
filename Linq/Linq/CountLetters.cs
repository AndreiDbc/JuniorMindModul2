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
            return phrase.ToLower().Where(char.IsLetter)
                .Aggregate((0, 0), (touple, character) => "euioa".Contains(character)
            ? (++touple.Item1, touple.Item2)
            : (touple.Item1, ++touple.Item2));
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

            Func<int, char, int> func = (number, character) => (number * 10) + character - '0';

            return phrase.StartsWith("-")
                ? phrase[1..].Aggregate(0, func) * -1
                : phrase.Aggregate(0, func);
        }

        public int GetMostApparitionsOfCharacter()
        {
            return phrase.Select(character => phrase.Intersect(new List<char> { character }, EqualityComparer<char>.Default).Count(toCompare => toCompare == character)).Max();
        }
    }
}
