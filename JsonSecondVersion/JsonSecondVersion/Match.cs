using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonSecondVersion
{
    interface IMatch
    {
        bool Success();

        string RemainingText();
    }

    public class Match
    {
        private readonly string text;

        public Match(string text)
        {
            this.text = text;
        }
    }
}
