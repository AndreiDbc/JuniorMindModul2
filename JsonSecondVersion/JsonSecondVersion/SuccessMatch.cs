using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonSecondVersion
{
    public class SuccessMatch : IMatch
    {
        private readonly string text;

        public SuccessMatch(string text)
        {
            this.text = text;
        }

        public bool Success(string text)
        {
            return this.text != text;
        }

        public string RemainingText(string text)
        {
            return text[1..];
        }
    }
}
