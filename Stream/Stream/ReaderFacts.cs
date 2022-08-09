using Xunit;

namespace Stream
{
    public class ReaderFacts
    {
        [Fact]
        public void ReadTextIscalledShouldReturnTextFromAFile()
        {
            const string path = @"C:\Users\Andrei\Desktop\Example1.txt";
            Reader reader = new Reader(path);
            Assert.Equal("This is an example.", reader.ReadText());
        }
    }
}