using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
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
        private readonly string repositoryPath;

        public GitManager(string fileName, IProcessRunner processRunner, string repositoryPath)
        {
            this.fileName = fileName;
            this.commandRunner = processRunner;
            this.repositoryPath = repositoryPath;
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
            commandRunner.ApplyCommand(new GitCommand(fileName, $"checkout {branchName}"), repositoryPath);
        }

        public string GetDiffBetweenConsecutiveCommits(string oldId, string newId, string filePath)
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
            return commandRunner.RunCommand(new GitCommand(fileName, "ls-files --others --modified --exclude-standard", repositoryPath));
        }

        public void UnstageFile(string filePath)
        {
            commandRunner.ApplyCommand(new GitCommand(fileName, $"reset HEAD {filePath}"), repositoryPath);
        }

        public void ApplyPatchInReverse(string patchPath)
        {
            commandRunner.ApplyCommand(new GitCommand(fileName, $"apply --reverse {patchPath}"), repositoryPath);
        }

        public void RestoreFileToMatchIndex(string targetFile)
        {
            commandRunner.ApplyCommand(new GitCommand(fileName, $"checkout -- {targetFile}"), repositoryPath);
        }

        public string GetUnstagedDiff(string filePath, bool doesFileExist = false)
        {
            var diff = commandRunner.RunCommand(new GitCommand(fileName, $"diff -- {filePath}", repositoryPath));
            if (doesFileExist && diff == "")
            {
                commandRunner.ApplyCommand(new GitCommand(fileName, $"add -N {filePath}"), repositoryPath);
                diff = commandRunner.RunCommand(new GitCommand(fileName, $"diff -- {filePath}", repositoryPath));
            }

            return diff;
        }

        public string GetStagedDiff(string stagedFile)
        {
            return commandRunner.RunCommand(new GitCommand(fileName, $"diff --staged -- {stagedFile}", repositoryPath));
        }

        public void Stage(string patchPath)
        {
            commandRunner.ApplyCommand(new GitCommand(fileName, $"apply --cached \"{patchPath}\""), repositoryPath);
        }

        public void StageFile(string toAdd)
        {
            commandRunner.ApplyCommand(new GitCommand(fileName, $"add {toAdd}"), repositoryPath);
        }

        public string GetOriginalFileLines(string currentFileName, string repoPath)
        {
            return commandRunner.RunCommand(
                new GitCommand(
                    fileName,
                    $"show {GetLastFileCommitId(currentFileName)}:{currentFileName}",
                    repoPath));
        }

        public void StageAllFiles()
        {
            commandRunner.ApplyCommand(new GitCommand(fileName, "add ."), repositoryPath);
        }

        public void UnstageAllFiles()
        {
            commandRunner.ApplyCommand(new GitCommand(fileName, "reset HEAD ."), repositoryPath);
        }

        public void Commit(string message)
        {
            commandRunner.ApplyCommand(new GitCommand(fileName, $"commit -m \"{message}\""), repositoryPath);
        }

        public void RestoreDeletedFile(string toRestore)
        {
            commandRunner.ApplyCommand(new GitCommand(fileName, $"restore --source=HEAD --staged --worktree -- {toRestore}"), repositoryPath);
        }

        private string GetLastFileCommitId(string currentFileName)
        {
            return commandRunner.RunCommand(new GitCommand(fileName, $"log -n 1 --pretty=format:%H -- {currentFileName}", repositoryPath))[..7];
        }
    }
}