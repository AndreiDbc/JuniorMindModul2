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
            var result = new Choice();
            Any signs = new ("+-/*");
            Sequence expression = new (new Character('('), new List(result, signs), new Character(')'));
            Choice operand = new (new Number(), expression);
            result.Add(operand);
            result.Add(expression);
            this.pattern = new List(result, signs);
        }

        public IMatch Match(string text)
        {
            return pattern.Match(text);
        }
    }
}