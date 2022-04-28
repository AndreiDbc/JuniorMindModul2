using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonSecondVersion
{
    class Character : IMatch
    {
        readonly char pattern;

        public Character(char pattern)
        {
            this.pattern = pattern;
        }

        public bool Success(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return false;
            }

            return text[0] == pattern;
        }

        public string RemainingText(string text)
        {
            if (Success(text))
            {
                return text[1..];
            }

            return text;
        }
    }
}
