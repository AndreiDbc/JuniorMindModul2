using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Linq
{
    static class StringExtensions
    {
        public static IEnumerable<WordCount> CountWords(this string text, int noOfWords = 0)
        {
            var words = text.Split(" .?!".ToCharArray());
            var topWords = words.GroupBy(
                a => a,
                (key, value) => new WordCount() { Word = key, Count = value.Count() },
                EqualityComparer<string>.Default).OrderByDescending(x => x.Count);
            return noOfWords == 0
                ? topWords
                : topWords.Take(noOfWords);
        }
    }
}
