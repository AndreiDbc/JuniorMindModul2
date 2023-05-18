namespace GitApp.Tests
{
    public class CommitTests
    {
        const string Data = "commit 4b4d2b7149900d2175d4a2a69bc52943dad08a37\r" +
            "\nAuthor: Andrei Daboc <99188105+AndreiDbc@users.noreply.github.com>\r" +
            "\nDate:   Thu Mar 2 15:00:42 2023 +0200\n    Added a constructor.\r\n";
        [Fact]
        public void ACommitObjectIsCreatedIdIsCalledShouldReturnId()
        {
            var commit = new Commit(Data);
            const string expected = "4b4d2b7";
            Assert.Equal(expected, commit.Id);
        }

        [Fact]
        public void ACommitObjectIsCreatedAuthorIsCalledShouldReturnAuthorName()
        {
            var commit = new Commit(Data);
            const string expected = "Andrei Daboc";
            Assert.Equal(expected, commit.Author);
        }

        [Fact]
        public void ACommitObjectIsCreatedDateIsCalledShouldReturnDate()
        {
            var commit = new Commit(Data);
            const string expected = "2023-03-02";
            Assert.Equal(expected, commit.Date);
        }

        [Fact]
        public void ACommitObjectIsCreatedMessageIsCalledShouldReturnMessage()
        {
            var commit = new Commit(Data);
            const string expected = "Added a constructor.";
            Assert.Equal(expected, commit.Message);
        }

        [Fact]
        public void ACommitObjectIsCreatedShaIsCalledSHouldReturnSha()
        {
            var commit = new Commit(Data);
            const string expected = "4b4d2b7149900d2175d4a2a69bc52943dad08a37";
            Assert.Equal(expected, commit.Sha);
        }
    }
}