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
    }
}