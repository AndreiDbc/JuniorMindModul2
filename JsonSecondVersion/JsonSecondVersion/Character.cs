using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonSecondVersion
{
    class Character : IPattern
    {
        readonly char pattern;

        public Character(char pattern)
        {
            this.pattern = pattern;
        }

        public IMatch Match(string text)
        {
            return !string.IsNullOrEmpty(text) && text[0] == pattern
                ? new SuccessMatch(text[1..])
                : new FailedMatch(text);
        }
    }
}