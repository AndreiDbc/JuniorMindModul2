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

        public Any(string accepted)
        {
            this.accepted = accepted;
        }

        public IMatch Match(string text)
        {
            foreach (char c in accepted)
            {
                if (!string.IsNullOrEmpty(text) && c == text[0])
                {
                    return new SuccessMatch(text[1..]);
                }
            }

            return new FailedMatch(text);
        }
    }
}
