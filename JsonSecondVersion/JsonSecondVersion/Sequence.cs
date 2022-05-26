using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonSecondVersion
{
    class Sequence : IPattern
    {
        private readonly IPattern[] patterns;

        public Sequence(params IPattern[] patterns)
        {
            this.patterns = patterns;
        }

        public IMatch Match(string text)
        {
            string failedText = text;
            IMatch match = new SuccessMatch(text);
            foreach (var pattern in patterns)
            {
                match = pattern.Match(text);
                if (match.Success())
                {
                    text = match.RemainingText();
                }
                else
                {
                    return new FailedMatch(failedText);
                }
            }

            return match;
        }
    }
}
