using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;

namespace GitApp
{
    public class Program
    {
        static void Main()
        {
            Console.OutputEncoding = Encoding.UTF8;
            var processRunner = new GitProcessRunner();
            var manager = new GitManager("git", processRunner);
            var commits = new FullCommits(manager.GetLog());
            TextWriter writer = new StringWriter();
            var display = new DisplayText(commits, writer, manager, new ConsoleWrapper());
            display.Display();
            writer.Flush();
            writer.Close();
        }
    }
}