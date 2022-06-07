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
            Range oneNine = new ('1', '9');
            Choice digits = new (new Character('0'), oneNine);
            Any parts = new Any(".eE");
            Any signs = new Any("+-");
        }

        public IMatch Match(string text)
        {
            return pattern.Match(text);
        }
    }
}
