using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Sdk;

namespace Linq
{
    static class NumberOperations
    {
        public static IEnumerable<IEnumerable<int>> GenerateSubarrays(this int[] numbers, int sum)
        {
            return Enumerable.Range(1, numbers.Length)
                .SelectMany(x => Enumerable.Range(0, numbers.Length - x + 1)
                .Select(y => numbers.Skip(y).Take(x)))
                .Where(z => z.Sum() <= sum);
        }
    }
}
