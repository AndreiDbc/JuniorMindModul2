using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace GitApp
{
    public class Program
    {
        public static void Main()
        {
            var manager = new GitManager("git");
            var branch = manager.GetCurrentGitBranch();
            const string detachedHeadMessage = "You are in 'detached HEAD' state. You can look around, make" +
                " experimental changes and commit them, and you can discard any commits you make in " +
                "this state without impacting any branches by switching back to a branch.";
            var initialMessage = $"Current branch name: {branch}\n" +
                $"Outgoing: {manager.GetNumberOfCommitsToPush(branch)}\n" +
                $"Incoming: {manager.GetNumberOfCommitsToPull(branch)}\n" +
                $"Branches: {manager.ShowAllBranches()}";
            Console.WriteLine(branch == ""
                ? detachedHeadMessage
                : initialMessage);
            var display = new DisplayText(manager.GetCommits());
            display.Interract(initialMessage);
        }
    }
}