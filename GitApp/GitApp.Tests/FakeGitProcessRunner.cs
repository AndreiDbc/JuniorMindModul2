using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Sdk;

namespace GitApp.Tests
{
    public class FakeGitProcessRunner : IProcessRunner
    {

        string IProcessRunner.RunCommand(GitCommand command)
        {
            return GetOutput(command);
        }

        void IProcessRunner.ApplyCommand(GitCommand command)
        {
            ApplyData(command);
        }

        public bool AppliedData { get; set; }

        private static string GetOutput(GitCommand command)
        {
            return command.Argument switch
            {
                "branch --show-current" => "master",
                "show --pretty=\"\" --name-only 7f37022" =>
                "GitApp/GitApp/DisplayText.cs\n" +
                "GitApp/GitApp/ConsoleWrapper.cs\n" +
                "GitApp/GitApp/IElement.cs\n" +
                "GitApp/GitApp/Program.cs\n" +
                "GitApp/GitApp/InitialData.cs\n" +
                "GitApp/GitApp/IInteractor.cs\n" +
                "GitApp/GitApp.Tests/DisplayTextTests.cs\n" +
                "GitApp/GitApp.Tests/FakeConsole.cs",
                "rev-parse --show-toplevel" => "",
                "diff bfa2684 7f37022 -- GitApp/GitApp/DisplayText.cs" =>
                    "diff --git a/GitApp/GitApp/DisplayText.cs b/GitApp/GitApp/DisplayText.cs\r\n" +
                    "index 2e671a6..0802a33 100644\r\n" +
                    "--- a/GitApp/GitApp/DisplayText.cs\r\n" +
                    "+++ b/GitApp/GitApp/DisplayText.cs\r\n" +
                    "@@ -192,11 +192,20 @@ namespace GitApp\r\n" +
                    "         private static List<string> SplitRow(string[] data, int limit)\r\n" +
                    "         {\r\n" +
                    "             var result = new List<string>();\r\n" +
                    "+            var codes = \"\";\r\n" +
                    "             for (int i = 0; i < data.Length; i++)\r\n" +
                    "             {\r\n" +
                    "-                for (int j = 0, k = 1; j < data[i].Length; j += limit, k++)\r\n" +
                    "+                var differenceLength = 0;\r\n" +
                    "+                codes = \"\";\r\n" +
                    "+                for (int j = 0, k = 1; j < data[i].Length; j += limit + differenceLength, k++)\r\n" +
                    "                 {\r\n" +
                    "-                    result.Add(data[i][j..Math.Min(data[i].Length, limit * k)]);\r\n" +
                    "+                    var element = data[i][j..Math.Min(data[i].Length, limit * k)];\r\n" +
                    "+                    var elementUtilities = new PageUtilities(element);\r\n" +
                    "+                    var previousDifferenceLength = differenceLength;\r\n" +
                    "+                    var elementCodesAndLength = elementUtilities.FormatingCodesStringUtilities();\r\n" +
                    "+                    differenceLength = elementCodesAndLength.length;\r\n" +
                    "+                    result.Add(codes + data[i][j..Math.Min(data[i].Length, limit * k + (previousDifferenceLength > differenceLength ? previousDifferenceLength : differenceLength))] + \"\\u001b[0m\");\r\n" +
                    "+                    codes = new PageUtilities(result[^1]).FormatingCodesStringUtilities().codes.Replace(\"\\u001b[0m\", \"\");\r\n" +
                    "                 }\r\n" +
                    "             }",
                _ => "",
            };
        }

        private void ApplyData(GitCommand command)
        {
            AppliedData = command.Argument switch
            {
                "add -p -- /GitApp/GitApp/DisplayText.cs" => command.Argument == "add -p -- /GitApp/GitApp/DisplayText.cs" && command.StartLine == 192 && command.EndLine == 203,
                _ => false,
            };
        }
    }
}
