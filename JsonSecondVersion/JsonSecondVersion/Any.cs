using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonSecondVersion
{
    class Any : IPattern
    {
        readonly string accepted;
        readonly IPattern[] patterns;

        public Any(string accepted)
        {
            this.accepted = accepted;
            patterns = new Character[accepted.Length];
            for (int i = 0; i < patterns.Length; i++)
            {
                patterns[i] = new Character(accepted[i]);
            }
        }

        public IMatch Match(string text)
        {
            foreach (var pattern in patterns)
            {
                if (pattern.Match(text).Success())
                {
                    return new SuccessMatch(text[1..]);
                }
            }

            return new FailedMatch(text);
        }
    }
}