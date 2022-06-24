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
            var array = new Sequence(
                new Character('['),
                new List(value, new Sequence(new Character(','), new Character(' '))),
                new Character(']'));
            var jsonObject = new Sequence(
                new Character('{'),
                new List(
                    new Sequence(
                        value,
                        new Character(':'),
                        value),
                    new Sequence(new Character(','), new Character(' '))),
                new Character('}'));
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
