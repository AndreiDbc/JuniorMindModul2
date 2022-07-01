using Xunit;

namespace Collections
{
    public class IntArrayFacts
    {
        [Fact]
        public void AddIsCalledShouldReturnArraywithOneMoreElement()
        {
            IntArray array = new ();
            array.Add(2);
            Assert.Equal(1, array.Count());
            Assert.Equal(2, array.Element(0));
        }

        [Fact]
        public void SetElementIsCalledShouldReturnArrayWithChangedElement()
        {
            IntArray array = new ();
            array.Add(1);
            array.Add(2);
            array.Add(3);
            array.Add(4);
            array.SetElement(3, 24);
            Assert.Equal(24, array.Element(3));
        }

        [Fact]
        public void ContainsIsCalledArrayDoesntContainNumberShouldReturnFalse()
        {
            IntArray array = new ();
            array.Add(1);
            array.Add(2);
            array.Add(3);
            array.Add(4);
            Assert.False(array.Contains(5));
        }

        [Fact]
        public void ContainsIsCalledArrayContainsNumberShouldReturnTrue()
        {
            IntArray array = new ();
            array.Add(1);
            array.Add(2);
            array.Add(3);
            array.Add(4);
            Assert.True(array.Contains(3));
        }

        [Fact]
        public void IndexOfIsCalledArrayContainsTheElementShouldReturnPosition()
        {
            IntArray array = new ();
            array.Add(1);
            array.Add(2);
            array.Add(3);
            array.Add(4);
            Assert.Equal(2, array.IndexOf(3));
        }

        [Fact]
        public void IndexOfIsCalledArrayDoesntContainTheElementShouldReturnMinusOne()
        {
            IntArray array = new ();
            array.Add(1);
            array.Add(2);
            array.Add(3);
            array.Add(4);
            Assert.Equal(-1, array.IndexOf(67));
        }

        [Fact]
        public void InsertIsCalledShuldReturnNewElementToPosition()
        {
            IntArray array = new ();
            array.Add(1);
            array.Add(2);
            array.Add(3);
            array.Add(4);
            array.Insert(2, 55);
            Assert.Equal(1, array.Element(0));
            Assert.Equal(2, array.Element(1));
            Assert.Equal(55, array.Element(2));
            Assert.Equal(3, array.Element(3));
            Assert.Equal(4, array.Element(4));
        }

        [Fact]
        public void ClearIsCalledShouldReturnEmptyArray()
        {
            IntArray array = new ();
            array.Add(1);
            array.Add(2);
            array.Add(3);
            array.Add(4);
            array.Clear();
            Assert.Equal('\0', array.Element(0));
            Assert.Equal('\0', array.Element(0));
            Assert.Equal('\0', array.Element(0));
            Assert.Equal('\0', array.Element(0));
        }

        [Fact]
        public void RemoveIsCalledShouldReturnArrayWithRemovedElement()
        {
            IntArray array = new ();
            array.Add(1);
            array.Add(2);
            array.Add(3);
            array.Add(4);
            array.Remove(2);
            Assert.Equal(1, array.Element(0));
            Assert.Equal(3, array.Element(1));
            Assert.Equal(4, array.Element(2));
        }
    }
}