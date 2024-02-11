using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace GitApp
{
    public class RepositoryInfo
    {
        public RepositoryInfo(GitManager gitManager, string repositoryPath)
        {
            var branchName = gitManager.GetCurrentGitBranch();
            NoOfCommitsToPull = gitManager.GetNumberOfCommitsToPull(branchName);
            NoOfCommitsToPush = gitManager.GetNumberOfCommitsToPush(branchName);
            RepositoryPath = repositoryPath;
            CurrentBranchName = gitManager.GetCurrentGitBranch();
        }

        public string NoOfCommitsToPull { get; set; }

        public string NoOfCommitsToPush { get; set; }

        public string RepositoryPath { get; }

        public string CurrentBranchName { get; set; }
    }
}
