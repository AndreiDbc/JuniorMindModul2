using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace JsonSecondVersion
{
    public class ValueFacts
    {
        [Fact]
        public void ValueIsGivenAnArrayShouldReturnEmptyString()
        {
            var value = new Value();
            Assert.Equal("", value.Match("[\"India\", \"Romania\", \"SUA\"]").RemainingText());
        }

        [Fact]
        public void ValueIsGivenAnObjectShouldReturnEmptyString()
        {
            var value = new Value();
            Assert.Equal("", value.Match("{\"name\":\"John\", \"age\":30, \"car\":[\"BMW\", \"VW\", \"Mercedes\"]}").RemainingText());
        }
    }
}
