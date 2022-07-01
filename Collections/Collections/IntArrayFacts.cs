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
    }
}