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
            Choice signs = new (new Character('\0'), new Any("+-"));
            Range oneNine = new ('1', '9');
            Choice digit = new (new Character('0'), oneNine);
            Choice digits = new (digit, new Many(digit));
            Choice integer = new (digit, oneNine, new Sequence(new Character('-'), digit), new Sequence(new Character('-'), oneNine));
            Choice exponent = new (new Character('\0'), new Sequence(new Any("e"), signs, digits), new Sequence(new Any("E"), signs, digits));
            Choice fraction = new (new Character('\0'), new Sequence(new Any("."), digits));
            this.pattern = new Sequence(integer, fraction, exponent);
        }

        public IMatch Match(string text)
        {
            return pattern.Match(text);
        }
    }
}
