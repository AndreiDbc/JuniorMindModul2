using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace GitApp
{
    public class GitProcessRunner : IProcessRunner
    {
        public string RunCommand(GitCommand command)
        {
            return GetCommandOutput(command);
        }

        public void ApplyCommand(GitCommand command)
        {
            AddData(command);
        }

        private static string GetCommandOutput(GitCommand command)
        {
            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = command.FileName,
                    Arguments = command.Argument,
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };

            if (command.Path != "")
            {
                process.StartInfo.WorkingDirectory = command.Path;
            }

            process.Start();
            var result = process.StandardOutput.ReadToEnd();
            process.Dispose();
            return result.TrimEnd();
        }

        private static void AddData(GitCommand command)
        {
            var workingDirectory = GetCommandOutput(new GitCommand("git", "rev-parse --show-toplevel"));

            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = command.FileName,
                    Arguments = command.Argument,
                    WorkingDirectory = workingDirectory,
                    RedirectStandardInput = command.ShouldRedirectInput,
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };

            process.Start();
            if (command.ShouldRedirectInput)
            {
                RedirectInput(process, command.Path);
                process.StandardInput.Close();
            }

            process.WaitForExit();
            process.Dispose();
        }

        private static void RedirectInput(Process process, string path)
        {
            using (StreamReader reader = new StreamReader(path))
            {
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    process.StandardInput.WriteLine(line);
                }
            }
        }
    }
}