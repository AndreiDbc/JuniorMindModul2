using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitApp
{
    public class TestClass
    {
        private readonly string repoPath;
        private readonly IConsoleWrapper consoleWrapper;

        public TestClass(string repoPath, IConsoleWrapper consoleWrapper)
        {
            this.repoPath = repoPath;
            this.consoleWrapper = consoleWrapper;
        }

        public void DisplayTestMenu(ref ConsoleKey key)
        {
            consoleWrapper.Clear();
            do
            {
                consoleWrapper.SetCursorPosition(0, 0);
                consoleWrapper.Write("Do you want the test menu to start the diff process? [y/n]");
                key = consoleWrapper.ReadKey().Key;
                if (key.Equals(ConsoleKey.Y))
                {
                    consoleWrapper.Clear();
                    GetPage(true);
                    consoleWrapper.Clear();
                }
                else if (key.Equals(ConsoleKey.N))
                {
                    consoleWrapper.Clear();
                    GetPage(false);
                    consoleWrapper.Clear();
                }

                Thread.Sleep(100);
            }
            while (key != ConsoleKey.Escape);
        }

        private void GetPage(bool shouldStartDiffProcess)
        {
            var key = ConsoleKey.NoName;
            var menu = new Menu(consoleWrapper).UpdateMenu(false, key);
            var topMessage = shouldStartDiffProcess ? "Diff process is in use" : "Diff process is not in use";
            menu.Insert(0, topMessage);
            var messages = shouldStartDiffProcess ? StartProcess() : ("N/A", "N/A");
            menu.Add("StandardOutput message: " + messages.Item1);
            menu.Add("StandardError message: " + messages.Item2);
            consoleWrapper.Write(string.Join(Environment.NewLine, menu));
            do
            {
                key = consoleWrapper.ReadKey().Key;
                consoleWrapper.SetCursorPosition(0, 0);
                menu = new Menu(consoleWrapper).UpdateMenu(false, key);
                menu.Insert(0, topMessage);
                messages = shouldStartDiffProcess ? StartProcess() : ("N/A", "N/A");
                menu.Add("StandardOutput message: " + messages.Item1);
                menu.Add("StandardError message: " + messages.Item2);
                consoleWrapper.Write(string.Join(Environment.NewLine, menu));
            }
            while (key != ConsoleKey.Escape);
        }

        private (string, string) StartProcess()
        {
            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "git",
                    Arguments = "diff -- GitApp/GitApp/TestClass.cs",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    WorkingDirectory = repoPath,
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };

            process.Start();
            var output = process.StandardOutput.ReadToEnd().Split("\n")[0];
            var error = process.StandardError.ReadToEnd().Split("\n")[0];
            process.Dispose();
            var windowWidth = consoleWrapper.WindowWidth;

            return (output[.. Math.Min(output.Length, windowWidth - "StandardOutput message: ".Length)],
             error[.. Math.Min(error.Length, windowWidth - "StandardError message: ".Length)]);
        }
    }
}
