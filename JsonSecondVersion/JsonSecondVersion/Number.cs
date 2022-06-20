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
            Choice signs = new (new Any("+-"));
            Range oneNine = new ('1', '9');
            Choice zero = new (new Sequence(new Character('-'), new Character('0')), new Character('0'));
            Choice digit = new (new Character('0'), oneNine);
            Sequence digits = new (new Choice(new OneOrMore(digit), digit));
            Choice integer = new (new Sequence(new Character('-'), digits), digits);
            Sequence exponent = new (new Any("Ee"), new Choice(new Sequence(signs, digits), digits));
            Sequence fraction = new (integer, new Character('.'), digits);
            this.pattern = new Choice(new Sequence(fraction, exponent), new Sequence(integer, exponent), fraction, new Choice(integer, zero));
        }

        public IMatch Match(string text)
        {
            return pattern.Match(text);
        }
    }
}
