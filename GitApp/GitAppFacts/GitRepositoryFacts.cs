namespace GitAppFacts
{
    public class GitRepositoryFacts
    {
        [Fact]
        public void CommitIsCalledShouldUpdateBranch()
        {
            var branches = new Dictionary<string, List<string>>()
            {
              { "branch1", new List<string>() },
              { "branch2", new List<string>() }
            };

            var success = new GitRepository(branches, "branch1").Commit("commit");
            Assert.True(success);
            Assert.Equal(1, branches["branch1"].Count);
        }

        [Fact]
        public void CommitIsCalledSecondBranchShouldRemainTheSame()
        {
            var branches = new Dictionary<string, List<string>>()
            {
              { "branch1", new List<string>() },
              { "branch2", new List<string>() }
            };

            var success = new GitRepository(branches, "branch1").Commit("commit");
            Assert.True(success);
            Assert.Equal(0, branches["branch2"].Count);
        }
    }
}