using Moq;
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

        public string GetBranchesNames()
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

        public string GetLastCommitMadeInBranch(string branchName)
        {
            return commandRunner.RunCommand(new GitCommand(fileName, $"log -1 {branchName}"));
        }

        public void ChangeBranch(string branchName)
        {
            commandRunner.ApplyCommand(new GitCommand(fileName, $"checkout {branchName}"));
        }

        public string GetDiffBetweenConsecutiveCommits(string oldId, string newId, string repositoryPath, string filePath)
        {
            return commandRunner.RunCommand(new GitCommand(fileName, $"diff {oldId} {newId} -- {filePath}", repositoryPath));
        }

        public string GetRepoPath()
        {
            return commandRunner.RunCommand(new GitCommand(fileName, "rev-parse --show-toplevel"));
        }

        public string GetStagedFiles()
        {
            return commandRunner.RunCommand(new GitCommand(fileName, "diff --staged --name-only"));
        }

        public string GetUnstagedFiles()
        {
            return commandRunner.RunCommand(new GitCommand(fileName, "ls-files --others --modified --exclude-standard", GetRepoPath()));
        }

        public void UnstageFile(string filePath)
        {
            commandRunner.ApplyCommand(new GitCommand(fileName, $"reset HEAD {filePath}"));
        }

        public void ApplyPatchInReverse(string patchPath)
        {
            commandRunner.ApplyCommand(new GitCommand(fileName, $"apply --reverse {patchPath}"));
        }

        public void RestoreFileToMatchIndex(string targetFile)
        {
            commandRunner.ApplyCommand(new GitCommand(fileName, $"checkout -- {targetFile}"));
        }

        public string GetUnstagedDiff(string filePath, string repositoryPath, bool doesFileExist = false)
        {
            var diff = commandRunner.RunCommand(new GitCommand(fileName, $"diff -- {filePath}", repositoryPath));
            if (doesFileExist && diff == "")
            {
                commandRunner.ApplyCommand(new GitCommand(fileName, $"add -N {filePath}"));
                diff = commandRunner.RunCommand(new GitCommand(fileName, $"diff -- {filePath}", repositoryPath));
            }

            return diff;
        }

        public string GetStagedDiff(string stagedFile, string repositoryPath)
        {
            return commandRunner.RunCommand(new GitCommand(fileName, $"diff --staged -- {stagedFile}", repositoryPath));
        }

        public void Stage(string patchPath)
        {
            commandRunner.ApplyCommand(new GitCommand(fileName, $"apply --cached \"{patchPath}\""));
        }

        public void StageFile(string toAdd)
        {
            commandRunner.ApplyCommand(new GitCommand(fileName, $"add {toAdd}"));
        }

        public string GetOriginalFileLines(string currentFileName, string repoPath)
        {
            return commandRunner.RunCommand(
                new GitCommand(
                    fileName,
                    $"show {GetLastFileCommitId(currentFileName, repoPath)}:{currentFileName}",
                    repoPath));
        }

        public string GetStagedFileLines(string currentFileName, string repoPath)
        {
            return commandRunner.RunCommand(new GitCommand(fileName, $"show :{currentFileName}", repoPath));
        }

        public void StageAllFiles()
        {
            commandRunner.ApplyCommand(new GitCommand(fileName, "add ."));
        }

        public void UnstageAllFiles()
        {
            commandRunner.ApplyCommand(new GitCommand(fileName, "reset HEAD ."));
        }

        public void Commit(string message)
        {
            commandRunner.ApplyCommand(new GitCommand(fileName, $"commit -m \"{message}\""));
        }

        private string GetLastFileCommitId(string currentFileName, string repoPath)
        {
            return commandRunner.RunCommand(new GitCommand(fileName, $"log -n 1 --pretty=format:%H -- {currentFileName}", repoPath))[..7];
        }
    }
}