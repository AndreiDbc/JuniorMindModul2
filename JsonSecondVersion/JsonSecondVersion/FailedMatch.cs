using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonSecondVersion
{
    public class FailedMatch : IMatch
    {
        private readonly string text;

        public FailedMatch(string text)
        {
            this.text = text;
        }

        public bool Success()
        {
            return false;
        }

        public string RemainingText()
        {
            return text;
        }
    }
}
