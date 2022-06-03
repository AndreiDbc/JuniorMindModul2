using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonSecondVersion
{
    class OneOrMore : IPattern
    {
        private readonly IPattern pattern;

        public OneOrMore(IPattern pattern)
        {
            this.pattern = new Many(pattern);
        }

        public IMatch Match(string text)
        {
            var match = pattern.Match(text);
            return match.RemainingText() == text
                ? new FailedMatch(text)
                : match;
        }
    }
}