using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitApp.Tests
{
    public class DiffTests
    {
        [Fact]
        public void GetTextIsCalledShouldReturnText()
        {
            var diff = new Diff(new GitManager("git", new FakeGitProcessRunner()).GetDiffBetweenConsecutiveCommits("bfa2684", "7f37022", "", "GitApp/GitApp/DisplayText.cs"));
            var expected = new List<string>()
            {
                "diff --git a/GitApp/GitApp/DisplayText.cs b/Gi\u001b[0m",
                "tApp/GitApp/DisplayText.cs\r\u001b[0m",
                "index 2e671a6..0802a33 100644\r\u001b[0m",
                "--- a/GitApp/GitApp/DisplayText.cs\r\u001b[0m",
                "+++ b/GitApp/GitApp/DisplayText.cs\r\u001b[0m",
                "@@ -192,11 +192,20 @@ namespace GitApp\r\u001b[0m",
                "         private static List<string> SplitRow(\u001b[0m",
                "string[] data, int limit)\r\u001b[0m",
                "         {\r\u001b[0m",
                "             var result = new List<string>();\r\u001b[0m",
                "\u001b[38;5;40m+            var codes = \"\";\r\u001b[0m",
                "             for (int i = 0; i < data.Length; \u001b[0m",
                "i++)\r\u001b[0m",
                "             {\r\u001b[0m",
                "\u001b[38;5;190m-                for (int j = 0, k = 1; j < da\u001b[0m",
                "\u001b[38;5;190mta[i].Length; j += limit, k++)\r\u001b[0m",
                "\u001b[38;5;190m+                var differenceLength = 0;\r\u001b[0m",
                "\u001b[38;5;40m+                codes = \"\";\r\u001b[0m",
                "\u001b[38;5;40m+                for (int j = 0, k = 1; j < da\u001b[0m",
                "\u001b[38;5;40mta[i].Length; j += limit + differenceLength, k\u001b[0m",
                "\u001b[38;5;40m++)\r\u001b[0m",
                "                 {\r\u001b[0m",
                "\u001b[38;5;190m-                    result.Add(data[i][j..Mat\u001b[0m",
                "\u001b[38;5;190mh.Min(data[i].Length, limit * k)]);\r\u001b[0m",
                "\u001b[38;5;190m+                    var element = data[i][j..\u001b[0m",
                "\u001b[38;5;190mMath.Min(data[i].Length, limit * k)];\r\u001b[0m",
                "\u001b[38;5;40m+                    var elementUtilities = ne\u001b[0m",
                "\u001b[38;5;40mw PageUtilities(element);\r\u001b[0m..."
            };
            var actual = diff.GetHighlightedSide(30, 46, 1288);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetPageDataAndSizeIsCalledShouldReturnPageAndSize()
        {
            var diff = new Diff(new GitManager("git", new FakeGitProcessRunner()).GetDiffBetweenConsecutiveCommits("bfa2684", "7f37022", "", "GitApp/GitApp/DisplayText.cs"));
            var expected = new List<IElement>()
            {
                new DiffLine("diff --git a/GitApp/GitApp/DisplayText.cs b/GitApp/GitApp/DisplayText.cs\r", "0"),
                new DiffLine("index 2e671a6..0802a33 100644\r", "1"),
                new DiffLine("--- a/GitApp/GitApp/DisplayText.cs\r", "2"),
                new DiffLine("+++ b/GitApp/GitApp/DisplayText.cs\r", "3"),
                new DiffLine("@@ -192,11 +192,20 @@ namespace GitApp\r", "4"),
                new DiffLine("         private static List<string> SplitRow(string[] data, int limit)\r", "5"),
                new DiffLine("         {\r", "6"),
                new DiffLine("             var result = new List<string>();\r", "7"),
                new DiffLine("+            var codes = \"\";\r", "8"),
                new DiffLine("             for (int i = 0; i < data.Length; i++)\r", "9"),
                new DiffLine("             {\r", "10"),
                new DiffLine("-                for (int j = 0, k = 1; j < data[i].Length; j += limit, k++)\r", "11"),
                new DiffLine("+                var differenceLength = 0;\r", "12"),
                new DiffLine("+                codes = \"\";\r", "13"),
                new DiffLine("+                for (int j = 0, k = 1; j < data[i].Length; j += limit + differenceLength, k++)\r", "14"),
                new DiffLine("                 {\r", "15"),
                new DiffLine("-                    result.Add(data[i][j..Math.Min(data[i].Length, limit * k)]);\r", "16"),
                new DiffLine("+                    var element = data[i][j..Math.Min(data[i].Length, limit * k)];\r", "17"),
                new DiffLine("+                    var elementUtilities = new PageUtilities(element);\r", "18")
            };
            var actual = diff.GetPageDataAndSize(1288, 46);
            Assert.Equal(expected[0].Id, actual.list[0].Id);
            Assert.Equal(expected[1].Id, actual.list[1].Id);
            Assert.Equal(expected[2].Id, actual.list[2].Id);
            Assert.Equal(expected[3].Id, actual.list[3].Id);
            Assert.Equal(expected[4].Id, actual.list[4].Id);
            Assert.Equal(expected[5].Id, actual.list[5].Id);
            Assert.Equal(expected[6].Id, actual.list[6].Id);
            Assert.Equal(expected[7].Id, actual.list[7].Id);
            Assert.Equal(expected[8].Id, actual.list[8].Id);
            Assert.Equal(expected[9].Id, actual.list[9].Id);
            Assert.Equal(expected[10].Id, actual.list[10].Id);
            Assert.Equal(expected[11].Id, actual.list[11].Id);
            Assert.Equal(expected[12].Id, actual.list[12].Id);
            Assert.Equal(expected[13].Id, actual.list[13].Id);
            Assert.Equal(expected[14].Id, actual.list[14].Id);
            Assert.Equal(expected[15].Id, actual.list[15].Id);
            Assert.Equal(expected[16].Id, actual.list[16].Id);
            Assert.Equal(expected[17].Id, actual.list[17].Id);
            Assert.Equal(expected[18].Id, actual.list[18].Id);
            Assert.Equal(1288, actual.textSize);
        }

        [Fact]
        public void GetHighlightedSideIsCalledShouldReturnLines()
        {
            var diff = new Diff(new GitManager("git", new FakeGitProcessRunner()).GetDiffBetweenConsecutiveCommits("bfa2684", "7f37022", "", "GitApp/GitApp/DisplayText.cs"));
            var expected = new List<string>()
            {
                "diff --git a/GitApp/GitApp/DisplayText.cs b/Gi\u001b[0m",
                "tApp/GitApp/DisplayText.cs\r\u001b[0m",
                "index 2e671a6..0802a33 100644\r\u001b[0m",
                "--- a/GitApp/GitApp/DisplayText.cs\r\u001b[0m",
                "+++ b/GitApp/GitApp/DisplayText.cs\r\u001b[0m",
                "@@ -192,11 +192,20 @@ namespace GitApp\r\u001b[0m",
                "         private static List<string> SplitRow(\u001b[0m",
                "string[] data, int limit)\r\u001b[0m",
                "         {\r\u001b[0m",
                "             var result = new List<string>();\r\u001b[0m",
                "\u001b[38;5;40m+            var codes = \"\";\r\u001b[0m",
                "             for (int i = 0; i < data.Length; \u001b[0m",
                "i++)\r\u001b[0m",
                "             {\r\u001b[0m",
                "\u001b[38;5;190m-                for (int j = 0, k = 1; j < da\u001b[0m",
                "\u001b[38;5;190mta[i].Length; j += limit, k++)\r\u001b[0m",
                "\u001b[38;5;190m+                var differenceLength = 0;\r\u001b[0m",
                "\u001b[38;5;40m+                codes = \"\";\r\u001b[0m",
                "\u001b[38;5;40m+                for (int j = 0, k = 1; j < da\u001b[0m",
                "\u001b[38;5;40mta[i].Length; j += limit + differenceLength, k\u001b[0m",
                "\u001b[38;5;40m++)\r\u001b[0m",
                "                 {\r\u001b[0m",
                "\u001b[38;5;190m-                    result.Add(data[i][j..Mat\u001b[0m",
                "\u001b[38;5;190mh.Min(data[i].Length, limit * k)]);\r\u001b[0m",
                "\u001b[38;5;190m+                    var element = data[i][j..\u001b[0m",
                "\u001b[38;5;190mMath.Min(data[i].Length, limit * k)];\r\u001b[0m",
                "\u001b[38;5;40m+                    var elementUtilities = ne\u001b[0m",
                "\u001b[38;5;40mw PageUtilities(element);\r\u001b[0m..."
            };
            var output = diff.GetHighlightedSide(30, 46, 1288);
            Assert.Equal(expected, output);
        }

        [Fact]
        public void GetHighlightedSideIsCalledDiffSideIsInUseShouldReturnLines()
        {
            var diff = new Diff(new GitManager("git", new FakeGitProcessRunner()).GetDiffBetweenConsecutiveCommits("bfa2684", "7f37022", "", "GitApp/GitApp/DisplayText.cs"));
            var expected = new List<string>()
            {
                "\u001b[7mdiff --git a/GitApp/GitApp/DisplayText.cs b/Gi\u001b[0m",
                "\u001b[7mtApp/GitApp/DisplayText.cs\r\u001b[0m",
                "index 2e671a6..0802a33 100644\r\u001b[0m",
                "--- a/GitApp/GitApp/DisplayText.cs\r\u001b[0m",
                "+++ b/GitApp/GitApp/DisplayText.cs\r\u001b[0m",
                "@@ -192,11 +192,20 @@ namespace GitApp\r\u001b[0m",
                "         private static List<string> SplitRow(\u001b[0m",
                "string[] data, int limit)\r\u001b[0m",
                "         {\r\u001b[0m",
                "             var result = new List<string>();\r\u001b[0m",
                "\u001b[38;5;40m+            var codes = \"\";\r\u001b[0m",
                "             for (int i = 0; i < data.Length; \u001b[0m",
                "i++)\r\u001b[0m",
                "             {\r\u001b[0m",
                "\u001b[38;5;190m-                for (int j = 0, k = 1; j < da\u001b[0m",
                "\u001b[38;5;190mta[i].Length; j += limit, k++)\r\u001b[0m",
                "\u001b[38;5;190m+                var differenceLength = 0;\r\u001b[0m",
                "\u001b[38;5;40m+                codes = \"\";\r\u001b[0m",
                "\u001b[38;5;40m+                for (int j = 0, k = 1; j < da\u001b[0m",
                "\u001b[38;5;40mta[i].Length; j += limit + differenceLength, k\u001b[0m",
                "\u001b[38;5;40m++)\r\u001b[0m",
                "                 {\r\u001b[0m",
                "\u001b[38;5;190m-                    result.Add(data[i][j..Mat\u001b[0m",
                "\u001b[38;5;190mh.Min(data[i].Length, limit * k)]);\r\u001b[0m",
                "\u001b[38;5;190m+                    var element = data[i][j..\u001b[0m",
                "\u001b[38;5;190mMath.Min(data[i].Length, limit * k)];\r\u001b[0m",
                "\u001b[38;5;40m+                    var elementUtilities = ne\u001b[0m",
                "\u001b[38;5;40mw PageUtilities(element);\r\u001b[0m..."
            };
            var output = diff.GetHighlightedSide(30, 46, 1288, true);
            Assert.Equal(expected, output);
        }
    }
}
