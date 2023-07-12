using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics.Metrics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitApp.Tests
{
    public class KeyInteractorTests
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
        public void KeySwitchIsCalledKeyIsPgUpShouldReturnZeroAndZero()
        {
            IInteractor<IElement> commits = new FullCommits(Commits);
            var keyInteractor = new KeyInteractor<IElement>(commits);
            commits.Current = 4;
            commits.Index = 4;
            var page = new List<string> { 
                "f944893 Andrei Daboc 2023-05-12 Boxes are now properly displayed at any size.",
                "92e239d Andrei Daboc 2023-05-09 FitTextInsideBox method's loop now breaks if \"i\" is equal to current line element's length minus one.",
                "10580c9 Andrei Daboc 2023-05-09 Modified functionality of DisplaySplitScreen method.",
                "fae78e6 Andrei Daboc 2023-05-09 Fixed date column arrangement on left side." };
            keyInteractor.KeyInfo = ConsoleKey.PageUp;
            (commits.Index, commits.Current) = keyInteractor.KeySwitch(page, "fae78e6", "f944893", false, 120, commits[commits.Current].GetElement().Length - 40);
            Assert.Equal(0, commits.Current);
            Assert.Equal(0, commits.Index);
        }

        [Fact]
        public void KeySwitchIsCalledKeyIsPgUpAndCurrentAndIndexAreZeroShouldReturnUnchangedNumbers()
        {
            IInteractor<IElement> commits = new FullCommits(Commits);
            var keyInteractor = new KeyInteractor<IElement>(commits);
            var page = new List<string> {
                "f944893 Andrei Daboc 2023-05-12 Boxes are now properly displayed at any size.",
                "92e239d Andrei Daboc 2023-05-09 FitTextInsideBox method's loop now breaks if \"i\" is equal to current line element's length minus one.",
                "10580c9 Andrei Daboc 2023-05-09 Modified functionality of DisplaySplitScreen method.",
                "fae78e6 Andrei Daboc 2023-05-09 Fixed date column arrangement on left side." };
            keyInteractor.KeyInfo = ConsoleKey.PageUp;
            (commits.Index, commits.Current) = keyInteractor.KeySwitch(page, "f944893", "f944893", false, 120, commits[commits.Current].GetElement().Length - 40);
            Assert.Equal(0, commits.Current);
            Assert.Equal(0, commits.Index);
        }

        [Fact]
        public void KeySwitchIsCalledDownArrowIsPressedShouldReturnCurrentPlusOne()
        {
            IInteractor<IElement> commits = new FullCommits(Commits);
            var keyInteractor = new KeyInteractor<IElement>(commits);
            commits.Current = 4;
            commits.Index = 4;
            var page = new List<string> {
                "f944893 Andrei Daboc 2023-05-12 Boxes are now properly displayed at any size.",
                "92e239d Andrei Daboc 2023-05-09 FitTextInsideBox method's loop now breaks if \"i\" is equal to current line element's length minus one.",
                "10580c9 Andrei Daboc 2023-05-09 Modified functionality of DisplaySplitScreen method.",
                "fae78e6 Andrei Daboc 2023-05-09 Fixed date column arrangement on left side." };
            keyInteractor.KeyInfo = ConsoleKey.DownArrow;
            (commits.Index, commits.Current) = keyInteractor.KeySwitch(page, "fae78e6", "f944893", false, 120, commits[commits.Current].GetElement().Length - 40);
            Assert.Equal(5, commits.Current);
            Assert.Equal(4, commits.Index);
        }

        [Fact]
        public void KeySwitchIsCalledDownArrowIsPressedCurrentCommitIsLastOnPageShoudIncrementIndexAndCurrent()
        {
            IInteractor<IElement> commits = new FullCommits(Commits);
            var keyInteractor = new KeyInteractor<IElement>(commits);
            commits.Current = 7;
            commits.Index = 7;
            var page = new List<string> {
                "f944893 Andrei Daboc 2023-05-12 Boxes are now properly displayed at any size.",
                "92e239d Andrei Daboc 2023-05-09 FitTextInsideBox method's loop now breaks if \"i\" is equal to current line element's length minus one.",
                "10580c9 Andrei Daboc 2023-05-09 Modified functionality of DisplaySplitScreen method.",
                "fae78e6 Andrei Daboc 2023-05-09 Fixed date column arrangement on left side." };
            keyInteractor.KeyInfo = ConsoleKey.DownArrow;
            (commits.Index, commits.Current) = keyInteractor.KeySwitch(page, "fae78e6", "f944893", false, 120, commits[commits.Current].GetElement().Length - 40);
            Assert.Equal(8, commits.Current);
            Assert.Equal(8, commits.Index);
        }

        [Fact]
        public void KeySwitchIsCalledDownArrowIsPressedCurrentCommitIsLastOnPageAndPageIsLargerThanWindowShouldIncrementCurrentByOneAndIndexByThree()
        {
            var temp = Commits.Replace(
                "Fixed date column arrangement on left side.",
                "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa" +
                "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa" +
                "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa" +
                "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa" +
                "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa");
            IInteractor<IElement> commits = new FullCommits(Commits);
            var keyInteractor = new KeyInteractor<IElement>(commits);
            commits.Current = 7;
            commits.Index = 4;
            var page = new List<string> {
                "f944893 Andrei Daboc 2023-05-12 Boxes are now properly displayed at any size.",
                "92e239d Andrei Daboc 2023-05-09 FitTextInsideBox method's loop now breaks if \"i\" is equal to current line element's length minus one.",
                "10580c9 Andrei Daboc 2023-05-09 Modified functionality of DisplaySplitScreen method.",
                "fae78e6 Andrei Daboc 2023-05-09 "  +"aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa" +
                "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa" +
                "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa" +
                "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa" +
                "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa"};
            keyInteractor.KeyInfo = ConsoleKey.DownArrow;
            (commits.Index, commits.Current) = keyInteractor.KeySwitch(page, "fae78e6", "f944893", true, 38, commits[commits.Current].GetElement().Length - 40);
            Assert.Equal(8, commits.Current);
            Assert.Equal(7, commits.Index);
        }

        [Fact]
        public void KeySwitchIsCalledUpArrowIsPressedShouldDecreaseCurrentByOne()
        {
            IInteractor<IElement> commits = new FullCommits(Commits);
            var keyInteractor = new KeyInteractor<IElement>(commits);
            commits.Current = 5;
            commits.Index = 4;
            var page = new List<string> {
                "f944893 Andrei Daboc 2023-05-12 Boxes are now properly displayed at any size.",
                "92e239d Andrei Daboc 2023-05-09 FitTextInsideBox method's loop now breaks if \"i\" is equal to current line element's length minus one.",
                "10580c9 Andrei Daboc 2023-05-09 Modified functionality of DisplaySplitScreen method.",
                "fae78e6 Andrei Daboc 2023-05-09 Fixed date column arrangement on left side." };
            keyInteractor.KeyInfo = ConsoleKey.UpArrow;
            (commits.Index, commits.Current) = keyInteractor.KeySwitch(page, "fae78e6", "f944893", false,  120, commits[commits.Current].GetElement().Length - 40);
            Assert.Equal(4, commits.Current);
            Assert.Equal(4, commits.Index);
        }

        [Fact]
        public void KeySwitchIsCalledUpArrowIsPressedAndCurrentElementIsFirstOnPageShouldDecreaseIndexAndCurrentByOne()
        {
            IInteractor<IElement> commits = new FullCommits(Commits);
            var keyInteractor = new KeyInteractor<IElement>(commits);
            commits.Current = 4;
            commits.Index = 4;
            var page = new List<string> {
                "f944893 Andrei Daboc 2023-05-12 Boxes are now properly displayed at any size.",
                "92e239d Andrei Daboc 2023-05-09 FitTextInsideBox method's loop now breaks if \"i\" is equal to current line element's length minus one.",
                "10580c9 Andrei Daboc 2023-05-09 Modified functionality of DisplaySplitScreen method.",
                "fae78e6 Andrei Daboc 2023-05-09 Fixed date column arrangement on left side." };
            keyInteractor.KeyInfo = ConsoleKey.UpArrow;
            (commits.Index, commits.Current) = keyInteractor.KeySwitch(page, "f944893", "f944893", false, 120, commits[commits.Current].GetElement().Length - 40);
            Assert.Equal(3, commits.Current);
            Assert.Equal(3, commits.Index);
        }

        [Fact]
        public void KeySwitchIsCalledPageDownIsPressedShouldUpdateIndexAndCurrent()
        {
            IInteractor<IElement> commits = new FullCommits(Commits);
            var keyInteractor = new KeyInteractor<IElement>(commits);
            commits.Current = 4;
            commits.Index = 4;
            var page = new List<string> {
                "f944893 Andrei Daboc 2023-05-12 Boxes are now properly displayed at any size.",
                "92e239d Andrei Daboc 2023-05-09 FitTextInsideBox method's loop now breaks if \"i\" is equal to current line element's length minus one.",
                "10580c9 Andrei Daboc 2023-05-09 Modified functionality of DisplaySplitScreen method.",
                "fae78e6 Andrei Daboc 2023-05-09 Fixed date column arrangement on left side." };
            keyInteractor.KeyInfo = ConsoleKey.PageDown;
            (commits.Index, commits.Current) = keyInteractor.KeySwitch(page, "fae78e6", "f944893", false, 120, commits[commits.Current].GetElement().Length - 40);
            Assert.Equal(8, commits.Current);
            Assert.Equal(8, commits.Index);
        }

        [Fact]
        public void KeySwitchIsCalledPageDownIsPressedAndIsLastPageFromCommitsShouldNotUpdateIndex()
        {
            IInteractor<IElement> commits = new FullCommits(Commits);
            var keyInteractor = new KeyInteractor<IElement>(commits);
            commits.Current = 8;
            commits.Index = 8;
            var page = new List<string> {
                "f944893 Andrei Daboc 2023-05-12 Boxes are now properly displayed at any size.",
                "92e239d Andrei Daboc 2023-05-09 FitTextInsideBox method's loop now breaks if \"i\" is equal to current line element's length minus one.",
                "10580c9 Andrei Daboc 2023-05-09 Modified functionality of DisplaySplitScreen method.",
                "d3d8ef7 Andrei Daboc 2023-05-08 Fixed an issue that caused not to display the correct date" };
            keyInteractor.KeyInfo = ConsoleKey.PageDown;
            (commits.Index, commits.Current) = keyInteractor.KeySwitch(page, "d3d8ef7", "f944893", false, 120, commits[commits.Current].GetElement().Length - 40);
            Assert.Equal(9, commits.Current);
            Assert.Equal(8, commits.Index);
        }
    }
}
