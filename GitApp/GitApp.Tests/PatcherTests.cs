using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace GitApp.Tests
{
    public class PatcherTests
    {
        [Fact]
        public void GetLinePatchPathIsCalledCurrentElementIsReplacedAndStartsWithMinusShouldReturnPatchPath()
        {
            var diffText = "diff --git a/GitApp/GitApp/Hunk.cs b/GitApp/GitApp/Hunk.cs\n" +
                "index c0fc9dc..d63a54b 100644\n" +
                "--- a/GitApp/GitApp/Hunk.cs\n" +
                "+++ b/GitApp/GitApp/Hunk.cs\n"+
                "@@ -1,7 +1,7 @@\n" +
                " ∩╗┐using System;\n" +
                " using System.Collections.Generic;\n" +
                " using System.Linq;\n" +
                "-using System.Text;\n" +
                "+using System.Text;//test\n" +
                " using System.Threading.Tasks;\n" +
                " \n" +
                " namespace GitApp";
            var diff = new Diff(diffText);
            diff.Current = 8;
            var fakeProcessRunner = new FakeGitProcessRunner();
            var patcher = new Patcher("", new GitManager("GitApp/GitApp/Hunk.cs", fakeProcessRunner), diff, true);
            var path = patcher.GetLinePatchPath(true, false, "GitApp/GitApp/Hunk.cs");
            var result = System.IO.File.ReadAllText(path);
            System.IO.File.Delete(path);
            const string expected = "diff --git a/GitApp/GitApp/Hunk.cs b/GitApp/GitApp/Hunk.cs\n" +
                "index c0fc9dc..d63a54b 100644\n" +
                "--- a/GitApp/GitApp/Hunk.cs\n" +
                "+++ b/GitApp/GitApp/Hunk.cs\n" +
                "@@ -1,7 +1,7 @@\n" +
                " using System;\n" +
                " using System.Collections.Generic;\n" +
                " using System.Linq;\n" +
                "-using System.Text;\n" +
                "+using System.Text;//test\n" +
                " using System.Threading.Tasks;\n" +
                " \n" +
                " namespace GitApp\n\n";
            Assert.Equal(expected, result);
        }

        [Fact]
        public void GetLinePatchPathIsCalledCurrentElementIsReplacedAndStartsWithPlusShouldReturnPatchPath()
        {
            var diffText = "diff --git a/GitApp/GitApp/Hunk.cs b/GitApp/GitApp/Hunk.cs\n" +
                "index c0fc9dc..d63a54b 100644\n" +
                "--- a/GitApp/GitApp/Hunk.cs\n" +
                "+++ b/GitApp/GitApp/Hunk.cs\n" +
                "@@ -1,7 +1,7 @@\n" +
                " ∩╗┐using System;\n" +
                " using System.Collections.Generic;\n" +
                " using System.Linq;\n" +
                "-using System.Text;\n" +
                "+using System.Text;//test\n" +
                " using System.Threading.Tasks;\n" +
                " \n" +
                " namespace GitApp";
            var diff = new Diff(diffText);
            diff.Current = 9;
            var fakeProcessRunner = new FakeGitProcessRunner();
            var patcher = new Patcher("", new GitManager("GitApp/GitApp/Hunk.cs", fakeProcessRunner), diff, true);
            var path = patcher.GetLinePatchPath(true, false, "GitApp/GitApp/Hunk.cs");
            var result = System.IO.File.ReadAllText(path);
            System.IO.File.Delete(path);
            const string expected = "diff --git a/GitApp/GitApp/Hunk.cs b/GitApp/GitApp/Hunk.cs\n" +
                "index c0fc9dc..d63a54b 100644\n" +
                "--- a/GitApp/GitApp/Hunk.cs\n" +
                "+++ b/GitApp/GitApp/Hunk.cs\n" +
                "@@ -1,7 +1,7 @@\n" +
                " using System;\n" +
                " using System.Collections.Generic;\n" +
                " using System.Linq;\n" +
                "-using System.Text;\n" +
                "+using System.Text;//test\n" +
                " using System.Threading.Tasks;\n" +
                " \n" +
                " namespace GitApp\n\n";
            Assert.Equal(expected, result);
        }

        [Fact]
        public void GetLinePatchPathIsCalledCurrentElementIsAddedShouldReturnPatchPath()
        {
            var diffText = "diff --git a/GitApp/GitApp/Hunk.cs b/GitApp/GitApp/Hunk.cs\n" +
                        "index c0fc9dc..d63a54b 100644\n" +
                        "--- a/GitApp/GitApp/Hunk.cs\n" +
                        "+++ b/GitApp/GitApp/Hunk.cs\n" +
                        "@@ -2,6 +2,7 @@\n" +
                        " using System.Collections.Generic;\n" +
                        " using System.Linq;\n" +
                        " using System.Text;\n" +
                        "+//test\n" +
                        " using System.Threading.Tasks;\n" +
                        " \n" +
                        " namespace GitApp";
            var diff = new Diff(diffText);
            diff.Current = 8;
            var fakeProcessRunner = new FakeGitProcessRunner();
            var patcher = new Patcher("", new GitManager("GitApp/GitApp/Hunk.cs", fakeProcessRunner), diff, true);
            var path = patcher.GetLinePatchPath(false, false, "GitApp/GitApp/Hunk.cs");
            var result = System.IO.File.ReadAllText(path);
            System.IO.File.Delete(path);
            Assert.Equal(diffText + "\n\n", result);
        }

        [Fact]
        public void GetLinePatchPathIsCalledCurrentElementIsRemovedShouldReturnPatchPath()
        {
            var diffText = "diff --git a/GitApp/GitApp/Hunk.cs b/GitApp/GitApp/Hunk.cs\n" +
                        "index c0fc9dc..d63a54b 100644\n" +
                        "--- a/GitApp/GitApp/Hunk.cs\n" +
                        "+++ b/GitApp/GitApp/Hunk.cs\n" +
                        "@@ -14,7 +14,6 @@ namespace GitApp\n" +
                        "         public Hunk(string rawData, string index)\n" +
                        "         {\n" +
                        "             this.rawData = rawData;\n" +
                        "-            this.index = index;\n" +
                        "         }\n" +
                        " \n" +
                        "         public string Id { get => index; }";
            var diff = new Diff(diffText);
            diff.Current = 8;
            var fakeProcessRunner = new FakeGitProcessRunner();
            var patcher = new Patcher("", new GitManager("GitApp/GitApp/Hunk.cs", fakeProcessRunner), diff, true);
            var path = patcher.GetLinePatchPath(false, false, "GitApp/GitApp/Hunk.cs");
            var result = System.IO.File.ReadAllText(path);
            System.IO.File.Delete(path);
            Assert.Equal(diffText + "\n\n", result);
        }

        [Fact]
        public void GetLinePatchPathIsCalledCurrentElementIsReplacedAndStartsWithMinusPreviousLinesAreLessThanThreeShouldReturnPatchPath()
        {
            var diffText = "diff --git a/GitApp/GitApp/Hunk.cs b/GitApp/GitApp/Hunk.cs\n" +
            "index c0fc9dc..d63a54b 100644\n" +
            "--- a/GitApp/GitApp/Hunk.cs\n" +
            "+++ b/GitApp/GitApp/Hunk.cs\n" +
            "@@ -1,4 +1,5 @@\n" +
            "-∩╗┐using System;\n" +
            "+∩╗┐//test\n" +
            "+using System;\n" +
            " using System.Collections.Generic;\n" +
            " using System.Linq;\n" +
            " using System.Text;";
            var diff = new Diff(diffText);
            diff.Current = 5;
            var fakeProcessRunner = new FakeGitProcessRunner();
            var patcher = new Patcher("", new GitManager("GitApp/GitApp/Hunk.cs", fakeProcessRunner), diff, true);
            var path = patcher.GetLinePatchPath(true, false, "GitApp/GitApp/Hunk.cs");
            var result = System.IO.File.ReadAllText(path);
            System.IO.File.Delete(path);
            var expected = "diff --git a/GitApp/GitApp/Hunk.cs b/GitApp/GitApp/Hunk.cs\n" +
                "index c0fc9dc..d63a54b 100644\n" +
                "--- a/GitApp/GitApp/Hunk.cs\n" +
                "+++ b/GitApp/GitApp/Hunk.cs\n" +
                "@@ -1,4 +1,4 @@\n" +
                "-﻿using System;\n" +
                "+﻿//test\n" +
                " using System.Collections.Generic;\n" +
                " using System.Linq;\n" +
                " using System.Text;\n\n";
            Assert.Equal(expected, result);
        }

        [Fact]
        public void GetLinePatchPathIsCalledCurrentElementIsReplacedAndStartsWithPlusPreviousLinesAreLessThanThreeShouldReturnPatchPath()
        {
            var diffText = "diff --git a/GitApp/GitApp/Hunk.cs b/GitApp/GitApp/Hunk.cs\n" +
            "index c0fc9dc..d63a54b 100644\n" +
            "--- a/GitApp/GitApp/Hunk.cs\n" +
            "+++ b/GitApp/GitApp/Hunk.cs\n" +
            "@@ -1,4 +1,5 @@\n" +
            "-∩╗┐using System;\n" +
            "+∩╗┐//test\n" +
            "+using System;\n" +
            " using System.Collections.Generic;\n" +
            " using System.Linq;\n" +
            " using System.Text;";
            var diff = new Diff(diffText);
            diff.Current = 6;
            var fakeProcessRunner = new FakeGitProcessRunner();
            var patcher = new Patcher("", new GitManager("GitApp/GitApp/Hunk.cs", fakeProcessRunner), diff, true);
            var path = patcher.GetLinePatchPath(true, false, "GitApp/GitApp/Hunk.cs");
            var result = System.IO.File.ReadAllText(path);
            System.IO.File.Delete(path);
            var expected = "diff --git a/GitApp/GitApp/Hunk.cs b/GitApp/GitApp/Hunk.cs\n" +
                "index c0fc9dc..d63a54b 100644\n" +
                "--- a/GitApp/GitApp/Hunk.cs\n" +
                "+++ b/GitApp/GitApp/Hunk.cs\n" +
                "@@ -1,4 +1,4 @@\n" +
                "-﻿using System;\n" +
                "+﻿//test\n" +
                " using System.Collections.Generic;\n" +
                " using System.Linq;\n" +
                " using System.Text;\n\n";
            Assert.Equal(expected, result);
        }

        [Fact]
        public void GetLinePatchPathIsCalledCurrentElementIsAddedPreviousLinesAreLessThanThreeShouldReturnPatchPath()
        {
            var diffText = "diff --git a/GitApp/GitApp/Hunk.cs b/GitApp/GitApp/Hunk.cs\n" +
                        "index c0fc9dc..d63a54b 100644\n" +
                        "--- a/GitApp/GitApp/Hunk.cs\n" +
                        "+++ b/GitApp/GitApp/Hunk.cs\n"+
                        "@@ -1,4 +1,5 @@\n" +
                        " ∩╗┐using System;\n" +
                        "+//test\n" +
                        " using System.Collections.Generic;\n" +
                        " using System.Linq;\n" +
                        " using System.Text;";
            var diff = new Diff(diffText);
            diff.Current = 6;
            var fakeProcessRunner = new FakeGitProcessRunner();
            var patcher = new Patcher("", new GitManager("GitApp/GitApp/Hunk.cs", fakeProcessRunner), diff, true);
            var path = patcher.GetLinePatchPath(false, false, "GitApp/GitApp/Hunk.cs");
            var result = System.IO.File.ReadAllText(path);
            System.IO.File.Delete(path);
            var expected = "diff --git a/GitApp/GitApp/Hunk.cs b/GitApp/GitApp/Hunk.cs\n" +
                        "index c0fc9dc..d63a54b 100644\n" +
                        "--- a/GitApp/GitApp/Hunk.cs\n" +
                        "+++ b/GitApp/GitApp/Hunk.cs\n" +
                        "@@ -1,4 +1,5 @@\n" +
                        " using System;\n" +
                        "+//test\n" +
                        " using System.Collections.Generic;\n" +
                        " using System.Linq;\n" +
                        " using System.Text;\n\n";
            Assert.Equal(expected, result);
        }

        [Fact]
        public void GetLinePatchPathIsCalledCurrentElementIsRemovedPreviousLinesAreLessThanThreeShouldReturnPatchPath()
        {
            var diffText = "diff --git a/GitApp/GitApp/Hunk.cs b/GitApp/GitApp/Hunk.cs\n" +
                        "index c0fc9dc..d63a54b 100644\n" +
                        "--- a/GitApp/GitApp/Hunk.cs\n" +
                        "+++ b/GitApp/GitApp/Hunk.cs\n" +
                        "@@ -1,5 +1,4 @@\n" +
                        " ∩╗┐using System;\n" +
                        "-using System.Collections.Generic;\n" +
                        " using System.Linq;\n" +
                        " using System.Text;\n" +
                        " using System.Threading.Tasks;";
            var diff = new Diff(diffText);
            diff.Current = 6;
            var fakeProcessRunner = new FakeGitProcessRunner();
            var patcher = new Patcher("", new GitManager("GitApp/GitApp/Hunk.cs", fakeProcessRunner), diff, true);
            var path = patcher.GetLinePatchPath(false, false, "GitApp/GitApp/Hunk.cs");
            var result = System.IO.File.ReadAllText(path);
            System.IO.File.Delete(path);
            var expected = "diff --git a/GitApp/GitApp/Hunk.cs b/GitApp/GitApp/Hunk.cs\n" +
                        "index c0fc9dc..d63a54b 100644\n" +
                        "--- a/GitApp/GitApp/Hunk.cs\n" +
                        "+++ b/GitApp/GitApp/Hunk.cs\n" +
                        "@@ -1,5 +1,4 @@\n" +
                        " using System;\n" +
                        "-using System.Collections.Generic;\n" +
                        " using System.Linq;\n" +
                        " using System.Text;\n" +
                        " using System.Threading.Tasks;\n\n";
            Assert.Equal(expected, result);
        }

        [Fact]
        public void GetLinePatchPathIsCalledCurrentElementIsReplacedAndStartsWithMinusEndLinesAreLessThanThreeShouldReturnPatchPath()
        {
            var diffText = "diff --git a/GitApp/GitApp/Hunk.cs b/GitApp/GitApp/Hunk.cs\n" +
                        "index c0fc9dc..d63a54b 100644\n" +
                        "--- a/GitApp/GitApp/Hunk.cs\n" +
                        "+++ b/GitApp/GitApp/Hunk.cs\n" +
                        "@@ -24,4 +24,4 @@ namespace GitApp\n" +
                        "             return rawData;\n" +
                        "         }\n" +
                        "     }\n" +
                        "-}\n" +
                        "+}//test";
            var diff = new Diff(diffText);
            diff.Current = 8;
            var fakeProcessRunner = new FakeGitProcessRunner();
            var patcher = new Patcher("", new GitManager("GitApp/GitApp/Hunk.cs", fakeProcessRunner), diff, true);
            var path = patcher.GetLinePatchPath(true, false, "GitApp/GitApp/Hunk.cs");
            var result = System.IO.File.ReadAllText(path);
            System.IO.File.Delete(path);
            var temp = result.Split(diffText);
            Assert.Equal(diffText + "\n\n", result);
        }

        [Fact]
        public void GetLinePatchPathIsCalledCurrentElementIsReplacedAndStartsWithPlusEndLinesAreLessThanThreeShouldReturnPatchPath()
        {
            var diffText = "diff --git a/GitApp/GitApp/Hunk.cs b/GitApp/GitApp/Hunk.cs\n" +
                        "index c0fc9dc..d63a54b 100644\n" +
                        "--- a/GitApp/GitApp/Hunk.cs\n" +
                        "+++ b/GitApp/GitApp/Hunk.cs\n" +
                        "@@ -24,4 +24,4 @@ namespace GitApp\n" +
                        "             return rawData;\n" +
                        "         }\n" +
                        "     }\n" +
                        "-}\n" +
                        "+}//test";
            var diff = new Diff(diffText);
            diff.Current = 9;
            var fakeProcessRunner = new FakeGitProcessRunner();
            var patcher = new Patcher("", new GitManager("GitApp/GitApp/Hunk.cs", fakeProcessRunner), diff, true);
            var path = patcher.GetLinePatchPath(true, false, "GitApp/GitApp/Hunk.cs");
            var result = System.IO.File.ReadAllText(path);
            System.IO.File.Delete(path);
            var temp = result.Split(diffText);
            Assert.Equal(diffText + "\n\n", result);
        }

        [Fact]
        public void GetLinePatchPathIsCalledCurrentElementIsAddedEndLinesAreLessThanThreeShouldReturnPatchPath()
        {
            var diffText = "diff --git a/GitApp/GitApp/Hunk.cs b/GitApp/GitApp/Hunk.cs\n" +
                        "index c0fc9dc..d63a54b 100644\n" +
                        "--- a/GitApp/GitApp/Hunk.cs\n" +
                        "+++ b/GitApp/GitApp/Hunk.cs\n" +
                        "@@ -24,4 +24,5 @@ namespace GitApp\n" +
                        "             return rawData;\n" +
                        "         }\n" +
                        "     }\n" +
                        "+//test\n" +
                        " }" ;
            var diff = new Diff(diffText);
            diff.Current = 8;
            var fakeProcessRunner = new FakeGitProcessRunner();
            var patcher = new Patcher("", new GitManager("GitApp/GitApp/Hunk.cs", fakeProcessRunner), diff, true);
            var path = patcher.GetLinePatchPath(false, false, "GitApp/GitApp/Hunk.cs");
            var result = System.IO.File.ReadAllText(path);
            System.IO.File.Delete(path);
            Assert.Equal(diffText + "\n\n", result);
        }

        [Fact]
        public void GetLinePatchPathIsCalledCurrentElementIsRemovedEndLinesAreLessThanThreeShouldReturnPatchPath()
        {
            var diffText = "diff --git a/GitApp/GitApp/Hunk.cs b/GitApp/GitApp/Hunk.cs\n" +
                        "index c0fc9dc..d63a54b 100644\n" +
                        "--- a/GitApp/GitApp/Hunk.cs\n" +
                        "+++ b/GitApp/GitApp/Hunk.cs\n" +
                        "@@ -24,4 +24,3 @@ namespace GitApp\n" +
                        "             return rawData;\n" +
                        "         }\n" +
                        "     }\n" +
                        "-}";
            var diff = new Diff(diffText);
            diff.Current = 8;
            var fakeProcessRunner = new FakeGitProcessRunner();
            var patcher = new Patcher("", new GitManager("GitApp/GitApp/Hunk.cs", fakeProcessRunner), diff, true);
            var path = patcher.GetLinePatchPath(false, false, "GitApp/GitApp/Hunk.cs");
            var result = System.IO.File.ReadAllText(path);
            System.IO.File.Delete(path);
            Assert.Equal(diffText + "\n\n", result);
        }

        [Fact]
        public void GetLinePatchPathIsCalledCurrentElementIsReplacedAndStartsWithMinusHunkContainsPreviouslyAddedLineShouldReturnPatchPath()
        {
            var diffText = "diff --git a/GitApp/GitApp/Hunk.cs b/GitApp/GitApp/Hunk.cs\n" +
            "index c0fc9dc..d63a54b 100644\n" +
            "--- a/GitApp/GitApp/Hunk.cs\n" +
            "+++ b/GitApp/GitApp/Hunk.cs\n" +
            "@@ -7,9 +7,10 @@ using System.Threading.Tasks;\n" +
            " namespace GitApp\n" +
            " {\n" +
            "     public class Hunk : IElement\n" +
            "+    //test\n" +
            "     {\n" +
            "         private readonly string rawData;\n" +
            "-        private readonly string index;\n" +
            "+        private readonly string index;//test\n" +
            " \n" +
            "         public Hunk(string rawData, string index)\n" +
            "         {";
            var diff = new Diff(diffText);
            diff.Current = 11;
            var fakeProcessRunner = new FakeGitProcessRunner();
            var patcher = new Patcher("", new GitManager("GitApp/GitApp/Hunk.cs", fakeProcessRunner), diff, true);
            var path = patcher.GetLinePatchPath(true, false, "GitApp/GitApp/Hunk.cs");
            var result = System.IO.File.ReadAllText(path);
            const string expected = "diff --git a/GitApp/GitApp/Hunk.cs b/GitApp/GitApp/Hunk.cs\n" +
                                "index c0fc9dc..d63a54b 100644\n" +
                                "--- a/GitApp/GitApp/Hunk.cs\n" +
                                "+++ b/GitApp/GitApp/Hunk.cs\n" +
                                "@@ -9,7 +9,7 @@ using System.Threading.Tasks;\n" +
                                "     public class Hunk : IElement\n" +
                                "     {\n" +
                                "         private readonly string rawData;\n" +
                                "-        private readonly string index;\n" +
                                "+        private readonly string index;//test\n" +
                                " \n" +
                                "         public Hunk(string rawData, string index)\n" +
                                "         {\n\n";
            System.IO.File.Delete(path);
            Assert.Equal(expected, result);
        }

        [Fact]
        public void GetLinePatchPathIsCalledCurrentElementIsReplacedAndStartsWithPlusHunkContainsPreviouslyAddedLineShouldReturnPatchPath()
        {
            var diffText = "diff --git a/GitApp/GitApp/Hunk.cs b/GitApp/GitApp/Hunk.cs\n" +
            "index c0fc9dc..d63a54b 100644\n" +
            "--- a/GitApp/GitApp/Hunk.cs\n" +
            "+++ b/GitApp/GitApp/Hunk.cs\n" +
            "@@ -7,9 +7,10 @@ using System.Threading.Tasks;\n" +
            " namespace GitApp\n" +
            " {\n" +
            "     public class Hunk : IElement\n" +
            "+    //test\n" +
            "     {\n" +
            "         private readonly string rawData;\n" +
            "-        private readonly string index;\n" +
            "+        private readonly string index;//test\n" +
            " \n" +
            "         public Hunk(string rawData, string index)\n" +
            "         {";
            var diff = new Diff(diffText);
            diff.Current = 12;
            var fakeProcessRunner = new FakeGitProcessRunner();
            var patcher = new Patcher("", new GitManager("GitApp/GitApp/Hunk.cs", fakeProcessRunner), diff, true);
            var path = patcher.GetLinePatchPath(true, false, "GitApp/GitApp/Hunk.cs");
            var result = System.IO.File.ReadAllText(path);
            const string expected = "diff --git a/GitApp/GitApp/Hunk.cs b/GitApp/GitApp/Hunk.cs\n" +
                                "index c0fc9dc..d63a54b 100644\n" +
                                "--- a/GitApp/GitApp/Hunk.cs\n" +
                                "+++ b/GitApp/GitApp/Hunk.cs\n" +
                                "@@ -9,7 +9,7 @@ using System.Threading.Tasks;\n" +
                                "     public class Hunk : IElement\n" +
                                "     {\n" +
                                "         private readonly string rawData;\n" +
                                "-        private readonly string index;\n" +
                                "+        private readonly string index;//test\n" +
                                " \n" +
                                "         public Hunk(string rawData, string index)\n" +
                                "         {\n\n";
            System.IO.File.Delete(path);
            Assert.Equal(expected, result);
        }

        [Fact]
        public void GetLinePatchPathIsCalledCurrentElementIsAddedHunkContainsPreviouslyAddedLineShouldReturnPatchPath()
        {
            var diffText = "diff --git a/GitApp/GitApp/Hunk.cs b/GitApp/GitApp/Hunk.cs\n" +
            "index c0fc9dc..d63a54b 100644\n" +
            "--- a/GitApp/GitApp/Hunk.cs\n" +
            "+++ b/GitApp/GitApp/Hunk.cs\n" +
            "@@ -7,8 +7,10 @@ using System.Threading.Tasks;\n" +
            " namespace GitApp\n" +
            " {\n" +
            "     public class Hunk : IElement\n" +
            "+    //test\n" +
            "     {\n" +
            "         private readonly string rawData;\n" +
            "+        //test\n" +
            "         private readonly string index;\n" +
            " \n" +
            "         public Hunk(string rawData, string index)";
            var diff = new Diff(diffText);
            diff.Current = 11;
            var fakeProcessRunner = new FakeGitProcessRunner();
            var patcher = new Patcher("", new GitManager("GitApp/GitApp/Hunk.cs", fakeProcessRunner), diff, true);
            var path = patcher.GetLinePatchPath(false, false, "GitApp/GitApp/Hunk.cs");
            var result = System.IO.File.ReadAllText(path);
            const string expected = "diff --git a/GitApp/GitApp/Hunk.cs b/GitApp/GitApp/Hunk.cs\n" +
                                "index c0fc9dc..d63a54b 100644\n" +
                                "--- a/GitApp/GitApp/Hunk.cs\n" +
                                "+++ b/GitApp/GitApp/Hunk.cs\n" +
                                "@@ -9,6 +9,7 @@ using System.Threading.Tasks;\n" +
                                "     public class Hunk : IElement\n" +
                                "     {\n" +
                                "         private readonly string rawData;\n"+
                                "+        //test\n" +
                                "         private readonly string index;\n" +
                                " \n" +
                                "         public Hunk(string rawData, string index)\n\n";
            System.IO.File.Delete(path);
            Assert.Equal(expected, result);
        }

        [Fact]
        public void GetLinePatchPathIsCalledCurrentElementIsRemovedHunkContainsPreviouslyAddedLineShouldReturnPatchPath()
        {
            var diffText = "diff --git a/GitApp/GitApp/Hunk.cs b/GitApp/GitApp/Hunk.cs\n" +
            "index c0fc9dc..d63a54b 100644\n" +
            "--- a/GitApp/GitApp/Hunk.cs\n" +
            "+++ b/GitApp/GitApp/Hunk.cs\n" +
            "@@ -7,10 +7,10 @@ using System.Threading.Tasks;\n" +
            " namespace GitApp\n" +
            " {\n" +
            "     public class Hunk : IElement\n" +
            "+    //test\n" +
            "     {\n" +
            "         private readonly string rawData;\n" +
            "         private readonly string index;\n" +
            "-\n" +
            "         public Hunk(string rawData, string index)\n" +
            "         {\n" +
            "             this.rawData = rawData;";
            var diff = new Diff(diffText);
            diff.Current = 12;
            var fakeProcessRunner = new FakeGitProcessRunner();
            var patcher = new Patcher("", new GitManager("GitApp/GitApp/Hunk.cs", fakeProcessRunner), diff, true);
            var path = patcher.GetLinePatchPath(false, false, "GitApp/GitApp/Hunk.cs");
            var result = System.IO.File.ReadAllText(path);
            const string expected = "diff --git a/GitApp/GitApp/Hunk.cs b/GitApp/GitApp/Hunk.cs\n" +
            "index c0fc9dc..d63a54b 100644\n" +
            "--- a/GitApp/GitApp/Hunk.cs\n" +
            "+++ b/GitApp/GitApp/Hunk.cs\n" +
            "@@ -10,7 +10,6 @@ using System.Threading.Tasks;\n" +
            "     {\n" +
            "         private readonly string rawData;\n" +
            "         private readonly string index;\n" +
            "-\n" +
            "         public Hunk(string rawData, string index)\n" +
            "         {\n" +
            "             this.rawData = rawData;\n\n";
            System.IO.File.Delete(path);
            Assert.Equal(expected, result);
        }

        [Fact]
        public void GetLinePatchPathIsCalledCurrentElementIsReplacedAndStartsWithMinusHunkContainsPreviouslyRemovedLineShouldReturnPatchPath()
        {
            var diffText = "diff --git a/GitApp/GitApp/Hunk.cs b/GitApp/GitApp/Hunk.cs\n" +
            "index c0fc9dc..d63a54b 100644\n" +
            "--- a/GitApp/GitApp/Hunk.cs\n" +
            "+++ b/GitApp/GitApp/Hunk.cs\n" +
            "@@ -7,10 +7,10 @@ using System.Threading.Tasks;\n" +
            " namespace GitApp\n" +
            " {\n" +
            "     public class Hunk : IElement\n" +
            "+    //test\n" +
            "     {\n" +
            "         private readonly string rawData;\n" +
            "         private readonly string index;\n" +
            "-\n" +
            "         public Hunk(string rawData, string index)\n" +
            "         {\n" +
            "             this.rawData = rawData;";
            var diff = new Diff(diffText);
            diff.Current = 12;
            var fakeProcessRunner = new FakeGitProcessRunner();
            var patcher = new Patcher("", new GitManager("GitApp/GitApp/Hunk.cs", fakeProcessRunner), diff, true);
            var path = patcher.GetLinePatchPath(false, false, "GitApp/GitApp/Hunk.cs");
            var result = System.IO.File.ReadAllText(path);
            const string expected = "diff --git a/GitApp/GitApp/Hunk.cs b/GitApp/GitApp/Hunk.cs\n" +
            "index c0fc9dc..d63a54b 100644\n" +
            "--- a/GitApp/GitApp/Hunk.cs\n" +
            "+++ b/GitApp/GitApp/Hunk.cs\n" +
            "@@ -10,7 +10,6 @@ using System.Threading.Tasks;\n" +
            "     {\n" +
            "         private readonly string rawData;\n" +
            "         private readonly string index;\n" +
            "-\n" +
            "         public Hunk(string rawData, string index)\n" +
            "         {\n" +
            "             this.rawData = rawData;\n\n";
            System.IO.File.Delete(path);
            Assert.Equal(expected, result);
        }

        [Fact]
        public void GetLinePatchPathIsCalledFileIsNewlyAddedShouldReturnPatchPath()
        {
            var diffText = "diff --git a/GitApp/GitApp/TestClass.cs b/GitApp/GitApp/TestClass.cs\n" +
                "new file mode 100644\n" +
                "index 0000000..77eb46a\n" +
                "--- /dev/null\n" +
                "+++ b/GitApp/GitApp/TestClass.cs\n" +
                "@@ -0,0 +1,12 @@\n" +
                "+∩╗┐using System;\n" +
                "+using System.Collections.Generic;\n" +
                "+using System.Linq;\n" +
                "+using System.Text;\n" +
                "+using System.Threading.Tasks;\n" +
                "+\n" +
                "+namespace GitApp\n" +
                "+{\n" +
                "+    internal class TestClass\n" +
                "+    {\n" +
                "+    }\n" +
                "+}";
            var diff = new Diff(diffText);
            diff.Current = 9;
            var fakeProcessRunner = new FakeGitProcessRunner();
            var patcher = new Patcher("", new GitManager("GitApp/GitApp/Hunk.cs", fakeProcessRunner), diff, true);
            var path = patcher.GetLinePatchPath(false, false, "GitApp/GitApp/Hunk.cs");
            var result = System.IO.File.ReadAllText(path);
            const string expected = "diff --git a/GitApp/GitApp/TestClass.cs b/GitApp/GitApp/TestClass.cs\n" +
                "new file mode 100644\n" +
                "index 0000000..77eb46a\n" +
                "--- /dev/null\n" +
                "+++ b/GitApp/GitApp/TestClass.cs\n" +
                "@@ -0,0 +0,1 @@\n" +
                "+using System.Text;\n\n";
            System.IO.File.Delete(path);
            Assert.Equal(expected, result);
        }

        [Fact]
        public void GetLinePatchPathIsCalledHeaderContainsThreeNumbersCurrentIsBeforeAddedLineShouldReturnPatchPath()
        {
            var diffText = "diff --git a/GitApp/GitApp/TestClass.cs b/GitApp/GitApp/TestClass.cs\n" +
                "new file mode 100644\n" +
                "index 0000000..77eb46a\n" +
                "--- /dev/null\n" +
                "+++ b/GitApp/GitApp/TestClass.cs\n" +
                "@@ -1 +1,12 @@\n" +
                "+∩╗┐using System;\n" +
                "+using System.Collections.Generic;\n" +
                "+using System.Linq;\n" +
                " using System.Text;\n" +
                "+using System.Threading.Tasks;\n" +
                "+\n" +
                "+namespace GitApp\n" +
                "+{\n" +
                "+    internal class TestClass\n" +
                "+    {\n" +
                "+    }\n" +
                "+}";
            var diff = new Diff(diffText);
            diff.Current = 8;
            var fakeProcessRunner = new FakeGitProcessRunner();
            var patcher = new Patcher("", new GitManager("GitApp/GitApp/Hunk.cs", fakeProcessRunner), diff, true);
            var path = patcher.GetLinePatchPath(false, false, "GitApp/GitApp/Hunk.cs");
            var result = System.IO.File.ReadAllText(path);
            const string expected = "diff --git a/GitApp/GitApp/TestClass.cs b/GitApp/GitApp/TestClass.cs\n" +
                "new file mode 100644\n" +
                "index 0000000..77eb46a\n" +
                "--- /dev/null\n" +
                "+++ b/GitApp/GitApp/TestClass.cs\n" +
                "@@ -1 +1,2 @@\n" +
                "+using System.Linq;\n" +
                " using System;\n\n";
            System.IO.File.Delete(path);
            Assert.Equal(expected, result);
        }

        [Fact]
        public void GetLinePatchPathIsCalledHeaderContainsThreeNumbersCurrentIsAfterAddedLineShouldReturnPatchPath()
        {
            var diffText = "diff --git a/GitApp/GitApp/TestClass.cs b/GitApp/GitApp/TestClass.cs\n" +
                "new file mode 100644\n" +
                "index 0000000..77eb46a\n" +
                "--- /dev/null\n" +
                "+++ b/GitApp/GitApp/TestClass.cs\n" +
                "@@ -1 +1,12 @@\n" +
                "+∩╗┐using System;\n" +
                "+using System.Collections.Generic;\n" +
                "+using System.Linq;\n" +
                " using System.Text;\n" +
                "+using System.Threading.Tasks;\n" +
                "+\n" +
                "+namespace GitApp\n" +
                "+{\n" +
                "+    internal class TestClass\n" +
                "+    {\n" +
                "+    }\n" +
                "+}";
            var diff = new Diff(diffText);
            diff.Current = 10;
            var fakeProcessRunner = new FakeGitProcessRunner();
            var patcher = new Patcher("", new GitManager("GitApp/GitApp/Hunk.cs", fakeProcessRunner), diff, true);
            var path = patcher.GetLinePatchPath(false, false, "GitApp/GitApp/Hunk.cs");
            var result = System.IO.File.ReadAllText(path);
            const string expected = "diff --git a/GitApp/GitApp/TestClass.cs b/GitApp/GitApp/TestClass.cs\n" +
                "new file mode 100644\n" +
                "index 0000000..77eb46a\n" +
                "--- /dev/null\n" +
                "+++ b/GitApp/GitApp/TestClass.cs\n" +
                "@@ -1 +1,2 @@\n" +
                " using System;\n" +
                "+using System.Threading.Tasks;\n\n";
            System.IO.File.Delete(path);
            Assert.Equal(expected, result);
        }

        [Fact]
        public void GetHunkPatchPathIsCalledShouldReturnPatchPath()
        {
            var diffText = "diff --git a/GitApp/GitApp/TestClass.cs b/GitApp/GitApp/TestClass.cs\n" +
                    "new file mode 100644\n" +
                    "index 0000000..77eb46a\n" +
                    "--- /dev/null\n" +
                    "+++ b/GitApp/GitApp/TestClass.cs\n" +
                    "@@ -1 +1,12 @@\n" +
                    "+using System;\n" +
                    "+using System.Collections.Generic;\n" +
                    "+using System.Linq;\n" +
                    " using System.Text;\n" +
                    "+using System.Threading.Tasks;\n" +
                    "+\n" +
                    "+namespace GitApp\n" +
                    "+{\n" +
                    "+    internal class TestClass\n" +
                    "+    {\n" +
                    "+    }\n" +
                    "+}";
            var diff = new Diff(diffText);
            diff.Current = 10;
            var fakeProcessRunner = new FakeGitProcessRunner();
            var patcher = new Patcher("", new GitManager("GitApp/GitApp/Hunk.cs", fakeProcessRunner), diff, true);
            var path = patcher.GetHunkPatchPath();
            var result = System.IO.File.ReadAllText(path);
            System.IO.File.Delete(path);
            Assert.Equal(diffText + "\n\n", result);
        }
    }
}
