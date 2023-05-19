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
            TextWriter writer = new StringWriter();
            var display = new DisplayText(commits, writer);
            var initialData = new InitialData(manager);
            display.Display(new ConsoleWrapper(), initialData.GetText());
            writer.Flush();
            writer.Close();
        }
    }
}