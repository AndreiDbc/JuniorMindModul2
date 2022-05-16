using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonSecondVersion
{
    interface IMatch
    {
        bool Success(string text);

        string RemainingText(string text);
    }

    class Match : IMatch
    {
        private readonly string text;
        private readonly IMatch pattern;

        public Match(string text, IMatch pattern)
        {
            this.text = text;
            this.pattern = pattern;
        }

        public bool Success(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return false;
            }

            return text.Length == 1;
        }

        public string RemainingText(string text)
        {
            return text[1..];
        }
    }
}
