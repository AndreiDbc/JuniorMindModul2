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
            var manager = new GitManager("git", processRunner);
            var commits = new FullCommits(manager.GetLog());
            TextWriter writer = new StringWriter();
            var display = new DisplayText(commits, writer, manager, new ConsoleWrapper(), new RepositoryInfo(manager));
            display.Display();
            writer.Flush();
            writer.Close();
        }
    }
}