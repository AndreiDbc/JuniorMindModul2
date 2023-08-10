using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace GitApp
{
    public class Program
    {
        static void Main()
        {
            var processRunner = new GitProcessRunner();
            var manager = new GitManager("git", processRunner);
            var commits = new FullCommits(manager.GetLog());
            TextWriter writer = new StringWriter();
            var display = new DisplayText(commits, writer, manager, new ConsoleWrapper());
            display.DisplayLog();
            writer.Flush();
            writer.Close();
        }
    }
}