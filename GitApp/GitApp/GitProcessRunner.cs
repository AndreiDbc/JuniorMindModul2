using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
            return result.Trim();
        }
    }
}
