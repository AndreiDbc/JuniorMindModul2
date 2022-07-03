using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Collections
{
    public class ObjectArrayFacts
    {
        [Fact]
        public void AnInstanceofObjectArrayisCreatedAddIsCalledShouldReturnNewElement()
        {
            var array = new ObjectArray();
            array.Add("asd");
            array.Add(true);
            array.Add(1232.213);
            Assert.Equal("asd", array[0]);
            Assert.True((bool)array[1]);
            Assert.Equal(1232.213, array[2]);
        }
    }
}
