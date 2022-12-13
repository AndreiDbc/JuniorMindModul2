using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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

        public static IEnumerable<string> GenerateSum(this int n, int k)
        {
            var range = Enumerable.Range(1, n);
            var signs = new[] { true, false };
            var signPermutations = new[] { true, false }.Select(i => range.SelectMany(j => new bool[j - 1].Append(i)));
            return signPermutations.Where(x => range.Aggregate(0, (sum, element) => x.ElementAt(element - 1)
            ? sum + element
            : sum + element * (-1)) == k)
                .Select(x => range.Aggregate("", (expression, element) =>
                expression + (x.ElementAt(element - 1) ? "+" : "-") + element) + "=" + k);
        }

        public static IEnumerable<IEnumerable<int>> GenerateTriplets(this IEnumerable<int> numbers)
        {
            var range = Enumerable.Range(0, numbers.Count() - 1);
            return range.SelectMany(i => range.Select(j => new[] { numbers.ElementAt(i), numbers.ElementAt(j) }
            .Concat(numbers.Where(k => numbers.ElementAt(i) * numbers.ElementAt(i) + numbers.ElementAt(j) * numbers.ElementAt(j) == k * k))))
            .OrderBy(x => x.Count(), Comparer<int>.Default)
                .Where(result => result.Count() == 3);
        }
    }
}
