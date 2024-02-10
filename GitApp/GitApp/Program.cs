using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;

namespace GitApp
{
    public class Program
    {
        static void Main()
        {
            new ConsoleWrapper().SetOutputEncodingToUTF8();
            var processRunner = new GitProcessRunner();
            var repositoryPath = processRunner.RunCommand(new GitCommand("git", "rev-parse --show-toplevel"));
            var manager = new GitManager("git", processRunner, repositoryPath);
            var commits = new FullCommits(manager.GetLog());
            var display = new DisplayText(commits, manager, new ConsoleWrapper(), new RepositoryInfo(manager, repositoryPath));
            display.Display();
        }
    }
}