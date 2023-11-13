using Castle.Components.DictionaryAdapter.Xml;
using Moq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata.Ecma335;
using System.Reflection.PortableExecutable;
using System.Runtime.ExceptionServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace GitApp
{
    public class DisplayText
    {
        private const string MenuBar = "\u001b[0mStatus [1] | Log [2]";
        private readonly FullCommits commits;
        private readonly GitManager gitManager;
        private readonly IConsoleWrapper consoleWrapper;
        private readonly ConsoleKey[] menuKeys;
        private TextWriter writer;
        ConsoleKey key;

        public DisplayText(FullCommits commits, TextWriter writer, GitManager gitManager, IConsoleWrapper consoleWrapper)
        {
            this.commits = commits;
            this.writer = writer;
            this.gitManager = gitManager;
            this.consoleWrapper = consoleWrapper;
            IsRightSideInUse = false;
            WasWindowResizedSmaller = false;
            menuKeys = new[] { ConsoleKey.D1, ConsoleKey.D2 };
            key = ConsoleKey.NoName;
        }

        private bool WasWindowResizedSmaller { get; set; }

        private bool IsRightSideInUse { get; set; }

        public void Display()
        {
            DisplayStatus();
            do
            {
                if (key.Equals(ConsoleKey.D1))
                {
                    DisplayStatus();
                }
                else if (key.Equals(ConsoleKey.D2))
                {
                    DisplayLog();
                }
            }
            while (key != ConsoleKey.Escape);
        }

        private static List<string> MergeSides(List<string> leftSide, List<string> rightSide)
        {
            var result = new List<string>();

            for (int i = 0; i < leftSide.Count; i++)
            {
                result.Add(i < rightSide.Count ? leftSide[i] : leftSide[i] + "\n");
                if (i < rightSide.Count)
                {
                    result[i] += rightSide[i];
                }
            }

            return result;
        }

        private bool GetExitConditionForHighlightedHunksPage(
            KeyInteractor<IElement> keyInteractor,
            ModifiedFiles unstaged,
            List<IElement> diffElements)
        {
            var exitCondition = keyInteractor.KeyInfo.Equals(ConsoleKey.Escape);
            exitCondition = exitCondition || menuKeys.Contains(key);
            exitCondition = exitCondition || keyInteractor.KeyInfo.Equals(ConsoleKey.LeftArrow);
            exitCondition = exitCondition || unstaged[unstaged.Current].GetElement() == null;
            exitCondition = exitCondition || keyInteractor.KeyInfo.Equals(ConsoleKey.A);
            exitCondition = exitCondition || diffElements[0].GetElement() == "";
            return exitCondition;
        }

        private void DisplayLog()
        {
            var commitTextSize = 0;
            var windowWidth = consoleWrapper.WindowWidth;
            var windowSize = consoleWrapper.WindowHeight * windowWidth;
            var initialText = new InitialData(gitManager).GetText();
            var initialTextSize = new PageUtilities(initialText).GetOccupancyRate(windowWidth);
            var page = new List<IElement>();
            var toDisplay = new List<string>();
            var isWindowToSmall = initialTextSize + new PageUtilities("\nPress 'Esc' to exit.").GetOccupancyRate(windowWidth) >= windowSize;
            toDisplay = isWindowToSmall
                        ? new List<string>()
                        : UpdatePage(initialText, ref commitTextSize, ref page);
            Write(toDisplay, isWindowToSmall);
            var windowSizeTemp = windowSize;

            do
            {
                key = consoleWrapper.ReadKey().Key;

                if (key.Equals(ConsoleKey.Escape))
                {
                    break;
                }

                if (key.Equals(ConsoleKey.Enter))
                {
                    DisplayExtendedPage();
                }

                if (menuKeys.Contains(key))
                {
                    break;
                }

                windowWidth = consoleWrapper.WindowWidth;
                windowSize = consoleWrapper.WindowHeight * windowWidth;
                WasWindowResizedSmaller = windowSize < windowSizeTemp;
                windowSizeTemp = windowSize;
                initialTextSize = new PageUtilities(initialText).GetOccupancyRate(windowWidth);
                isWindowToSmall = initialTextSize + new PageUtilities("\nPress 'Esc' to exit.").GetOccupancyRate(windowWidth) >= windowSize;
                toDisplay = isWindowToSmall
                        ? new List<string>()
                        : UpdatePage(initialText, ref commitTextSize, ref page);
                Write(toDisplay, isWindowToSmall);
            }
            while (true);
        }

        private void DisplayStatus()
        {
            var repoPath = gitManager.GetRepoPath();
            var unstaged = new UnstagedFiles(gitManager.GetFileStatus(repoPath));
            var windowHeight = consoleWrapper.WindowHeight;
            var windowWidth = consoleWrapper.WindowWidth;
            var windowSize = windowHeight * windowWidth;
            var windowSizeTemp = windowSize;
            var unstagedHeight = (windowHeight - 1) / 2 + (windowHeight - 1) % 2 - 2;
            var unstagedAvailableSpace = ((int)(0.6 * windowWidth) - 2) * unstagedHeight;
            var page = unstaged.GetPageDataAndSize(unstagedAvailableSpace, (int)(0.6 * windowWidth) - 2).list;
            var leftSide = unstaged[unstaged.Current].GetElement() == ""
                   ? new List<string>()
                   : unstaged.GetHighlightedSide(unstagedHeight, (int)(0.6 * windowWidth) - 2, unstagedAvailableSpace);
            var staged = new ModifiedFiles(gitManager.GetStagedFiles());
            var stagedToString = staged.GetHighlightedSide(windowHeight - unstagedHeight, (int)(0.6 * windowWidth) - 2, (windowHeight - unstagedHeight) * (int)(0.6 * windowWidth) - 2);
            stagedToString[0] = stagedToString[0].Replace("\x1b[7m", "");
            Write(UpdateStatusPage(ref page, ref leftSide, ref unstaged, repoPath, stagedToString), consoleWrapper.WindowHeight < 7);
            do
            {
                key = consoleWrapper.ReadKey().Key;
                if (key.Equals(ConsoleKey.RightArrow))
                {
                    var currentFile = unstaged[unstaged.Current].GetElement();
                    DisplayHighlightedHunksPage(
                        new Diff(gitManager.GetUnstagedDiff(currentFile, repoPath)),
                        ref unstaged,
                        repoPath,
                        currentFile);
                }

                if (key.Equals(ConsoleKey.Escape) || menuKeys.Contains(key))
                {
                    break;
                }

                windowHeight = consoleWrapper.WindowHeight;
                windowWidth = consoleWrapper.WindowWidth;
                windowSize = windowHeight * windowWidth;
                WasWindowResizedSmaller = windowSize < windowSizeTemp;
                windowSizeTemp = windowSize;
                staged = new ModifiedFiles(gitManager.GetStagedFiles());
                stagedToString = staged.GetHighlightedSide(windowHeight - unstagedHeight, (int)(0.6 * windowWidth) - 2, (windowHeight - unstagedHeight) * (int)(0.6 * windowWidth) - 2);
                stagedToString[0] = stagedToString[0].Replace("\x1b[7m", "");

                Write(UpdateStatusPage(ref page, ref leftSide, ref unstaged, repoPath, stagedToString), consoleWrapper.WindowHeight < 6);
            }
            while (true);
        }

        private void DisplayHighlightedHunksPage(Diff diff, ref UnstagedFiles unstaged, string repoPath, string currentFile)
        {
            var windowHeight = consoleWrapper.WindowHeight;
            var windowWidth = consoleWrapper.WindowWidth;
            var windowSize = windowHeight * windowWidth;
            var windowSizeTemp = windowSize;
            var rightSideWidth = new TextWidthSetter(consoleWrapper).SetTextWidth(false);
            var availableSpace = rightSideWidth * (windowHeight - 3);
            var rightSide = diff[diff.Current].GetElement() == ""
                ? new List<string>()
                : diff.GetHighlightedSide(windowHeight - 3, rightSideWidth, availableSpace, true);
            var keyInteractor = new KeyInteractor<IElement>(diff);
            var staged = new ModifiedFiles(gitManager.GetStagedFiles());
            (var page, var pageSize) = diff.GetPageDataAndSize(availableSpace, rightSideWidth, true);
            var diffElements = diff.GetText();
            Write(UpdateHighlightedHunksPage(rightSide, unstaged, staged, keyInteractor), windowHeight < 7);

            do
            {
                key = consoleWrapper.ReadKey().Key;
                keyInteractor.KeyInfo = key;
                staged = new ModifiedFiles(gitManager.GetStagedFiles());
                var stagedList = staged.GetText();

                if (keyInteractor.KeyInfo.Equals(ConsoleKey.Enter))
                {
                    UpdateVariablesWhenStagingHunk(ref diff, ref unstaged, repoPath, ref diffElements, currentFile);
                }
                else if (keyInteractor.KeyInfo.Equals(ConsoleKey.S))
                {
                    UpdateVariablesWhenStagingLine(ref diff, repoPath, rightSide, stagedList, currentFile, ref unstaged, ref diffElements);
                }
                else if (keyInteractor.KeyInfo.Equals(ConsoleKey.A))
                {
                    new Patcher(repoPath, gitManager, diff, currentFile != "").StageAll(unstaged[unstaged.Current].GetElement());
                    unstaged = new UnstagedFiles(gitManager.GetFileStatus(repoPath));
                }

                if (GetExitConditionForHighlightedHunksPage(keyInteractor, unstaged, diffElements))
                {
                    break;
                }

                keyInteractor = new KeyInteractor<IElement>(diff);
                keyInteractor.KeyInfo = key;
                windowHeight = consoleWrapper.WindowHeight;
                rightSideWidth = new TextWidthSetter(consoleWrapper).SetTextWidth(false);
                availableSpace = rightSideWidth * (windowHeight - 3);
                windowHeight = consoleWrapper.WindowHeight;
                windowWidth = consoleWrapper.WindowWidth;
                windowSize = windowHeight * windowWidth;
                (diff.Index, diff.Current) = keyInteractor.KeySwitch(rightSide, page[^1].Id, page[0].Id, pageSize > availableSpace, rightSideWidth, diff[diff.Current].GetElement().Length);
                (page, pageSize) = diff.GetPageDataAndSize(availableSpace, rightSideWidth, true);
                rightSide = diff[diff.Current].GetElement() == ""
                    ? new List<string>()
                    : diff.GetHighlightedSide(windowHeight - 3, rightSideWidth, availableSpace, true);
                WasWindowResizedSmaller = windowSize < windowSizeTemp;
                windowSizeTemp = windowSize;
                staged = new ModifiedFiles(gitManager.GetStagedFiles());
                Write(UpdateHighlightedHunksPage(rightSide, unstaged, staged, keyInteractor), windowHeight < 7);
            }
            while (true);
        }

        private void UpdateVariablesWhenStagingHunk(ref Diff diff, ref UnstagedFiles unstaged, string repoPath, ref List<IElement> diffElements, string currentFile)
        {
            var diffIndex = diff.Index == 0 ? 0 : diff.Index;
            var diffCurrent = diff.Current == 0 ? 0 : diff.Current;
            var patchPath = new Patcher(repoPath, gitManager, diff, currentFile != "").GetHunkPatchPath();
            var currentUnstaged = unstaged[unstaged.Current].GetElement();
            var unstagedIndex = unstaged.Index;
            var unstagedCurrent = unstaged.Current;
            unstaged = new UnstagedFiles(gitManager.GetFileStatus(repoPath));
            unstaged.Current = unstagedCurrent;
            unstaged.Index = unstagedIndex;
            gitManager.Stage(patchPath);
            if (System.IO.File.Exists(patchPath))
            {
                System.IO.File.Delete(patchPath);
            }

            diff = new Diff(gitManager.GetUnstagedDiff(currentUnstaged, repoPath));
            diffElements = diff.GetText();
            diff.Current = Math.Min(diffCurrent, diffElements.Count - 1);
            diff.Index = diffIndex;
        }

        private void UpdateVariablesWhenStagingLine(
            ref Diff diff,
            string repoPath,
            List<string> rightSide,
            List<IElement> stagedList,
            string currentFile,
            ref UnstagedFiles unstaged,
            ref List<IElement> diffElements)
        {
            var diffIndex = diff.Index == 0 ? 0 : diff.Index;
            var diffCurrent = diff.Current == 0 ? 0 : diff.Current - 1;
            var patchPath = new Patcher(repoPath, gitManager, diff, currentFile != "")
                .GetLinePatchPath(
                    rightSide.Any(line => line.StartsWith("\u001b[7m\x1b[38;5;190m")),
                    stagedList.Any(unstagedFile => unstagedFile.GetElement() == currentFile),
                    currentFile);
            var currentUnstaged = unstaged[unstaged.Current].GetElement();
            var unstagedIndex = unstaged.Index;
            var unstagedCurrent = unstaged.Current;
            unstaged = new UnstagedFiles(gitManager.GetFileStatus(repoPath));
            unstaged.Current = unstagedCurrent;
            unstaged.Index = unstagedIndex;
            gitManager.Stage(patchPath);
            if (System.IO.File.Exists(patchPath))
            {
                System.IO.File.Delete(patchPath);
            }

            diff = new Diff(gitManager.GetUnstagedDiff(currentUnstaged, repoPath));
            diffElements = diff.GetText();
            diff.Current = Math.Min(diffCurrent, diffElements.Count - 1);
            diff.Index = diffIndex;
        }

        private List<string> GetTextToRedraw(List<string> toWrite)
        {
            var result = new List<string>();
            toWrite.ForEach(element =>
            {
                var elementUtilities = new PageUtilities(element);
                var clearedElement = elementUtilities.FormatingCodesStringUtilities().clearedString;
                var padLength = new PageUtilities(clearedElement).GetOccupancyRate(consoleWrapper.WindowWidth) - clearedElement.Length;
                var toPad = new string(' ', padLength);
                result.Add(clearedElement.Length % consoleWrapper.WindowWidth != 0 ? element + toPad : element);
            });

            return result;
        }

        private List<string> SelectSide(List<string> leftSide, List<string> rightSide, KeyInteractor<IElement> keyInteractor)
        {
            IsRightSideInUse = keyInteractor.KeyInfo.Equals(ConsoleKey.Spacebar) ? !IsRightSideInUse : IsRightSideInUse;
            return IsRightSideInUse ? rightSide : leftSide;
        }

        private List<string> SelectScreen(List<string> leftSide, List<string> rightSide, KeyInteractor<IElement> keyInteractor)
        {
            var height = consoleWrapper.WindowHeight;
            var width = consoleWrapper.WindowWidth;
            if (height < 15 && width < 50)
            {
                return SelectSide(leftSide, rightSide, keyInteractor);
            }

            return MergeSides(leftSide, rightSide);
        }

        private void Write(List<string> toWrite, bool isWindowToSmall)
        {
            var width = consoleWrapper.WindowWidth;
            var windowSize = consoleWrapper.WindowHeight * width;
            if (isWindowToSmall)
            {
                writer = new StringWriter();
                consoleWrapper.Clear();
                consoleWrapper.Write("Increase window size!");
            }
            else
            {
                toWrite.Insert(0, MenuBar);
                var temp = GetTextToRedraw(toWrite);
                if (WasWindowResizedSmaller)
                {
                    WasWindowResizedSmaller = false;
                    consoleWrapper.Clear();
                }

                writer = new StringWriter();
                temp.ForEach(element => writer.WriteLine(element));
                consoleWrapper.SetCursorPosition(0, 0);
                consoleWrapper.Write(writer.ToString()[..^1].TrimStart());
            }
        }

        private void UpdateLeftSideVariables(
            ref List<IElement> elements,
            ref List<string> leftSide,
            bool isWindowToSmall,
            IInteractor<IElement> interactor,
            int width)
        {
            var windowWidth = consoleWrapper.WindowWidth;
            var windowHeight = consoleWrapper.WindowHeight;
            if (isWindowToSmall)
            {
                return;
            }

            if (windowWidth < 50 && windowHeight < 15)
            {
                elements = interactor.GetPageDataAndSize(width * (windowHeight - 3), width - 2).list;
                leftSide = interactor.GetHighlightedSide(windowHeight - 3, width, width * (windowHeight - 3), true);
            }
            else
            {
                elements = interactor.GetPageDataAndSize(width * (windowHeight - 3), width, true).list;
                leftSide = interactor.GetHighlightedSide(windowHeight - 3, width, width * (windowHeight - 3), true);
            }
        }

        private void DisplayExtendedPage()
        {
            var keyInteractor = new KeyInteractor<IElement>(commits);
            var windowHeight = consoleWrapper.WindowHeight;
            var windowWidth = consoleWrapper.WindowWidth;
            var windowSize = windowHeight * windowWidth;
            var windowSizeTemp = windowSize;
            var isWindowToSmall = windowHeight < 10;
            var commitList = new List<IElement>();
            var leftSideCommits = new List<string>();
            UpdateLeftSideVariables(ref commitList, ref leftSideCommits, isWindowToSmall, commits, windowWidth < 50 && windowHeight < 15 ? windowWidth - 2 : ((int)(0.6 * windowWidth) - 2));
            var toDisplay = isWindowToSmall
                ? new List<string>()
                : UpdateExtendedPage(leftSideCommits, keyInteractor);
            Write(toDisplay, isWindowToSmall);
            do
            {
                key = consoleWrapper.ReadKey().Key;
                keyInteractor.KeyInfo = key;
                if (keyInteractor.KeyInfo == ConsoleKey.RightArrow)
                {
                    DisplayDiffPage(commits[commits.Current], commits[commits.Current + 1]);
                }

                if (keyInteractor.KeyInfo.Equals(ConsoleKey.Escape) || menuKeys.Contains(key))
                {
                    break;
                }

                (commits.Index, commits.Current) = keyInteractor
                    .KeySwitch(leftSideCommits, commitList[^1].Id, commitList[0].Id, false, consoleWrapper.WindowWidth, commits[commits.Current].GetElement().Length - 40);
                windowHeight = consoleWrapper.WindowHeight;
                windowSize = windowHeight * consoleWrapper.WindowWidth;
                WasWindowResizedSmaller = windowSize < windowSizeTemp;
                windowSizeTemp = windowSize;
                isWindowToSmall = consoleWrapper.WindowHeight < 10;
                UpdateLeftSideVariables(ref commitList, ref leftSideCommits, isWindowToSmall, commits, new TextWidthSetter(consoleWrapper).SetTextWidth(true));
                toDisplay = isWindowToSmall
                    ? new List<string>()
                    : UpdateExtendedPage(leftSideCommits, keyInteractor);
                Write(toDisplay, isWindowToSmall);
            }
            while (true);
        }

        private void DisplayDiffPage(IElement currentCommit, IElement previousCommit)
        {
            var repoPath = gitManager.GetRepoPath();
            var windowHeight = consoleWrapper.WindowHeight;
            var windowWidth = consoleWrapper.WindowWidth;
            var windowSize = windowHeight * windowWidth;
            var windowSizeTemp = windowSize;
            var rowSize = SetTextWidth(false);
            var availableSpace = rowSize * (windowHeight - 3);
            var isWindowToSmall = windowHeight < 3;
            var modifiedFiles = new ModifiedFiles(gitManager.GetCommitModifiedFileNames(currentCommit.Id));
            var leftSideElements = new List<string>();
            var diff = new Diff(gitManager.GetDiffBetweenConsecutiveCommits(previousCommit.Id, currentCommit.Id, repoPath, modifiedFiles[0].GetElement()));
            var page = new List<IElement>();
            var keyInteractor = new KeyInteractor<IElement>(modifiedFiles);
            keyInteractor.KeyInfo = ConsoleKey.NoName;
            UpdateLeftSideVariables(ref page, ref leftSideElements, isWindowToSmall, modifiedFiles, rowSize);
            var rightSide = windowWidth < 50 && windowHeight < 15
                    ? diff.GetHighlightedSide(windowHeight - 3, windowWidth - 2, (windowHeight - 2) * (windowWidth - 2), false)
                    : diff.GetHighlightedSide(windowHeight - 3, windowWidth - (int)(0.6 * windowWidth) - 2, availableSpace, false);
            var toDisplay = isWindowToSmall
                ? new List<string>()
                : UpdateDiffPage(leftSideElements, rightSide, rowSize + 2, keyInteractor);
            Write(toDisplay, windowHeight < 3);
            do
            {
                key = consoleWrapper.ReadKey().Key;
                keyInteractor.KeyInfo = key;
                if (keyInteractor.KeyInfo.Equals(ConsoleKey.RightArrow))
                {
                    GetHighlightedDiffPage(diff, leftSideElements);
                }

                if (keyInteractor.KeyInfo.Equals(ConsoleKey.Escape) || keyInteractor.KeyInfo.Equals(ConsoleKey.LeftArrow) || menuKeys.Contains(key))
                {
                    break;
                }

                windowHeight = consoleWrapper.WindowHeight;
                windowWidth = consoleWrapper.WindowWidth;
                isWindowToSmall = windowHeight < 3;
                rowSize = SetTextWidth(false);
                availableSpace = rowSize * (windowHeight - 3);
                windowSize = windowHeight * consoleWrapper.WindowWidth;
                WasWindowResizedSmaller = windowSize < windowSizeTemp;
                windowSizeTemp = windowSize;
                (modifiedFiles.Index, modifiedFiles.Current) = keyInteractor.KeySwitch(leftSideElements, page[^1].Id, page[0].Id, false, windowWidth, page[^1].GetElement().Length);
                UpdateLeftSideVariables(ref page, ref leftSideElements, isWindowToSmall, modifiedFiles, rowSize);
                var filePath = modifiedFiles[modifiedFiles.Current].GetElement();
                diff = new Diff(gitManager.GetDiffBetweenConsecutiveCommits(previousCommit.Id, currentCommit.Id, repoPath, filePath));
                rightSide = windowWidth < 50 && windowHeight < 15
                    ? diff.GetHighlightedSide(windowHeight - 3, windowWidth - 2, (windowHeight - 2) * (windowWidth - 2), false)
                    : diff.GetHighlightedSide(windowHeight - 3, windowWidth - (int)(0.6 * windowWidth) - 2, availableSpace, false);
                toDisplay = isWindowToSmall
                    ? new List<string>()
                    : UpdateDiffPage(leftSideElements, rightSide, rowSize + 2, keyInteractor);
                Write(toDisplay, windowHeight < 3);
            }
            while (true);
        }

        private void GetHighlightedDiffPage(Diff diff, List<string> leftSideElements)
        {
            var windowHeight = consoleWrapper.WindowHeight;
            var windowWidth = consoleWrapper.WindowWidth;
            var windowSize = windowHeight * windowWidth;
            var windowSizeTemp = windowSize;
            var rowSize = SetTextWidth(false);
            var availableSpace = rowSize * (windowHeight - 3);
            (var page, var pageSize) = diff.GetPageDataAndSize(availableSpace, rowSize, true);
            var highlightedDiff = diff.GetHighlightedSide(windowHeight - 3, rowSize, availableSpace, true);
            var keyInteractor = new KeyInteractor<IElement>(diff);
            var toDisplay = windowHeight < 3
                ? new List<string>()
                : UpdateDiffPage(leftSideElements, highlightedDiff, rowSize + 2, keyInteractor, true);
            Write(toDisplay, windowHeight < 3);
            do
            {
                key = consoleWrapper.ReadKey().Key;
                keyInteractor.KeyInfo = key;
                if (keyInteractor.KeyInfo == ConsoleKey.Escape || keyInteractor.KeyInfo == ConsoleKey.LeftArrow || menuKeys.Contains(key))
                {
                    break;
                }

                windowHeight = consoleWrapper.WindowHeight;
                rowSize = SetTextWidth(false);
                availableSpace = rowSize * (windowHeight - 3);
                windowSize = windowHeight * consoleWrapper.WindowWidth;
                WasWindowResizedSmaller = windowSize < windowSizeTemp;
                windowSizeTemp = windowSize;
                (diff.Index, diff.Current) = keyInteractor.KeySwitch(highlightedDiff, page[^1].Id, page[0].Id, pageSize > availableSpace, rowSize - 2, diff[diff.Current].GetElement().Length);
                (page, pageSize) = diff.GetPageDataAndSize(availableSpace, rowSize, true);
                highlightedDiff = diff.GetHighlightedSide(windowHeight - 3, rowSize, availableSpace, true);
                toDisplay = windowHeight < 3
                ? new List<string>()
                : UpdateDiffPage(leftSideElements, highlightedDiff, rowSize + 2, keyInteractor, true);
                Write(toDisplay, windowHeight < 3);
            }
            while (true);
        }

        private int SetTextWidth(bool forLeftSide)
        {
            var windowWidth = consoleWrapper.WindowWidth;
            var result = forLeftSide ? (int)(0.6 * windowWidth) - 2 : windowWidth - (int)(0.6 * windowWidth) - 2;
            var windowHeight = consoleWrapper.WindowHeight;
            return windowWidth < 50 && windowHeight < 15
                ? windowWidth - 2
                : result;
        }

        private List<string> UpdateDiffPage(
            List<string> leftSide,
            List<string> diff,
            int rowSize,
            KeyInteractor<IElement> keyInteractor,
            bool shouldColorBoxLines = false)
        {
            var windowHeight = consoleWrapper.WindowHeight;
            var windowWidth = consoleWrapper.WindowWidth;
            List<string> rightSide;
            if (windowHeight < 15 && windowWidth < 50)
            {
                rightSide = new BoxDrawer(diff, "Diff").DrawBox(windowWidth, windowHeight - 1, false, shouldColorBoxLines);
                leftSide = new BoxDrawer(leftSide, "Files").DrawBox(windowWidth, windowHeight - 1);
            }
            else
            {
                rightSide = new BoxDrawer(diff, "Diff").DrawBox(rowSize, windowHeight - 1, false, shouldColorBoxLines);
                leftSide = new BoxDrawer(leftSide, "Files").DrawBox((int)(0.6 * windowWidth), windowHeight - 1);
            }

            return SelectScreen(leftSide, rightSide, keyInteractor);
        }

        private List<string> UpdateHighlightedHunksPage(
            List<string> diff,
            ModifiedFiles unstaged,
            ModifiedFiles staged,
            KeyInteractor<IElement> keyInteractor)
        {
            var windowHeight = consoleWrapper.WindowHeight;
            var windowWidth = consoleWrapper.WindowWidth;
            var unstagedHeight = (windowHeight - 1) / 2 + (windowHeight - 1) % 2 - 2;
            var unstagedAvailableSpace = ((int)(0.6 * windowWidth) - 2) * unstagedHeight;
            List<string> rightSide;
            List<string> leftSide;
            if (windowHeight < 15 && windowWidth < 50)
            {
                rightSide = new BoxDrawer(diff, "Diff").DrawBox(windowWidth, windowHeight - 1, false, true);
                leftSide = new BoxDrawer(unstaged.GetHighlightedSide(unstagedHeight, windowWidth - 2, (unstagedHeight - 2) * (windowWidth - 2)), "Unstaged").DrawBox(windowWidth, unstagedHeight + 2);
                var stagedToString = staged.GetHighlightedSide(
                        windowHeight - ((windowHeight - 1) / 2 + (windowHeight - 1) % 2 - 2) - 2,
                        (int)(0.6 * windowWidth) - 2,
                        ((windowHeight - 1) / 2 - 2) * (int)(0.6 * windowWidth) - 2);
                stagedToString[0] = stagedToString[0].Replace("\u001b[7m", "");
                stagedToString = new BoxDrawer(stagedToString, "Staged").DrawBox(windowWidth, windowHeight - unstagedHeight - 3, true);
                leftSide.AddRange(stagedToString);
            }
            else
            {
                var textwidth = new TextWidthSetter(consoleWrapper);
                var leftSideWidth = textwidth.SetTextWidth(true);
                rightSide = new BoxDrawer(diff, "Diff").DrawBox(textwidth.SetTextWidth(false) + 2, windowHeight - 1, false, true);
                leftSide = new BoxDrawer(unstaged.GetHighlightedSide(unstagedHeight, leftSideWidth, unstagedAvailableSpace), "Unstaged")
                    .DrawBox((int)(0.6 * windowWidth), unstagedHeight + 2);
                var stagedToString = staged.GetHighlightedSide(
                        windowHeight - ((windowHeight - 1) / 2 + (windowHeight - 1) % 2 - 2) - 2,
                        (int)(0.6 * windowWidth) - 2,
                        ((windowHeight - 1) / 2 - 2) * (int)(0.6 * windowWidth) - 2);
                stagedToString[0] = stagedToString[0].Replace("\u001b[7m", "");
                stagedToString = new BoxDrawer(stagedToString, "Staged").DrawBox((int)(0.6 * windowWidth), windowHeight - unstagedHeight - 3, true);
                leftSide.AddRange(stagedToString);
            }

            return SelectScreen(leftSide, rightSide, keyInteractor);
        }

        private List<string> UpdateExtendedPage(List<string> leftSideCommits, KeyInteractor<IElement> keyInteractor)
        {
            var width = consoleWrapper.WindowWidth;
            var height = consoleWrapper.WindowHeight;
            var rightSide = consoleWrapper.WindowHeight < 15 && consoleWrapper.WindowWidth < 50
                ? commits.FormateUnhighlightedSide(height - 1, width, gitManager)
                : commits.FormateUnhighlightedSide(height - 1, width - (int)(0.6 * width), gitManager);

            return SelectScreen(
                new BoxDrawer(leftSideCommits, "Commits").DrawBox(height < 15 && width < 50 ? width : (int)(0.6 * width), height - 1),
                rightSide,
                keyInteractor);
        }

        private List<string> UpdatePage(
            string initialText,
            ref int commitOcupancyRate,
            ref List<IElement> page)
        {
            var keyInteractor = new KeyInteractor<IElement>(commits)
            {
                KeyInfo = key
            };

            var windowWidth = consoleWrapper.WindowWidth;
            var windowSize = consoleWrapper.WindowHeight * windowWidth;
            var initialTextUtilities = new PageUtilities(initialText);
            var initialTextSize = initialTextUtilities.GetOccupancyRate(windowWidth);
            var availableSpace = windowSize - initialTextSize - 2 * windowWidth;
            var isPageLargerThanWindow = commitOcupancyRate > availableSpace;
            var commitsToString = new List<string>();
            page.ForEach(line =>
            {
                var commitDetails = line.GetElement().Split("\n");
                commitsToString.Add(commitDetails[0] + " " + commitDetails[1] + " " + commitDetails[2] + " " + commitDetails[3]);
            });
            (commits.Index, commits.Current) = page.Count == 0
                ? (0, 0)
                : keyInteractor.KeySwitch(commitsToString, page[^1].Id, page[0].Id, isPageLargerThanWindow, consoleWrapper.WindowWidth, commits[commits.Current].GetElement().Length - 40);
            (page, commitOcupancyRate) = commits.GetPageDataAndSize(availableSpace, consoleWrapper.WindowWidth);
            var toWrite = commits.GetHighlightedSide(consoleWrapper.WindowHeight - initialTextSize / windowWidth - 2, windowWidth, availableSpace);
            toWrite.Add("\u001b[0mPress 'Esc' to exit.");
            toWrite.InsertRange(0, initialText.Split("\n"));
            return toWrite;
        }

        private List<string> UpdateStatusPage(ref List<IElement> page, ref List<string> leftSide, ref UnstagedFiles unstaged, string repoPath, List<string> stagedToString)
        {
            var keyInteractor = new KeyInteractor<IElement>(unstaged);
            keyInteractor.KeyInfo = key;
            var windowHeight = consoleWrapper.WindowHeight;
            var windowWidth = consoleWrapper.WindowWidth;
            var diffAvailableSpace = (windowWidth - (int)(0.6 * windowWidth) - 2) * (windowHeight - 3);
            var unstagedHeight = (windowHeight - 1) / 2 + (windowHeight - 1) % 2 - 2;
            var unstagedAvailableSpace = ((int)(0.6 * windowWidth) - 2) * unstagedHeight;
            (unstaged.Index, unstaged.Current) = keyInteractor.KeySwitch(leftSide, page[^1].Id, page[0].Id, false, (int)(0.6 * windowWidth) - 2, unstaged[unstaged.Current].GetElement().Length);
            page = unstaged.GetPageDataAndSize(unstagedAvailableSpace, (int)(0.6 * windowWidth) - 2).list;
            leftSide = unstaged[unstaged.Current].GetElement() == ""
                   ? new List<string>()
                   : unstaged.GetHighlightedSide(unstagedHeight, (int)(0.6 * windowWidth) - 2, unstagedAvailableSpace);
            var leftSideTemp = new List<string>();
            leftSideTemp.AddRange(new BoxDrawer(leftSide, "Unstaged").DrawBox((int)(0.6 * windowWidth), unstagedHeight + 2));
            var currentUnstaged = unstaged[unstaged.Current].GetElement();
            var diff = new Diff(gitManager.GetUnstagedDiff(currentUnstaged, repoPath));
            var rightSide = new BoxDrawer(
                diff[0].GetElement() == ""
                    ? new List<string>()
                    : diff.GetHighlightedSide(windowHeight - 3, windowWidth - (int)(0.6 * windowWidth) - 2, diffAvailableSpace, false), "Diff")
                .DrawBox(windowWidth - (int)(0.6 * windowWidth), windowHeight - 1);
            stagedToString = new BoxDrawer(stagedToString, "Staged").DrawBox((int)(0.6 * windowWidth), (windowHeight - 1) / 2, true);
            leftSideTemp.AddRange(stagedToString);

            return MergeSides(leftSideTemp, rightSide);
        }
    }
}