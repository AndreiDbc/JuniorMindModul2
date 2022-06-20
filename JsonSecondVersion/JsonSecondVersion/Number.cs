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
            Range digit = new ('0', '9');
            OneOrMore digits = new (digit);
            Sequence integer = new (new Optional(new Character('-')), new Choice(new Character('0'), digits));
            Sequence exponent = new (new Any("Ee"), new Optional(signs), digits);
            Sequence fraction = new (new Character('.'), digits);
            this.pattern = new Sequence(integer, new Optional(fraction), new Optional(exponent));
        }

        public IMatch Match(string text)
        {
            return pattern.Match(text);
        }
    }
}
