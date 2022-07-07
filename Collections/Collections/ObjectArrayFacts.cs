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

        [Fact]
        public void IndexOfIsCalledShouldReturnTheIndexOfAnObject()
        {
            var array = new ObjectArray();
            object obj = 1232.213;
            array.Add("asd");
            array.Add(true);
            array.Add(1232.213);
            Assert.Equal(2, array.IndexOf(obj));
        }

        [Fact]
        public void IndexOfIsCalledShouldReturnMinusOne()
        {
            var array = new ObjectArray();
            object obj = false;
            array.Add("asd");
            array.Add(true);
            array.Add(1232.213);
            Assert.Equal(-1, array.IndexOf(obj));
        }

        [Fact]
        public void ContainsIsCalledShouldReturnTrue()
        {
            var array = new ObjectArray();
            object obj = true;
            array.Add("asd");
            array.Add(true);
            array.Add(1232.213);
            Assert.True(array.Contains(obj));
        }

        [Fact]
        public void ContainsIsCalledShouldReturnFalse()
        {
            var array = new ObjectArray();
            object obj = false;
            array.Add("asd");
            array.Add(true);
            array.Add(1232.213);
            Assert.False(array.Contains(obj));
        }

        [Fact]
        public void InsertIsCalledShouldReturnOneMoreElementToArray()
        {
            var array = new ObjectArray();
            object obj = false;
            array.Add("asd");
            array.Add(true);
            array.Add(1232.213);
            array.Insert(1, obj);
            Assert.Equal(obj, array[1]);
        }

        [Fact]
        public void ClearIsCalledShouldSetCountToZero()
        {
            var array = new ObjectArray();
            array.Add("asd");
            array.Add(true);
            array.Add(1232.213);
            array.Clear();
            Assert.Equal(0, array.Count);
        }

        [Fact]
        public void RemoveIsCalledShouldReturnArrayWithRemovedObject()
        {
            var array = new ObjectArray();
            array.Add("asd");
            array.Add(true);
            array.Add(1232.213);
            array.Remove(true);
            Assert.Equal("asd", array[0]);
            Assert.Equal(1232.213, array[2]);
        }

        [Fact]
        public void RemoveAtIsCalledShouldReturnArrayWithRemovedObject()
        {
            var array = new ObjectArray();
            array.Add("asd");
            array.Add(true);
            array.Add(1232.213);
            array.RemoveAt(1);
            Assert.Equal("asd", array[0]);
            Assert.Equal(1232.213, array[2]);
        }
    }
}
