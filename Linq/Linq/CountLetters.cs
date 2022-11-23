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
            var select = phrase.Select(value => value);
            return (select.Count(letter => "EeUuIiOoAa".Any(toCompare => toCompare.Equals(letter))),
            select.Count(letter => "QqWwRrTtYyPpSsDdFfGgHhJjKkLlZzXxCcVvBbNnMm".Any(toCompare => toCompare.Equals(letter))));
        }

        public char GetSingleCharacter()
        {
            foreach (char c in phrase)
            {
                if (phrase.Intersect(c.ToString(), EqualityComparer<char>.Default).Count() == 1)
                {
                    return c;
                }
            }

            return '\0';
        }
    }
}
