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
            while (match.Success())
            {
                match = pattern.Match(match.RemainingText());
            }

            return new SuccessMatch(match.RemainingText());
        }
    }
}
