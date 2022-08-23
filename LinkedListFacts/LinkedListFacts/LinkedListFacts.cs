using Xunit;

namespace LinkedListFacts
{
    public class LinkedListFacts
    {
        [Fact]
        public void AnInstanceofLinkedListisCreatedAddIsCalledShouldReturnNewElement()
        {
            var array = new LinkedList<int>();
            array.Add(23);
            array.Add(12);
            array.Add(1232);
            Assert.Equal(23, array[0]);
            Assert.Equal(12, array[1]);
        }

        [Fact]
        public void CopyToIsCalledShouldReturnUpdatedArray()
        {
            string[] array = new[] { "f", "gs", "ss", "Gf" };
            var list = new LinkedList<string>();
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
        public void IndexOfIsCalledShouldReturnTheIndexOfAnObject()
        {
            const double obj = 1232.213;
            var array = new LinkedList<double>();
            array.Add(23);
            array.Add(12);
            array.Add(1232.213);
            Assert.Equal(2, array.IndexOf(obj));
        }

        [Fact]
        public void IndexOfIsCalledShouldReturnMinusOne()
        {
            var array = new LinkedList<bool>();
            const bool obj = false;
            array.Add(true);
            array.Add(true);
            array.Add(true);
            Assert.Equal(-1, array.IndexOf(obj));
        }

        [Fact]
        public void ContainsIsCalledShouldReturnTrue()
        {
            var array = new LinkedList<string>();
            const string obj = "vg";
            array.Add("asd");
            array.Add("vg");
            array.Add("SDA");
            Assert.True(array.Contains(obj));
        }

        [Fact]
        public void ContainsIsCalledShouldReturnFalse()
        {
            var array = new LinkedList<bool>();
            const bool obj = false;
            array.Add(true);
            array.Add(true);
            array.Add(true);
            Assert.False(array.Contains(obj));
        }

        [Fact]
        public void InsertIsCalledShouldReturnOneMoreElementToArray()
        {
            var array = new LinkedList<char>();
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
            var array = new LinkedList<int>();
            array.Add(67);
            array.Add(45);
            array.Add(45);
            array.Clear();
            Assert.Equal(0, array.Count);
        }

        [Fact]
        public void RemoveIsCalledShouldReturnArrayWithRemovedObject()
        {
            var array = new LinkedList<string>();
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
            var array = new LinkedList<string>();
            array.Add("asd");
            array.Add("gfs");
            array.Add("AS");
            Assert.True(array.Remove("gfs"));
        }

        [Fact]
        public void RemoveIsCalledShouldReturnFalse()
        {
            var array = new LinkedList<string>();
            array.Add("asd");
            array.Add("gfs");
            array.Add("AS");
            Assert.False(array.Remove("s"));
        }

        [Fact]
        public void RemoveAtIsCalledShouldReturnArrayWithRemovedObject()
        {
            var array = new LinkedList<string>();
            array.Add("asd");
            array.Add("gfs");
            array.Add("AS");
            array.RemoveAt(1);
            Assert.Equal("asd", array[0]);
            Assert.Equal("AS", array[2]);
        }
    }
}