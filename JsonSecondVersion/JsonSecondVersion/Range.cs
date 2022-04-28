using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonSecondVersion
{
    public class Range : IMatch
    {
        private readonly char start;
        private readonly char end;

        public Range(char start, char end)
        {
            this.start = start;
            this.end = end;
        }

        public bool Success(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return false;
            }

            return text[0] >= start && text[0] <= end;
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
