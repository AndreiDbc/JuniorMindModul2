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
            Any signs = new ("+-");
            Choice zero = new (new Sequence(new Character('-'), new Character('0')), new Character('0'));
            Range digit = new ('0', '9');
            OneOrMore digits = new (digit);
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
