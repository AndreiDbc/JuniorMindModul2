using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonSecondVersion
{
    class Text : IPattern
    {
        private readonly string prefix;

        public Text(string prefix)
        {
            this.prefix = prefix;
        }

        public IMatch Match(string text)
        {
            if (string.IsNullOrEmpty(text) || !text.StartsWith(prefix))
            {
                return new FailedMatch(text);
            }

            return new SuccessMatch(text[prefix.Length..]);
        }
    }
}
