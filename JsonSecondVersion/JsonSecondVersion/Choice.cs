using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonSecondVersion
{
    class Choice : IPattern
    {
        private readonly IPattern[] patterns;

        public Choice(params IPattern[] patterns)
        {
            this.patterns = patterns;
        }

        public IMatch Match(string text)
        {
            foreach (var pattern in patterns)
            {
                string temp = pattern.Match(text).RemainingText(text);
                if (pattern.Match(temp).Success(text[1..]))
                {
                    return new SuccessMatch(text[1..]);
                }
            }

            return new FailedMatch(text);
        }
    }
}
