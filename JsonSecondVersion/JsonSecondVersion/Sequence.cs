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
            foreach (var pattern in patterns)
            {
                var match = pattern.Match(failedText);
                if (pattern.Match(text).Success())
                {
                    text = pattern.Match(text).RemainingText();
                }
                else
                {
                    return new FailedMatch(failedText);
                }
            }

            return new SuccessMatch(text);
        }
    }
}
