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
            var array = new Choice(
                new Sequence(
                    new Character('['),
                    ws,
                    new Character(']')),
                new Sequence(
                new Character('['),
                new List(value, new Sequence(new Character(','), new Character(' '))),
                new Character(']')));
            var jsonObject = new Choice(
                new Sequence(
                    new Character('{'),
                    ws,
                    new Character('}')),
                new Sequence(
                new Character('{'),
                new List(
                    new Sequence(
                        value,
                        new Character(':'),
                        value),
                    new Sequence(new Character(','), new Character(' '))),
                new Character('}')));
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
