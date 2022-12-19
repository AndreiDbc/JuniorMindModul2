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
            return PermutateSigns(n).Where(x => range.Aggregate(0, (sum, element) => sum + element * x.ElementAt(element - 1)) == k)
                .Select(x => range.Aggregate("", (expression, element) =>
                expression + (x.ElementAt(element - 1) == 1 ? "+" : "-") + element) + "=" + k);
        }

        public static IEnumerable<IEnumerable<int>> GenerateTriplets(this IEnumerable<int> numbers)
        {
            return Enumerable.Range(0, numbers.Count() - 1).SelectMany(i => Enumerable.Range(i + 1, numbers.Count() - i - 1)
                     .SelectMany(j => Enumerable.Range(j + 1, numbers.Count() - j - 1)
                        .Select(k => new[] { numbers.ElementAt(i), numbers.ElementAt(j), numbers.ElementAt(k) }))).Where(ArePythagorean);
        }

        private static bool ArePythagorean(IEnumerable<int> values)
        {
            return (values.ElementAt(0) * values.ElementAt(0) + values.ElementAt(1) * values.ElementAt(1) == values.ElementAt(2) * values.ElementAt(2)) ||
                (values.ElementAt(1) * values.ElementAt(1) + values.ElementAt(2) * values.ElementAt(2) == values.ElementAt(0) * values.ElementAt(0)) ||
                (values.ElementAt(2) * values.ElementAt(2) + values.ElementAt(0) * values.ElementAt(0) == values.ElementAt(1) * values.ElementAt(1));
        }

        private static IEnumerable<IEnumerable<int>> PermutateSigns(int length)
        {
            IEnumerable<IEnumerable<int>> seed = new[] { new int[0] };
            return Enumerable.Range(1, Convert.ToInt32(Math.Pow(2, length)))
                .Aggregate(seed, (result, count) => result.Concat(new[] { result.ElementAt(count - 1).Concat(new[] { -1 }), result.ElementAt(count - 1).Concat(new[] { 1 }) }))
                .Where(values => values.Count() == length);
        }
    }
}
