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
            this.pattern = new Optional(new Sequence(new OneOrMore(new Sequence(element, separator)), new Optional(element)));
        }

        public IMatch Match(string text)
        {
            return pattern.Match(text);
        }
    }
}
