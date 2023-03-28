using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace GitApp
{
    public class Program
    {
        static void Main()
        {
            var manager = new GitManager("git", new GitProcessRunner());
            var commits = new FullCommits(manager.GetLog());
            var display = new DisplayText(commits);
            display.Display();
        }
    }
}