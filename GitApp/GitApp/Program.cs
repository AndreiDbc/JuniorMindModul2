using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace GitApp
{
    public class Program
    {
        public static void Main()
        {
            var manager = new GitManager("git", new GitProcessRunner());
            var display = new DisplayText(
                new CommitsIDs(manager.GetCommitId().Split("\n").ToList()),
                new CommitsAuthorsName(manager.GetCommitAuthorName().Split("\n").ToList()),
                new CommitsDates(manager.GetCommitDate().Split("\n").ToList()),
                new CommitsMessages(manager.GetCommitMessage().Split("\n").ToList()));
            display.Interract();
        }
    }
}