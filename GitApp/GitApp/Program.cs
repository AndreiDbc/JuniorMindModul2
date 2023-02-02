using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace GitApp
{
    public class Program
    {
        public static void Main()
        {
            var gitCommand = new GitCommand("git");
            var branch = gitCommand.GetCurrentGitBranch();
            const string detachedHeadMessage = "You are in 'detached HEAD' state. You can look around, make" +
                " experimental changes and commit them, and you can discard any commits you make in " +
                "this state without impacting any branches by switching back to a branch.";
            var initialMessage = $"Current branch name: {branch}\n" +
                $"Outgoing: {gitCommand.GetNumberOfCommitsToPush(branch)}\n" +
                $"Incoming: {gitCommand.GetNumberOfCommitsToPull(branch)}\n" +
                $"Branches: {gitCommand.ShowAllBranches()}";
            Console.WriteLine(branch == ""
                ? detachedHeadMessage
                : initialMessage);
            var option = new DisplayOptions(gitCommand.GetCommits());
            option.Interract(initialMessage);
        }
    }
}