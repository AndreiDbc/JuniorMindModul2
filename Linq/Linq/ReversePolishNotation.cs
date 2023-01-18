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

        public int Calculate()
        {
            var elements = expression.Split(" ");
            var stack = new Stack<int>();
            return elements.Aggregate(0, (_, element) =>
            {
                int calculus = 0;
                if (int.TryParse(element, out var number))
                {
                    stack.Push(number);
                }
                else
                {
                    int temp = stack.Pop();

                    switch (element)
                    {
                        case "*":

                            calculus = stack.Pop() * temp;
                            stack.Push(calculus);
                            break;
                        case "/":
                            calculus = stack.Pop() / temp;
                            stack.Push(calculus);
                            break;
                        case "+":
                            calculus = stack.Pop() + temp;
                            stack.Push(calculus);
                            break;
                        case "-":
                            calculus = stack.Pop() - temp;
                            stack.Push(calculus);
                            break;
                    }
                }

                return calculus;
            });
        }
    }
}