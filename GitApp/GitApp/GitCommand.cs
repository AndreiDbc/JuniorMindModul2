using GitAppFacts;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitApp
{
    public class GitCommand
    {
        private readonly string branchName;

        public GitCommand(string branchName)
        {
            this.branchName = branchName;
        }

        public string GetNumberOfCommitsToPush()
        {
            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "git",
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

        public string GetNumberOfCommitsToPull()
        {
            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "git",
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
    }
}
