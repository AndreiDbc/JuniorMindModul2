namespace GitApp.Tests
{
    public class CommitTests
    {
        [Fact]
        public void ACommitObjectIsCreatedIdIsCalledShouldReturnId()
        {
            const string data = "commit 4b4d2b7149900d2175d4a2a69bc52943dad08a37\r" +
                "\nAuthor: Andrei Daboc <99188105+AndreiDbc@users.noreply.github.com>\r" +
                "\nDate:   Thu Mar 2 15:00:42 2023 +0200\n    Added a constructor.\r\n";

            var commit = new Commit(data);
            const string expected = "4b4d2b7";
            Assert.Equal(expected, commit.Id);
        }

        [Fact]
        public void ACommitObjectIsCreatedAuthorIsCalledShouldReturnAuthorName()
        {
            const string data = "commit 4b4d2b7149900d2175d4a2a69bc52943dad08a37\r" +
                "\nAuthor: Andrei Daboc <99188105+AndreiDbc@users.noreply.github.com>\r" +
                "\nDate:   Thu Mar 2 15:00:42 2023 +0200\n    Added a constructor.\r\n";

            var commit = new Commit(data);
            const string expected = "Andrei Daboc";
            Assert.Equal(expected, commit.Author);
        }

        [Fact]
        public void ACommitObjectIsCreatedDateIsCalledShouldReturnDate()
        {
            const string data = "commit 4b4d2b7149900d2175d4a2a69bc52943dad08a37\r" +
                "\nAuthor: Andrei Daboc <99188105+AndreiDbc@users.noreply.github.com>\r" +
                "\nDate:   Thu Mar 2 15:00:42 2023 +0200\n    Added a constructor.\r\n";

            var commit = new Commit(data);
            const string expected = "2023-03-02";
            Assert.Equal(expected, commit.Date);
        }

        [Fact]
        public void ACommitObjectIsCreatedMessageIsCalledShouldReturnMessage()
        {
            const string data = "commit 4b4d2b7149900d2175d4a2a69bc52943dad08a37\r" +
                "\nAuthor: Andrei Daboc <99188105+AndreiDbc@users.noreply.github.com>\r" +
                "\nDate:   Thu Mar 2 15:00:42 2023 +0200\n    Added a constructor.\r\n";

            var commit = new Commit(data);
            const string expected = "Added a constructor.";
            Assert.Equal(expected, commit.Message);
        }
    }
}