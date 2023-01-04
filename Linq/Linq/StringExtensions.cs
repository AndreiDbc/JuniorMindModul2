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
            var dictionary = new Dictionary<string, WordCount>();
            var words = text.Split(" .?!".ToCharArray());
            return words.Aggregate(dictionary, (x, y) =>
            {
                if (x.ContainsKey(y))
                {
                    x[y].Count++;
                }
                else
                {
                    x.Add(y, new WordCount() { Count = 1, Word = y.ToLower() });
                }

                return x;
            }).Values.OrderByDescending(x => x.Count);
        }
    }
}
