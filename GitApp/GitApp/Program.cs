using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace GitApp
{
    public class Program
    {
        public static void Main()
        {
            var branch = GetCurrentGitBranch();
            var command = new GitCommand(branch);

            Console.Write(branch == ""
                ? "You are in 'detached HEAD' state. You can look around, make experimental changes " +
                "and commit them, and you can discard any commits you make in this state without " +
                "impacting any branches by switching back to a branch."
                : $"Branch name: {branch}\n" +
                $"Outgoing: {command.GetNumberOfCommitsToPush()} \n" +
                $"Incoming: {command.GetNumberOfCommitsToPull()}");
        }

        public static string GetCurrentGitBranch()
        {
            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "git",
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
    }
}