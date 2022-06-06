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
            this.pattern = new OneOrMore(new Choice(element, separator));
        }

        public IMatch Match(string text)
        {
            return pattern.Match(text);
        }
    }
}
