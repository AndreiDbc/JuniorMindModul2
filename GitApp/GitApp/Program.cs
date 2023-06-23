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
            var display = new DisplayText(commits, writer, manager);
            var initialData = new InitialData(manager);
            display.Display(new ConsoleWrapper(), initialData.GetText());
            writer.Flush();
            writer.Close();
        }
    }
}