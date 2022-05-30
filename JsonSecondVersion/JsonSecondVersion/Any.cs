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
                Character character = new (c);
                if (!string.IsNullOrEmpty(text) && character.Match(text).Success())
                {
                    return new SuccessMatch(text[1..]);
                }
            }

            return new FailedMatch(text);
        }
    }
}
