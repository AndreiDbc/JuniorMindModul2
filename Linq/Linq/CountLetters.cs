using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

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
                .Aggregate((vowels: 0, consonants: 0), (touple, character) => "euioa".Contains(character)
            ? (++touple.vowels, touple.consonants)
            : (touple.vowels, ++touple.consonants));
        }

        public char GetSingleCharacter()
        {
            return phrase.First(value => phrase.Count(toCompare => value == toCompare) == 1);
        }

        public int Convert()
        {
            bool isNegative = phrase.StartsWith("-");
            Func<string, bool> hasOnlyDigits = input => input.All(char.IsDigit);
            if (isNegative ? !hasOnlyDigits(phrase[1..]) : !hasOnlyDigits(phrase))
            {
                throw new ArgumentException("String may only contain digits.");
            }

            Func<int, char, int> func = (number, character) => (number * 10) + character - '0';

            return isNegative
                ? phrase[1..].Aggregate(0, func) * -1
                : phrase.Aggregate(0, func);
        }

        public int GetMostApparitionsOfCharacter()
        {
            return phrase.Select(character => phrase.Count(toCompare => toCompare == character)).Max();
        }

        public IEnumerable<string> GetPalindromes()
        {
            var phraseSubstrings =
                from i in Enumerable.Range(0, phrase.Length)
                from j in Enumerable.Range(0, phrase.Length - i + 1)
                where j >= 1
                select phrase.Substring(i, j);

            return phraseSubstrings.Where(x => x == new string(x.ToCharArray().Reverse().ToArray()));
        }
    }
}