using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace GitApp
{
    public class Program
    {
        public static void Main()
        {
            var branch = new GitCommand().GetCurrentGitBranch();
            var command = new GitCommand();
            Console.Write(branch == ""
                ? "You are in 'detached HEAD' state. You can look around, make experimental changes " +
                "and commit them, and you can discard any commits you make in this state without " +
                "impacting any branches by switching back to a branch."
                : $"Branch name: {branch}\n" +
                $"Outgoing: {command.GetNumberOfCommitsToPush(branch)} \n" +
                $"Incoming: {command.GetNumberOfCommitsToPull(branch)}");
        }
    }
}