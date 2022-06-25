using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonSecondVersion
{
    public class Value : IPattern
    {
        private readonly IPattern pattern;

        public Value()
        {
            var value = new Choice(
            new JSONString(),
            new Number(),
            new Text("true"),
            new Text("false"),
            new Text("null"));
            var ws = new Many(new Any(" \0\n\r\t"));
            var element = new Sequence(ws, value, ws);
            var elements = new Choice(new List(element, new Character(',')), element);
            var member = new Sequence(ws, new JSONString(), ws, new Character(':'), element);
            var members = new Choice(new List(member, new Character(',')), member);
            var array = new Choice(
                new Sequence(new Character('['), ws, new Character(']')),
                new Sequence(new Character('['), elements, new Character(']')));
            var jsonObject = new Choice(
                new Sequence(new Character('{'), ws, new Character('}')),
                new Sequence(new Character('{'), members, new Character('}')));
            value.Add(array);
            value.Add(jsonObject);
            this.pattern = value;
        }

        public IMatch Match(string text)
        {
            return pattern.Match(text);
        }
    }
}
