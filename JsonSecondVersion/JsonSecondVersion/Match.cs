using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonSecondVersion
{
    public interface IMatch
    {
        bool Success();

        string RemainingText();
    }

    class Match : IMatch
    {
        private readonly string text;

        public Match(string text)
        {
            this.text = text;
        }

        public bool Success()
        {
            if (string.IsNullOrEmpty(text))
            {
                return false;
            }

            return text.Length == 1;
        }

        public string RemainingText()
        {
            return text[1..];
        }
    }
}
