using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Collections
{
    public class ListFacts
    {
        [Fact]
        public void AnInstanceofObjectArrayisCreatedAddIsCalledShouldReturnNewElement()
        {
            var array = new List<int>();
            array.Add(23);
            array.Add(12);
            array.Add(1232);
            Assert.Equal(23, array[0]);
            Assert.Equal(12, array[1]);
            Assert.Equal(1232, array[2]);
        }

        [Fact]
        public void IndexOfIsCalledShouldReturnTheIndexOfAnObject()
        {
            const double obj = 1232.213;
            var array = new List<double>();
            array.Add(23);
            array.Add(12);
            array.Add(1232.213);
            Assert.Equal(2, array.IndexOf(obj));
        }

        [Fact]
        public void IndexOfIsCalledShouldReturnMinusOne()
        {
            var array = new List<bool>();
            const bool obj = false;
            array.Add(true);
            array.Add(true);
            array.Add(true);
            Assert.Equal(-1, array.IndexOf(obj));
        }

        [Fact]
        public void ContainsIsCalledShouldReturnTrue()
        {
            var array = new List<string>();
            const string obj = "vg";
            array.Add("asd");
            array.Add("vg");
            array.Add("SDA");
            Assert.True(array.Contains(obj));
        }

        [Fact]
        public void ContainsIsCalledShouldReturnFalse()
        {
            var array = new List<bool>();
            const bool obj = false;
            array.Add(true);
            array.Add(true);
            array.Add(true);
            Assert.False(array.Contains(obj));
        }

        [Fact]
        public void InsertIsCalledShouldReturnOneMoreElementToArray()
        {
            var array = new List<char>();
            const char obj = 'c';
            array.Add('1');
            array.Add('2');
            array.Add('3');
            array.Insert(1, obj);
            Assert.Equal(obj, array[1]);
        }

        [Fact]
        public void ClearIsCalledShouldSetCountToZero()
        {
            var array = new List<int>();
            array.Add(67);
            array.Add(45);
            array.Add(45);
            array.Clear();
            Assert.Equal(0, array.Count);
        }

        [Fact]
        public void RemoveIsCalledShouldReturnArrayWithRemovedObject()
        {
            var array = new List<string>();
            array.Add("asd");
            array.Add("gfs");
            array.Add("AS");
            array.Remove("gfs");
            Assert.Equal("asd", array[0]);
            Assert.Equal("AS", array[2]);
        }

        [Fact]
        public void RemoveIsCalledShouldReturnTrue()
        {
            var array = new List<string>();
            array.Add("asd");
            array.Add("gfs");
            array.Add("AS");
            Assert.True(array.Remove("gfs"));
        }

        [Fact]
        public void RemoveIsCalledShouldReturnFalse()
        {
            var array = new List<string>();
            array.Add("asd");
            array.Add("gfs");
            array.Add("AS");
            Assert.False(array.Remove("s"));
        }

        [Fact]
        public void RemoveAtIsCalledShouldReturnArrayWithRemovedObject()
        {
            var array = new List<string>();
            array.Add("asd");
            array.Add("gfs");
            array.Add("AS");
            array.RemoveAt(1);
            Assert.Equal("asd", array[0]);
            Assert.Equal("AS", array[2]);
        }

        [Fact]
        public void GetEnumeratorMoveNextIsCalledShouldReturnTrue()
        {
            var array = new List<string>();
            array.Add("asd");
            array.Add("gfs");
            array.Add("AS");
            Assert.True(array.GetEnumerator().MoveNext());
        }

        [Fact]
        public void GetEnumeratorMoveNextIsCalledShouldReturnNextElement()
        {
            var array = new List<string>();
            array.Add("asd");
            array.Add("gfs");
            array.Add("AS");
            IEnumerator<string> test = array.GetEnumerator();
            test.MoveNext();
            test.MoveNext();
            Assert.Equal(array[1], test.Current);
        }

        [Fact]
        public void CopyToIsCalledShouldReturnUpdatedArray()
        {
            string[] array = new[] { "f", "gs", "ss", "Gf" };
            var list = new List<string>();
            list.Add("asd");
            list.Add("gfs");
            list.Add("AS");
            list.CopyTo(array, 1);
            Assert.Equal("f", array[0]);
            Assert.Equal("asd", array[1]);
            Assert.Equal("gfs", array[2]);
            Assert.Equal("AS", array[3]);
        }

        [Fact]
        public void IsReadOnlyIsCalledShouldReturnFalse()
        {
            var list = new List<string>();
            list.Add("asd");
            list.Add("gfs");
            list.Add("AS");
            Assert.False(list.IsReadOnly);
        }

        [Fact]
        public void CopyToIsCalledShouldThrowIndexOutOfRangeException()
        {
            var list = new List<string>();
            list.Add("asd");
            list.Add("gfs");
            list.Add("AS");
            string[] test = new string[4];
            Assert.Throws<ArgumentException>(() => list.CopyTo(test, -1));
        }

        [Fact]
        public void CopyToIsCalledShouldThrowArgumentNullException()
        {
            var list = new List<string>();
            list.Add("asd");
            list.Add("gfs");
            list.Add("AS");
            Assert.Throws<ArgumentNullException>(() => list.CopyTo(null, 1));
        }

        [Fact]
        public void CopyToIsCalledShouldThrowArgumentException()
        {
            var list = new List<string>();
            list.Add("asd");
            list.Add("gfs");
            list.Add("AS");
            string[] test = new string[2];
            Assert.Throws<ArgumentException>(() => list.CopyTo(test, 0));
        }

        [Fact]
        public void InsertIsCalledShouldThrowArgumentOutOfRangeException()
        {
            var list = new List<string>();
            list.Add("asd");
            list.Add("gfs");
            list.Add("AS");
            Assert.Throws<IndexOutOfRangeException>(() => list.Insert(-4, "kfff"));
        }

        [Fact]
        public void RemoveAtIsCalledShouldThrowArgumentOutOfRangeException()
        {
            var list = new List<string>();
            list.Add("asd");
            list.Add("gfs");
            list.Add("AS");
            Assert.Throws<IndexOutOfRangeException>(() => list.RemoveAt(-4));
        }

        [Fact]
        public void ValueIsSetUsingIndeexerShouldThrowArgumentOutOfRangeException()
        {
            var list = new List<string>();
            list.Add("asd");
            list.Add("gfs");
            list.Add("AS");
            Assert.Throws<ArgumentException>(() => list[-3] = "gh");
        }
    }
}
