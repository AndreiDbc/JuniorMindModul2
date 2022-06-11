using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonSecondVersion
{
    public class Number : IPattern
    {
        private readonly IPattern pattern;

        public Number()
        {
            Any minus = new ("-");
            Any dot = new (".");
            Any exponent = new ("eE");
            Any signs = new ("+-");
            Many positiveNumber = new (new Range('1', '0'));
            Sequence negativeNumber = new (minus, positiveNumber);
            Sequence fractionalPart = new (dot, positiveNumber);
            Choice exponentPart = new (new Sequence(exponent, positiveNumber, new Sequence(exponent, signs, positiveNumber)));
            Sequence hasFractionAndExponent = new (new Choice(positiveNumber, negativeNumber), fractionalPart, exponentPart);
            Sequence hasFractionOrExponent = new (new Choice(positiveNumber, negativeNumber), new Choice(fractionalPart, exponentPart));
            this.pattern = new Choice(hasFractionAndExponent, hasFractionOrExponent, new Choice(positiveNumber, negativeNumber));
        }

        public IMatch Match(string text)
        {
            return pattern.Match(text);
        }
    }
}
