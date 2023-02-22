using GitAppFacts;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace GitApp
{
    public class GitManager
    {
        private readonly string fileName;
        private readonly GitProcessRunner commandRunner;

        public GitManager(string fileName, GitProcessRunner processRunner)
        {
            this.fileName = fileName;
            this.commandRunner = processRunner;
        }

        public string GetCurrentGitBranch()
        {
            return commandRunner.RunCommand(new GitCommand(fileName, "branch --show-current"));
        }

        public string GetNumberOfCommitsToPush(string branchName)
        {
            return commandRunner.RunCommand(new GitCommand(fileName, $"rev-list origin/{branchName}..HEAD --count"));
        }

        public string GetNumberOfCommitsToPull(string branchName)
        {
            return commandRunner.RunCommand(new GitCommand(fileName, $"rev-list --count origin/{branchName} --not HEAD"));
        }

        public string ShowAllBranches()
        {
            return commandRunner.RunCommand(new GitCommand(fileName, "branch"));
        }
    }
}
