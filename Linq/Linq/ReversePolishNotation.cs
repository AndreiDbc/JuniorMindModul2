using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linq
{
    class ReversePolishNotation
    {
        private readonly string expression;

        public ReversePolishNotation(string expression)
        {
            this.expression = expression;
        }

        public int GetResult()
        {
            IEnumerable<int> numbers = new int[] { };
            return expression.Split(" ").Aggregate(numbers, (seed, element) =>
                seed.Append(int.TryParse(element, out var number) ? number : Calculate(seed.TakeLast(2), element))).Last();
        }

        private static int Calculate(IEnumerable<int> numbers, string element)
        {
            var first = numbers.ElementAt(0);
            var second = numbers.ElementAt(1);
            return element switch
            {
                "*" => first * second,
                "/" => first / second,
                "-" => first - second,
                "+" => first + second
            };
        }
    }
}