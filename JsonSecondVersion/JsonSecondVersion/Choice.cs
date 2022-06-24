using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonSecondVersion
{
    class Choice : IPattern
    {
        private IPattern[] patterns;

        public Choice(params IPattern[] patterns)
        {
            this.patterns = patterns;
        }

        public IMatch Match(string text)
        {
            foreach (var pattern in patterns)
            {
                var match = pattern.Match(text);
                if (match.Success())
                {
                    return match;
                }
            }

            return new FailedMatch(text);
        }

        public void Add(Sequence toAdd)
        {
            IPattern[] newPatterns = new IPattern[patterns.Length + 1];
            for (int i = 0; i < patterns.Length; i++)
            {
                newPatterns[i] = patterns[i];
            }

            newPatterns[^1] = toAdd;
            patterns = newPatterns;
        }
    }
}