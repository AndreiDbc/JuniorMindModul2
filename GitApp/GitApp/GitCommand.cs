using GitAppFacts;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace GitApp
{
    public class GitCommand
    {
        private readonly string fileName;

        public GitCommand(string fileName)
        {
            this.fileName = fileName;
        }

        public string GetCurrentGitBranch()
        {
            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = fileName,
                    Arguments = "branch --show-current",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };
            process.Start();
            var branch = process.StandardOutput.ReadToEnd();
            process.Dispose();
            return branch.Trim();
        }

        public string GetNumberOfCommitsToPush(string branchName)
        {
            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = fileName,
                    Arguments = $"rev-list origin/{branchName}..HEAD --count",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };
            process.Start();
            var commits = process.StandardOutput.ReadToEnd();
            process.Dispose();
            return commits.Trim();
        }

        public string GetNumberOfCommitsToPull(string branchName)
        {
            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = fileName,
                    Arguments = $"rev-list --count origin/{branchName} --not HEAD",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };
            process.Start();
            var commits = process.StandardOutput.ReadToEnd();
            process.Dispose();
            return commits.Trim();
        }

        public string ShowAllBranches()
        {
            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = fileName,
                    Arguments = "branch",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };
            process.Start();
            var commits = process.StandardOutput.ReadToEnd();
            process.Dispose();
            return commits.Trim();
        }
    }
}
