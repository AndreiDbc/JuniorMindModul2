using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonSecondVersion
{
    class List : IPattern
    {
        private readonly IPattern pattern;

        public List(IPattern element, IPattern separator)
        {
            this.pattern = new Optional(new OneOrMore(new Choice(new Sequence(element, separator), new Sequence(element))));
        }

        public IMatch Match(string text)
        {
            return pattern.Match(text);
        }
    }
}
