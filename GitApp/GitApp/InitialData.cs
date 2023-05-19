using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace GitApp
{
    public class InitialData
    {
        private readonly GitManager manager;

        public InitialData(GitManager manager)
        {
            this.manager = manager;
        }

        public string GetText()
        {
            var branch = manager.GetCurrentGitBranch();
            return branch == ""
                    ? "You are in 'detached HEAD' state. You can look around, make" +
                    " experimental changes and commit them, and you can discard any commits you make in " +
                    "this state without impacting any branches by switching back to a branch."
                    : $"Current branch name: {branch}\n" +
                    $"Outgoing: {manager.GetNumberOfCommitsToPush(branch)}\n" +
                    $"Incoming: {manager.GetNumberOfCommitsToPull(branch)}\n" +
                    $"Branches: {manager.ShowAllBranches()}";
        }
    }
}
