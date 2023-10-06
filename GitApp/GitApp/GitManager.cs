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
        private readonly IProcessRunner commandRunner;

        public GitManager(string fileName, IProcessRunner processRunner)
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

        public string GetCommitModifiedFileNames(string commit)
        {
            return commandRunner.RunCommand(new GitCommand(fileName, $"show --pretty=\"\" --name-only {commit}"));
        }

        public string GetLog()
        {
            return commandRunner.RunCommand(new GitCommand(fileName, "log"));
        }

        public string GetDiffBetweenConsecutiveCommits(string oldId, string newId, string repositoryPath, string filePath)
        {
            return commandRunner.RunCommand(new GitCommand(fileName, $"diff {oldId} {newId} -- {filePath}", repositoryPath));
        }

        public string GetRepoPath()
        {
            return commandRunner.RunCommand(new GitCommand(fileName, "rev-parse --show-toplevel"));
        }

        public string GetUnstagedFiles(string repositoryPath)
        {
            return commandRunner.RunCommand(new GitCommand(fileName, "status --short", repositoryPath));
        }

        public string GetStagedFiles()
        {
            return commandRunner.RunCommand(new GitCommand(fileName, "diff --staged --name-only"));
        }

        public string GetUnstagedDiff(string filePath, string repositoryPath)
        {
            return commandRunner.RunCommand(new GitCommand(fileName, $"diff {filePath}", repositoryPath));
        }

        public void Stage(string patchName, string repoPath)
        {
            commandRunner.ApplyCommand(new GitCommand(fileName, $"add {patchName}", repoPath));
        }

        public void StageFile(string repoPath, string toAdd)
        {
            commandRunner.ApplyCommand(new GitCommand(fileName, $"add {toAdd}", repoPath));
        }
    }
}