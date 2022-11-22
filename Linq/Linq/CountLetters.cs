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

        public int CountVocals()
        {
            const string vocals = "EeUuIiOoAa";
            return phrase.Select(vocal => vocal).Count(letter => vocals.Any(toCompare => toCompare.Equals(letter)));
        }
    }
}
