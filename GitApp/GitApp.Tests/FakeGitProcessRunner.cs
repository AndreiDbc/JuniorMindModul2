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
        private readonly string path;
        public FakeGitProcessRunner(string path = "")
        {
            this.path = path;
        }

        string IProcessRunner.RunCommand(GitCommand command)
        {
            return GetOutput(command);
        }

        void IProcessRunner.ApplyCommand(GitCommand command)
        {
            ApplyData();
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
                "show: GitApp/GitApp/Hunk.cs" => "//test\n" +
                    "using System;\n" +
                    "using System.Collections.Generic;\n" +
                    "using System.Linq;\n" +
                    "using System.Text;\n" +
                    "using System.Threading.Tasks;\n" +
                    "\n" +
                    "namespace GitApp\n" +
                    "{\n" +
                    "    public class Hunk : IElement\n" +
                    "    {\n" +
                    "        private readonly string rawData;\n" +
                    "        private readonly string index;\n" +
                    "\n" +
                    "        public Hunk(string rawData, string index)\n" +
                    "        {\n" +
                    "            this.rawData = rawData;\n" +
                    "            this.index = index;\n" +
                    "        }\n" +
                    "\n" +
                    "        public string Id { get => index; }\n" +
                    "\n" +
                    "        public string GetElement()\n" +
                    "        {\n" +
                    "            return rawData;\n" +
                    "        }\n" +
                    "    }\n" +
                    "}",
                "log -n 1 --pretty=format:%H -- GitApp/GitApp/Hunk.cs" => "99999999999999",
                "show 9999999:GitApp/GitApp/Hunk.cs" => "using System;\n" +
                    "using System.Collections.Generic;\n" +
                    "using System.Linq;\n" +
                    "using System.Text;\n" +
                    "using System.Threading.Tasks;\n" +
                    "\n" +
                    "namespace GitApp\n" +
                    "{\n" +
                    "    public class Hunk : IElement\n" +
                    "    {\n" +
                    "        private readonly string rawData;\n" +
                    "        private readonly string index;\n" +
                    "\n" +
                    "        public Hunk(string rawData, string index)\n" +
                    "        {\n" +
                    "            this.rawData = rawData;\n" +
                    "            this.index = index;\n" +
                    "        }\n" +
                    "\n" +
                    "        public string Id { get => index; }\n" +
                    "\n" +
                    "        public string GetElement()\n" +
                    "        {\n" +
                    "            return rawData;\n" +
                    "        }\n" +
                    "    }\n" +
                    "}",
                "show :TestClass"=> "using System.Text;",
            _ => "",
            };
        }

        private void ApplyData()
        {
            AppliedData = FileExists();
        }

        private bool FileExists()
        {
            if (System.IO.File.Exists(path))
            {
                return true;
            }
            return false;
        }
    }
}
