using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitApp.Tests
{
    public class FakeGitProcessRunner : IProcessRunner
    {
        string IProcessRunner.RunCommand(GitCommand command)
        {
            return GetOutput(command);
        }

        private static string GetOutput(GitCommand command)
        {
            return command.Argument switch
            {
                "branch --show-current" => "master",
                _ => "",
            };
        }
    }
}
