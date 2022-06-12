using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonSecondVersion
{
    public class JSONString : IPattern
    {
        private readonly IPattern pattern;

        public JSONString()
        {
            this.pattern = pattern;
        }

        public IMatch Match(string text)
        {
            return pattern.Match(text);
        }
    }
}
