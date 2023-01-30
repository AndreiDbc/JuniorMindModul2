using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace GitApp
{
    public class Program
    {
        public static void Main()
        {
        }

        public static string GetCurrentGitBranch()
        {
            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "git",
                    Arguments = "branch --show-current",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };
            process.Start();
            var branch = process.StandardOutput.ReadToEnd();
            process.Dispose();
            return branch.Trim();
        }
    }
}