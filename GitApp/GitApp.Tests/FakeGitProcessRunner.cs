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
                "show --pretty=\"\" --name-only 7f37022"=> 
                "GitApp/GitApp/ConsoleWrapper.cs\n" +
                "GitApp/GitApp/DisplayText.cs\n"+
                "GitApp/GitApp/IElement.cs\n"+
                "GitApp/GitApp/Program.cs\n"+
                "GitApp/GitApp/InitialData.cs\n"+
                "GitApp/GitApp/IInteractor.cs\n"+
                "GitApp/GitApp.Tests/DisplayTextTests.cs\n" +
                "GitApp/GitApp.Tests/FakeConsole.cs",
                _ => "",
            };
        }
    }
}
