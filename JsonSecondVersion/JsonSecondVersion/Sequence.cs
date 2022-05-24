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
            string temp = text;
            string textCopy = text;
            foreach (var pattern in patterns)
            {
                text = pattern.Match(text).RemainingText();
                if (text == temp)
                {
                    return new FailedMatch(textCopy);
                }

                temp = text;
            }

            return new SuccessMatch(text);
        }
    }
}
