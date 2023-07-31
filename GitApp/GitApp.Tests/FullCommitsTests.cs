using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Sdk;

namespace GitApp.Tests
{
    public class FullCommitsTests
    {
        const string Commits = "commit 7f37022a20e43184a99dc063be55a27ff5afb34d (HEAD -> master)" +
                "\nAuthor: Andrei Daboc <99188105+AndreiDbc@users.noreply.github.com>" +
                "\nDate:   Wed May 17 20:46:13 2023 +0300" +
                "\n\n    Removed temp variable from FitTextInsideBox method." +
                "\n\ncommit bfa2684c13f78acf0468d2b9d459ff1021327d18" +
                "\nAuthor: Andrei Daboc <99188105+AndreiDbc@users.noreply.github.com>" +
                "\nDate:   Wed May 17 20:09:35 2023 +0300" +
                "\n\n    Fixed an issue that caused TruncateText method to be called when not necessary." +
                "\n\ncommit 1db76a24c52ba7c952fa9e47e57e77fcfbedd3dd" +
                "\nAuthor: Andrei Daboc <99188105+AndreiDbc@users.noreply.github.com>" +
                "\nDate:   Wed May 17 12:04:21 2023 +0300" +
                "\n\n    Data is now displayed on the whole console window." +
                "\n\ncommit adfdf0eb554c0e40ce13bb8c5a9b4612a62c6045 (origin/master, origin/HEAD)" +
                "\nAuthor: Andrei Daboc <99188105+AndreiDbc@users.noreply.github.com>" +
                "\nDate:   Fri May 12 12:32:46 2023 +0300" +
                "\n\n    Removed FormateRightSide's \"limit\" parameter." +
                "\n\ncommit f944893636d43d1bf1468c41202ce194bf8d25e3" +
                "\nAuthor: Andrei Daboc <99188105+AndreiDbc@users.noreply.github.com>" +
                "\nDate:   Fri May 12 12:27:52 2023 +0300" +
                "\n\n    Boxes are now properly displayed at any size." +
                "\n\ncommit 92e239d63095136fc2811956043938ddb3a1e4fe" +
                "\nAuthor: Andrei Daboc <99188105+AndreiDbc@users.noreply.github.com>" +
                "\nDate:   Tue May 9 19:49:54 2023 +0300" +
                "\n\n    FitTextInsideBox method's loop now breaks if \"i\" is equal to current line element's length minus one." +
                "\n\ncommit 10580c965b789e1ef4c1d588530903f7b82e9136" +
                "\nAuthor: Andrei Daboc <99188105+AndreiDbc@users.noreply.github.com>" +
                "\nDate:   Tue May 9 19:48:11 2023 +0300" +
                "\n\n    Modified functionality of DisplaySplitScreen method." +
                "\n\ncommit fae78e6f1f66c2a85d89871dc053bc90c269ddbd" +
                "\nAuthor: Andrei Daboc <99188105+AndreiDbc@users.noreply.github.com>" +
                "\nDate:   Tue May 9 13:34:45 2023 +0300" +
                "\n\n    Fixed date column arrangement on left side." +
                "\n\ncommit f4ad2943cab64ca72b7ab5fa194a81183509b7bf" +
                "\nAuthor: Andrei Daboc <99188105+AndreiDbc@users.noreply.github.com>" +
                "\nDate:   Tue May 9 13:25:32 2023 +0300" +
                "\n\n    GetLeftSide method's leftSideColumnSize variable is now 15% of console window's width." +
                "\n\ncommit d3d8ef77607f81818f50e846a49b05623ab8fa68" +
                "\nAuthor: Andrei Daboc <99188105+AndreiDbc@users.noreply.github.com>" +
                "\nDate:   Mon May 8 14:37:15 2023 +0300" +
                "\n\n    Fixed an issue that caused not to display the correct date.\r\n";
        [Fact]
        public void GetTextIsCalledShouldReturnCommits()
        {
            const string input = "commit 7f37022a20e43184a99dc063be55a27ff5afb34d (HEAD -> master)" +
                "\nAuthor: Andrei Daboc <99188105+AndreiDbc@users.noreply.github.com>" +
                "\nDate:   Wed May 17 20:46:13 2023 +0300" +
                "\n\n    Removed temp variable from FitTextInsideBox method." +
                "\n\ncommit bfa2684c13f78acf0468d2b9d459ff1021327d18" +
                "\nAuthor: Andrei Daboc <99188105+AndreiDbc@users.noreply.github.com>" +
                "\nDate:   Wed May 17 20:09:35 2023 +0300" +
                "\n\n    Fixed an issue that caused TruncateText method to be called when not necessary.";
            var fullCommits = new FullCommits(input);
            const string expected1 = "7f37022\nAndrei Daboc\n2023-05-17\nRemoved temp variable from FitTextInsideBox method.\n7f37022a20e43184a99dc063be55a27ff5afb34d";
            const string expected2 = "bfa2684\nAndrei Daboc\n2023-05-17\nFixed an issue that caused TruncateText method to be called when not necessary.\nbfa2684c13f78acf0468d2b9d459ff1021327d18";
            Assert.Equal(expected1, fullCommits.GetText()[0].GetElement());
            Assert.Equal(expected2, fullCommits.GetText()[1].GetElement());
        }

        [Fact]
        public void GetHighlitedSideIsCalledShouldReturnText()
        {
            var fullCommits = new FullCommits(Commits);
            var expected = new List<string>()
            {
                "\u001b[7m\u001b[48;5;27m7f37022 \u001b[48;5;46mAndrei Daboc \u001b[48;5;196m2023-05-17 \u001b[0m\u001b[7mRemoved temp variable from FitTextInsideBox method.",
                "\u001b[0m\u001b[38;5;27mbfa2684 \u001b[38;5;46mAndrei Daboc \u001b[38;5;196m2023-05-17 \u001b[0mFixed an issue that caused TruncateText method to be called when not necessary.",
                "\u001b[0m\u001b[38;5;27m1db76a2 \u001b[38;5;46mAndrei Daboc \u001b[38;5;196m2023-05-17 \u001b[0mData is now displayed on the whole console window.",
                "\u001b[0m\u001b[38;5;27madfdf0e \u001b[38;5;46mAndrei Daboc \u001b[38;5;196m2023-05-12 \u001b[0mRemoved FormateRightSide's \"limit\" parameter.",
                "\u001b[0m\u001b[38;5;27mf944893 \u001b[38;5;46mAndrei Daboc \u001b[38;5;196m2023-05-12 \u001b[0mBoxes are now properly displayed at any size.",
                "\u001b[0m\u001b[38;5;27m92e239d \u001b[38;5;46mAndrei Daboc \u001b[38;5;196m2023-05-09 \u001b[0mFitTextInsideBox method's loop now breaks if \"i\" is equal to current line element's length minus one.",
                "\u001b[0m\u001b[38;5;27m10580c9 \u001b[38;5;46mAndrei Daboc \u001b[38;5;196m2023-05-09 \u001b[0mModified functionality of DisplaySplitScreen method.",
                "\u001b[0m\u001b[38;5;27mfae78e6 \u001b[38;5;46mAndrei Daboc \u001b[38;5;196m2023-05-09 \u001b[0mFixed date column arrangement on left side.",
                "\u001b[0m\u001b[38;5;27mf4ad294 \u001b[38;5;46mAndrei Daboc \u001b[38;5;196m2023-05-09 \u001b[0mGetLeftSide method's leftSideColumnSize variable is now 15% of console window's width.",
                "\u001b[0m\u001b[38;5;27md3d8ef7 \u001b[38;5;46mAndrei Daboc \u001b[38;5;196m2023-05-08 \u001b[0mFixed an issue that caused not to display the correct date."
            };
            var output = fullCommits.GetHighlightedSide(25, 120, 3000);
            Assert.Equal(expected, output);
        }

        [Fact]
        public void GetHighlightedSideIsCalledScreenIsSplitShouldReturnText()
        {
            var fullCommits = new FullCommits(Commits);
            var expected = new List<string>()
            {
                "\u001b[7m\u001b[48;5;27m7f37022 \u001b[48;5;46mAndrei Daboc \u001b[48;5;196m2023-05-17 \u001b[0m\u001b[7mRemoved temp variable from FitTextI...",
                "\u001b[0m\u001b[38;5;27mbfa2684 \u001b[38;5;46mAndrei Daboc \u001b[38;5;196m2023-05-17 \u001b[0mFixed an issue that caused Truncate...",
                "\u001b[0m\u001b[38;5;27m1db76a2 \u001b[38;5;46mAndrei Daboc \u001b[38;5;196m2023-05-17 \u001b[0mData is now displayed on the whole ...",
                "\u001b[0m\u001b[38;5;27madfdf0e \u001b[38;5;46mAndrei Daboc \u001b[38;5;196m2023-05-12 \u001b[0mRemoved FormateRightSide's \"limit\" ...",
                "\u001b[0m\u001b[38;5;27mf944893 \u001b[38;5;46mAndrei Daboc \u001b[38;5;196m2023-05-12 \u001b[0mBoxes are now properly displayed at...",
                "\u001b[0m\u001b[38;5;27m92e239d \u001b[38;5;46mAndrei Daboc \u001b[38;5;196m2023-05-09 \u001b[0mFitTextInsideBox method's loop now ...",
                "\u001b[0m\u001b[38;5;27m10580c9 \u001b[38;5;46mAndrei Daboc \u001b[38;5;196m2023-05-09 \u001b[0mModified functionality of DisplaySp...",
                "\u001b[0m\u001b[38;5;27mfae78e6 \u001b[38;5;46mAndrei Daboc \u001b[38;5;196m2023-05-09 \u001b[0mFixed date column arrangement on le...",
                "\u001b[0m\u001b[38;5;27mf4ad294 \u001b[38;5;46mAndrei Daboc \u001b[38;5;196m2023-05-09 \u001b[0mGetLeftSide method's leftSideColumn...",
                "\u001b[0m\u001b[38;5;27md3d8ef7 \u001b[38;5;46mAndrei Daboc \u001b[38;5;196m2023-05-08 \u001b[0mFixed an issue that caused not to d..."
            };
            var output = fullCommits.GetHighlightedSide(25, 70, 1960, true);
            Assert.Equal(expected, output);
        }

        [Fact]
        public void GetPageDataAndSizeIsCalledShouldReturnTextAndSize()
        {
            var fullCommits = new FullCommits(Commits);
            var output = fullCommits.GetPageDataAndSize(3000, 120);
            Assert.Equal("7f37022", output.list[0].Id);
            Assert.Equal("bfa2684", output.list[1].Id);
            Assert.Equal("1db76a2", output.list[2].Id);
            Assert.Equal("adfdf0e", output.list[3].Id);
            Assert.Equal("f944893", output.list[4].Id);
            Assert.Equal("92e239d", output.list[5].Id);
            Assert.Equal("10580c9", output.list[6].Id);
            Assert.Equal("fae78e6", output.list[7].Id);
            Assert.Equal("f4ad294", output.list[8].Id);
            Assert.Equal("d3d8ef7", output.list[9].Id);
            Assert.Equal(1320, output.textSize);
        }

        [Fact]
        public void GetPageDataAndSizeIsCalledScreenIsSplitShouldReturnTextAndSize()
        {
            var fullCommits = new FullCommits(Commits);
            var output = fullCommits.GetPageDataAndSize(3000, 120, true);
            Assert.Equal("7f37022", output.list[0].Id);
            Assert.Equal("bfa2684", output.list[1].Id);
            Assert.Equal("1db76a2", output.list[2].Id);
            Assert.Equal("adfdf0e", output.list[3].Id);
            Assert.Equal("f944893", output.list[4].Id);
            Assert.Equal("92e239d", output.list[5].Id);
            Assert.Equal("10580c9", output.list[6].Id);
            Assert.Equal("fae78e6", output.list[7].Id);
            Assert.Equal("f4ad294", output.list[8].Id);
            Assert.Equal("d3d8ef7", output.list[9].Id);
            Assert.Equal(1200, output.textSize);
        }

        [Fact]
        public void FormateUnhighlightedSideIsCalledShouldReturnBoxes()
        {
            var fullCommits = new FullCommits(Commits);
            var output = fullCommits.FormateUnhighlightedSide(30, 48, new GitManager("git", new FakeGitProcessRunner()));
            var expected = new List<string>
            {
                "\u001b[0m┌Info──────────────────────────────────────────┐",
                "\u001b[0m│\u001b[0mAuthor: Andrei Daboc\u001b[0m\u001b[0m                          │",
                "\u001b[0m│\u001b[0mDate: 2023-05-17\u001b[0m\u001b[0m                              │",
                "\u001b[0m│\u001b[0mSHA: 7f37022a20e43184a99dc063be55a27ff5afb34d\u001b[0m\u001b[0m │",
                "\u001b[0m│                                              │",
                "\u001b[0m│                                              │",
                "\u001b[0m│                                              │",
                "\u001b[0m│                                              │",
                "\u001b[0m│                                              │",
                "\u001b[0m└──────────────────────────────────────────────┘",
                "\u001b[0m┌Message───────────────────────────────────────┐",
                "\u001b[0m│\u001b[0mRemoved temp variable from FitTextInsideBox me\u001b[0m\u001b[0m│",
                "\u001b[0m│\u001b[0mthod.\u001b[0m\u001b[0m                                         │",
                "\u001b[0m│                                              │",
                "\u001b[0m│                                              │",
                "\u001b[0m│                                              │",
                "\u001b[0m│                                              │",
                "\u001b[0m│                                              │",
                "\u001b[0m│                                              │",
                "\u001b[0m└──────────────────────────────────────────────┘",
                "\u001b[0m┌Files─────────────────────────────────────────┐",
                "\u001b[0m│\u001b[0mGitApp/GitApp/DisplayText.cs\u001b[0m\u001b[0m                  │",
                "\u001b[0m│\u001b[0mGitApp/GitApp/ConsoleWrapper.cs\u001b[0m\u001b[0m               │",
                "\u001b[0m│\u001b[0mGitApp/GitApp/IElement.cs\u001b[0m\u001b[0m                     │",
                "\u001b[0m│\u001b[0mGitApp/GitApp/Program.cs\u001b[0m\u001b[0m                      │",
                "\u001b[0m│\u001b[0mGitApp/GitApp/InitialData.cs\u001b[0m\u001b[0m                  │",
                "\u001b[0m│\u001b[0mGitApp/GitApp/IInteractor.cs\u001b[0m\u001b[0m                  │",
                "\u001b[0m│\u001b[0mGitApp/GitApp.Tests/DisplayTextTests.cs\u001b[0m\u001b[0m       │",
                "\u001b[0m│\u001b[0mGitApp/GitApp.Tests/FakeConsole.cs\u001b[0m\u001b[0m            │",
                "\u001b[0m└──────────────────────────────────────────────┘"
            };
            Assert.Equal(expected, output);
        }
    }
}
