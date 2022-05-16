using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonSecondVersion
{
    interface IPattern
    {
        IMatch Match(string text);
    }

    class Choice : IPattern
    {
        private readonly IPattern[] patterns;

        public Choice(params IPattern[] patterns)
        {
            this.patterns = patterns;
        }

        public bool Success(string text)
        {
            foreach (var pattern in patterns)
            {
                if (!pattern.Success(text))
                {
                    return false;
                }
            }

            return true;
        }

        public string RemainingText(string text)
        {
            if (Success(text))
            {
                return RemainingText(text[1..]);
            }

            return text;
        }
    }
}
