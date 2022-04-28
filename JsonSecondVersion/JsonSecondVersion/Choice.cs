using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonSecondVersion
{
    class Choice : IMatch
    {
        private readonly IMatch[] patterns;

        public Choice(params IMatch[] patterns)
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
