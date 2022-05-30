using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonSecondVersion
{
    class Many : IPattern
    {
        private readonly IPattern pattern;

        public Many(IPattern pattern)
        {
            this.pattern = pattern;
        }

        public IMatch Match(string text)
        {
            IMatch match = new SuccessMatch(text);
            foreach (char c in text)
            {
                var toMatch = pattern.Match(c.ToString());
                match = pattern.Match(match.RemainingText());
                if (!toMatch.Success())
                {
                    return new SuccessMatch(match.RemainingText());
                }
            }

            return new SuccessMatch(text);
        }
    }
}
