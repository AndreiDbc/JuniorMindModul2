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

    }
}