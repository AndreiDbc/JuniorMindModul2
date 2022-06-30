using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonSecondVersion
{
    public class MathematicalExpression : IPattern
    {
        private readonly IPattern pattern;

        public MathematicalExpression()
        {
            Any signs = new ("+-/*");
            Choice operand = new (new Number());
            Sequence expression = new (new Character('('), operand, signs, operand, new Character(')'));
            operand.Add(expression);
            this.pattern = new Sequence(operand, signs, operand);
        }

        public IMatch Match(string text)
        {
            return pattern.Match(text);
        }
    }
}