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
        public static IEnumerable<WordCount> CountWords(this string text)
        {
            var words = text.Split(" .?!".ToCharArray());
            return words.GroupBy(
                a => a,
                b => b,
                (key, value) => new WordCount() { Word = key, Count = value.Count() },
                EqualityComparer<string>.Default).OrderByDescending(x => x.Count);
        }
    }
}
