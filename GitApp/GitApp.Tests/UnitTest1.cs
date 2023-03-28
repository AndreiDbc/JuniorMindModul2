namespace GitApp.Tests
{
    public class CommitTests
    {
        [Fact]
        public void Test1()
        {
            const string data = "commit 4b4d2b7149900d2175d4a2a69bc52943dad08a37\r" +
                "\nAuthor: Andrei Daboc <99188105+AndreiDbc@users.noreply.github.com>\r" +
                "\nDate:   Thu Mar 2 15:00:42 2023 +0200\r\n\r\n    Added a constructor.\r\n";

            var commit = new Commit(data);
            const string expected = "4b4d2b7149900d2175d4a2a69bc52943dad08a37";
            Assert.Equal(expected, commit.Id);
        }
    }
}