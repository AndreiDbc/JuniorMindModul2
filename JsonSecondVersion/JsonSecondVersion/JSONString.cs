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
            Choice hexDigit = new (new Range('0', '9'), new Range('a', 'f'), new Range('A', 'F'));
            Sequence recognisedEscapeSequence = new (backslash, new Choice(new Any("\"btrfn/\\"), new Sequence(new Character('u'), hexDigit, hexDigit, hexDigit, hexDigit)));
            Choice totalCharacters = new (new Character('\u0020'), new Character('\u0021'), new Range('\u0023', '\u005b'), new Range('\u005d', '\uffff'), recognisedEscapeSequence);
            this.pattern = new Sequence(quote, new Many(totalCharacters), quote);
        }

        public IMatch Match(string text)
        {
            return pattern.Match(text);
        }
    }
}
