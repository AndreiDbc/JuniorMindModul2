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
            Character quote = new ('\"');
            Character backslash = new ('\\');
            Any recognisedEscapeCharacters = new ("\"btrfnu/\\");
            Range small = new Range('a', 'z');
            Range capital = new Range('A', 'Z');
            this.pattern = new Sequence(quote, new Many(new Choice(new Choice(small, capital), new Sequence(backslash, recognisedEscapeCharacters))), quote);
        }

        public IMatch Match(string text)
        {
            return pattern.Match(text);
        }
    }
}
