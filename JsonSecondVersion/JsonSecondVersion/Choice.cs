using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonSecondVersion
{
    interface IPattern
    {
        bool Match(string text);
    }

    class Choice : IPattern
    {
        private readonly IPattern[] patterns;

        public Choice(params IPattern[] patterns)
        {
           this.patterns = patterns;
        }

        public bool Match(string text)
        {
            foreach (var pattern in patterns)
            {
                if (!pattern.Match(text))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
