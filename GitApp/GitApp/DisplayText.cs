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
            var isWindowToSmall = initialTextSize >= windowSize - 2 * windowWidth;
            toDisplay = isWindowToSmall
                        ? new List<string>()
                        : UpdatePage(initialText, ref commitTextSize, ref page);
            toDisplay.Add("'Esc' to exit|'Enter' for next menu");
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
                isWindowToSmall = initialTextSize >= windowSize - 2 * windowWidth;
                toDisplay = isWindowToSmall
                            ? new List<string>()
                            : UpdatePage(initialText, ref commitTextSize, ref page);
                toDisplay.Add("'Esc' to exit|'Enter' for next menu");
                Write(toDisplay, isWindowToSmall);
            }
            while (true);
        }

        private void DisplayStatus()
        {
            var repoPath = gitManager.GetRepoPath();
            var windowHeight = consoleWrapper.WindowHeight;
            var windowWidth = consoleWrapper.WindowWidth;
            var windowSize = windowHeight * windowWidth;
            var windowSizeTemp = windowSize;
            var unstagedHeight = (windowHeight - 2) / 2 + (windowHeight - 2) % 2 - 2;
            var unstagedAvailableSpace = ((int)(0.6 * windowWidth) - 2) * unstagedHeight;
            var unstaged = new ModifiedFiles(gitManager.GetUnstagedFiles());
            var unstagedElements = unstaged.GetPageDataAndSize(unstagedAvailableSpace, (int)(0.6 * windowWidth) - 2).list;
            var unstagedPageList = unstaged[unstaged.Current].GetElement() == ""
                   ? new List<string>()
                   : unstaged.GetHighlightedSide(unstagedHeight, (int)(0.6 * windowWidth) - 2, unstagedAvailableSpace);
            var stagedHeight = windowHeight - unstagedHeight;
            var stagedAvailableSpace = stagedHeight * ((int)(0.6 * windowWidth) - 2);
            var staged = new ModifiedFiles(gitManager.GetStagedFiles());
            var stagedElements = staged.GetPageDataAndSize(unstagedAvailableSpace, (int)(0.6 * windowWidth) - 2).list;
            var stagedPageList = staged[staged.Current].GetElement() == ""
                    ? new List<string>()
                    : staged.GetHighlightedSide(stagedHeight, (int)(0.6 * windowWidth) - 2, stagedAvailableSpace);
            var isUnstagedInUse = true;
            var isStagedInUse = false;
            var unstagedAvailability = GetUnstagedAvailability(ref staged, ref unstaged, ref isUnstagedInUse, ref isStagedInUse, true, stagedHeight, unstagedHeight);
            var toDisplay = UpdateStatusPage(
                ref unstagedElements,
                ref stagedElements,
                ref unstagedPageList,
                ref stagedPageList,
                ref unstaged,
                ref staged,
                unstagedAvailability);
            toDisplay.Add("'Esc' to exit|Right Arrow for next menu");
            Write(toDisplay, consoleWrapper.WindowHeight < 7);

            do
            {
                key = consoleWrapper.ReadKey().Key;
                if (key.Equals(ConsoleKey.RightArrow))
                {
                    var currentFile = unstaged[unstaged.Current].GetElement();
                    DisplayHighlightedHunksPage(
                        new Diff(unstagedAvailability
                            ? gitManager.GetUnstagedDiff(currentFile, repoPath, currentFile != "" && unstaged.GetText().Count >= 1)
                            : gitManager.GetStagedDiff(staged[staged.Current].GetElement(), gitManager.GetRepoPath())),
                        ref unstaged,
                        ref staged,
                        repoPath,
                        currentFile,
                        unstagedAvailability);
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
                var unstagedTempIndex = unstaged.Index;
                var unstagedTempCurrent = unstaged.Current;
                unstaged = new ModifiedFiles(gitManager.GetUnstagedFiles());
                var unstagedCount = unstaged.ListCount;
                unstaged.Index = Math.Min(unstagedTempIndex, unstagedCount - 1);
                unstaged.Current = Math.Min(unstagedTempCurrent, unstagedCount - 1);
                var stagedTempIndex = staged.Index;
                var stagedTempCurrent = staged.Current;
                staged = new ModifiedFiles(gitManager.GetStagedFiles());
                var stagedCount = staged.ListCount;
                staged.Index = Math.Min(stagedTempIndex, stagedCount - 1);
                staged.Current = Math.Min(stagedTempCurrent, stagedCount - 1);
                unstagedAvailability = GetUnstagedAvailability(ref staged, ref unstaged, ref isUnstagedInUse, ref isStagedInUse, unstagedAvailability, stagedHeight, unstagedHeight);
                toDisplay = UpdateStatusPage(
                    ref unstagedElements,
                    ref stagedElements,
                    ref unstagedPageList,
                    ref stagedPageList,
                    ref unstaged,
                    ref staged,
                    unstagedAvailability);
                toDisplay.Add("'Esc' to exit|Right Arrow for next menu");
                Write(toDisplay, consoleWrapper.WindowHeight < 7);
            }
            while (true);
        }

        private (int index, int current) SetModifiedFileIndeexes(ModifiedFiles modifiedFilesToSet, ModifiedFiles modifiedFilesToUse, int height)
        {
            if (modifiedFilesToUse.Current == 0 && key == ConsoleKey.UpArrow)
            {
                modifiedFilesToSet.Current = modifiedFilesToSet.ListCount - 1;
                modifiedFilesToSet.Index = modifiedFilesToSet.ListCount - height < 0 ? 0 : modifiedFilesToSet.ListCount - height;
            }
            else
            {
                modifiedFilesToSet.Current = 0;
                modifiedFilesToSet.Index = 0;
            }

            return (modifiedFilesToSet.Index, modifiedFilesToSet.Current);
        }

        private bool GetUnstagedAvailability(
            ref ModifiedFiles staged,
            ref ModifiedFiles unstaged,
            ref bool isUnstagedInUse,
            ref bool isStagedInUse,
            bool unstagedAvailability,
            int stagedHeight,
            int unstagedHeight)
        {
            var conditionToUseUnstaged = (unstaged.Current == 0 && key == ConsoleKey.UpArrow) || (unstaged.Current == unstaged.ListCount - 1 && key == ConsoleKey.DownArrow);
            var conditionToUseStaged = (staged.Current == 0 && key == ConsoleKey.UpArrow) || (staged.Current == staged.ListCount - 1 && key == ConsoleKey.DownArrow);
            if (conditionToUseUnstaged && !isStagedInUse)
            {
                (staged.Index, staged.Current) = SetModifiedFileIndeexes(staged, unstaged, stagedHeight);
                key = ConsoleKey.NoName;
                isUnstagedInUse = false;
                isStagedInUse = true;
                return false;
            }
            else if (conditionToUseStaged && !isUnstagedInUse)
            {
                (unstaged.Index, unstaged.Current) = SetModifiedFileIndeexes(unstaged, staged, unstagedHeight);
                key = ConsoleKey.NoName;
                isUnstagedInUse = true;
                isStagedInUse = false;
                return true;
            }

            return unstagedAvailability;
        }

        private void DisplayHighlightedHunksPage(Diff diff, ref ModifiedFiles unstaged, ref ModifiedFiles staged, string repoPath, string currentFile, bool unstagedAvailability)
        {
            var windowHeight = consoleWrapper.WindowHeight;
            var windowWidth = consoleWrapper.WindowWidth;
            var windowSize = windowHeight * windowWidth;
            var windowSizeTemp = windowSize;
            var rightSideWidth = new TextWidthSetter(consoleWrapper).SetTextWidth(false);
            var availableSpace = rightSideWidth * (windowHeight - 4);
            var rightSide = diff[diff.Current].GetElement() == ""
                ? new List<string>()
                : diff.GetHighlightedSide(windowHeight - 4, rightSideWidth, availableSpace, true);
            var keyInteractor = new KeyInteractor<IElement>(diff);
            (var page, var pageSize) = diff.GetPageDataAndSize(availableSpace, rightSideWidth, true);
            var diffElements = diff.GetText();
            var toDisplay = UpdateHighlightedHunksPage(rightSide, unstaged, staged, keyInteractor, unstagedAvailability);
            var additionalMenuBar = "'Esc' or Left Arrow to return" +
                (unstagedAvailability
                    ? "|'Enter' add hunk |'A' stage all|'S' stage line"
                    : "|'Enter' remove hunk |'A' unstage all|'S' unstage line");
            toDisplay.Add(additionalMenuBar);
            Write(toDisplay, windowHeight < 7);

            do
            {
                key = consoleWrapper.ReadKey().Key;
                keyInteractor.KeyInfo = key;
                var stagedList = staged.GetText();

                SelectStageOption(ref diff, ref unstaged, ref staged, ref diffElements, rightSide, stagedList, unstagedAvailability);

                if (GetExitConditionForHighlightedHunksPage(keyInteractor, unstaged, diffElements))
                {
                    break;
                }

                keyInteractor = new KeyInteractor<IElement>(diff);
                keyInteractor.KeyInfo = key;
                windowHeight = consoleWrapper.WindowHeight;
                rightSideWidth = new TextWidthSetter(consoleWrapper).SetTextWidth(false);
                availableSpace = rightSideWidth * (windowHeight - 4);
                windowHeight = consoleWrapper.WindowHeight;
                windowWidth = consoleWrapper.WindowWidth;
                windowSize = windowHeight * windowWidth;
                (diff.Index, diff.Current) = keyInteractor.KeySwitch(rightSide, page[^1].Id, page[0].Id, pageSize > availableSpace, rightSideWidth, diff[diff.Current].GetElement().Length);
                (page, pageSize) = diff.GetPageDataAndSize(availableSpace, rightSideWidth, true);
                rightSide = diff[diff.Current].GetElement() == ""
                    ? new List<string>()
                    : diff.GetHighlightedSide(windowHeight - 4, rightSideWidth, availableSpace, true);
                WasWindowResizedSmaller = windowSize < windowSizeTemp;
                windowSizeTemp = windowSize;
                toDisplay = UpdateHighlightedHunksPage(rightSide, unstaged, staged, keyInteractor, unstagedAvailability);
                toDisplay.Add(additionalMenuBar);
                Write(toDisplay, windowHeight < 7);
            }
            while (true);
        }

        private void SelectStageOption(
            ref Diff diff,
            ref ModifiedFiles unstaged,
            ref ModifiedFiles staged,
            ref List<IElement> diffElements,
            List<string> rightSide,
            List<IElement> stagedList,
            bool unstagedAvailability)
        {
            var currentFile = unstagedAvailability
                ? unstaged[unstaged.Current].GetElement()
                : staged[staged.Current].GetElement();
            var repoPath = gitManager.GetRepoPath();
            if (key.Equals(ConsoleKey.Enter))
            {
                UpdateVariablesWhenStagingOrUnstagingHunk(ref diff, ref unstaged, ref staged, repoPath, ref diffElements, unstagedAvailability);
            }
            else if (key.Equals(ConsoleKey.S))
            {
                UpdateVariablesWhenStagingOrUnstagingLine(ref diff, unstagedAvailability, rightSide, stagedList, ref unstaged, ref staged, ref diffElements);
            }
            else if (key.Equals(ConsoleKey.A))
            {
                if (unstagedAvailability)
                {
                    gitManager.StageFile(repoPath, currentFile);
                }
                else
                {
                    gitManager.UnstageFile(currentFile);
                }

                unstaged = new ModifiedFiles(gitManager.GetUnstagedFiles());
                staged = new ModifiedFiles(gitManager.GetStagedFiles());
            }
        }

        private void UpdateVariablesWhenStagingOrUnstagingHunk(
            ref Diff diff,
            ref ModifiedFiles unstaged,
            ref ModifiedFiles staged,
            string repoPath,
            ref List<IElement> diffElements,
            bool unstagedAvailability)
        {
            var currentUnstaged = unstaged[unstaged.Current].GetElement();
            var currentStaged = staged[staged.Current].GetElement();
            var diffIndex = diff.Index == 0 ? 0 : diff.Index;
            var diffCurrent = diff.Current == 0 ? 0 : diff.Current;
            var currentFile = unstagedAvailability ? currentUnstaged : currentStaged;
            var patchPath = new Patcher(repoPath, gitManager, diff, currentFile != "").GetHunkPatchPath();
            var unstagedIndex = unstaged.Index;
            var unstagedCurrent = unstaged.Current;
            unstaged = new ModifiedFiles(gitManager.GetUnstagedFiles());
            unstaged.Current = unstagedCurrent;
            unstaged.Index = unstagedIndex;
            var stagedIndex = staged.Index;
            var stagedCurrent = staged.Current;
            staged = new ModifiedFiles(gitManager.GetStagedFiles());
            staged.Current = stagedCurrent;
            staged.Index = stagedIndex;

            if (unstagedAvailability)
            {
                gitManager.Stage(patchPath);
            }
            else
            {
                var tempFilePath = Path.GetTempPath() + "tempFile" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".txt";
                System.IO.File.Create(tempFilePath).Close();
                var currentElementLines = System.IO.File.ReadAllLines(repoPath + "/" + currentFile);
                currentElementLines[0] = "﻿\ufeff" + currentElementLines[0];
                System.IO.File.WriteAllLines(tempFilePath, currentElementLines);
                gitManager.RestoreFileToMatchIndex(currentFile);
                gitManager.UnstageFile(currentFile);
                gitManager.ApplyPatchInReverse(patchPath);
                gitManager.StageFile(repoPath, currentFile);
                System.IO.File.WriteAllLines(repoPath + "/" + currentFile, System.IO.File.ReadAllLines(tempFilePath));
                System.IO.File.Delete(tempFilePath);
            }

            if (System.IO.File.Exists(patchPath))
            {
                System.IO.File.Delete(patchPath);
            }

            diff = unstagedAvailability
                ? new Diff(gitManager.GetUnstagedDiff(currentUnstaged, repoPath, currentFile != "" && unstaged.GetText().Count >= 1))
                : new Diff(gitManager.GetStagedDiff(currentStaged, repoPath));
            diffElements = diff.GetText();
            diff.Current = Math.Min(diffCurrent, diffElements.Count - 1);
            diff.Index = diffIndex;
        }

        private void UpdateVariablesWhenStagingOrUnstagingLine(
            ref Diff diff,
            bool unstagedAvailability,
            List<string> rightSide,
            List<IElement> stagedList,
            ref ModifiedFiles unstaged,
            ref ModifiedFiles staged,
            ref List<IElement> diffElements)
        {
            var currentStaged = staged[staged.Current].GetElement();
            var currentUnstaged = unstaged[unstaged.Current].GetElement();
            var repoPath = gitManager.GetRepoPath();
            var currentFile = unstagedAvailability ? currentUnstaged : currentStaged;
            var diffIndex = diff.Index == 0 ? 0 : diff.Index;
            var diffCurrent = diff.Current == 0 ? 0 : diff.Current - 1;
            var patchPath = new Patcher(gitManager.GetRepoPath(), gitManager, diff, currentFile != "")
                .GetLinePatchPath(
                    rightSide.Any(line => line.StartsWith("\u001b[7m\x1b[38;5;190m")),
                    stagedList.Any(unstagedFile => unstagedFile.GetElement() == currentUnstaged),
                    currentFile);
            var unstagedIndex = unstaged.Index;
            var unstagedCurrent = unstaged.Current;
            unstaged = new ModifiedFiles(gitManager.GetUnstagedFiles());
            unstaged.Current = unstagedCurrent;
            unstaged.Index = unstagedIndex;
            var stagedIndex = staged.Index;
            var stagedCurrent = staged.Current;
            staged = new ModifiedFiles(gitManager.GetStagedFiles());
            staged.Index = stagedIndex;
            staged.Current = stagedCurrent;
            if (unstagedAvailability)
            {
                gitManager.Stage(patchPath);
            }
            else
            {
                var tempFilePath = Path.GetTempPath() + "tempFile" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".txt";
                System.IO.File.Create(tempFilePath).Close();
                var currentElementLines = System.IO.File.ReadAllLines(repoPath + "/" + currentFile);
                currentElementLines[0] = "﻿\ufeff" + currentElementLines[0];
                System.IO.File.WriteAllLines(tempFilePath, currentElementLines);
                gitManager.RestoreFileToMatchIndex(currentFile);
                gitManager.UnstageFile(currentFile);
                gitManager.ApplyPatchInReverse(patchPath);
                gitManager.StageFile(repoPath, currentFile);
                System.IO.File.WriteAllLines(repoPath + "/" + currentFile, System.IO.File.ReadAllLines(tempFilePath));
                System.IO.File.Delete(tempFilePath);
            }

            if (System.IO.File.Exists(patchPath))
            {
                System.IO.File.Delete(patchPath);
            }

            diff = unstagedAvailability
                ? new Diff(gitManager.GetUnstagedDiff(currentUnstaged, repoPath, currentFile != "" && unstaged.GetText().Count >= 1))
                : new Diff(gitManager.GetStagedDiff(currentStaged, repoPath));
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
                var result = writer.ToString();
                consoleWrapper.SetCursorPosition(0, 0);
                consoleWrapper.Write(result == null ? "" : result[.. ^1].TrimStart());
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
                elements = interactor.GetPageDataAndSize(width * (windowHeight - 4), width - 2).list;
                leftSide = interactor.GetHighlightedSide(windowHeight - 4, width, width * (windowHeight - 4), true);
            }
            else
            {
                elements = interactor.GetPageDataAndSize(width * (windowHeight - 4), width, true).list;
                leftSide = interactor.GetHighlightedSide(windowHeight - 4, width, width * (windowHeight - 4), true);
            }
        }

        private void DisplayExtendedPage()
        {
            var keyInteractor = new KeyInteractor<IElement>(commits);
            var windowHeight = consoleWrapper.WindowHeight;
            var windowWidth = consoleWrapper.WindowWidth;
            var windowSize = windowHeight * windowWidth;
            var windowSizeTemp = windowSize;
            var isWindowToSmall = windowHeight < 11;
            var commitList = new List<IElement>();
            var leftSideCommits = new List<string>();
            UpdateLeftSideVariables(ref commitList, ref leftSideCommits, isWindowToSmall, commits, windowWidth < 50 && windowHeight < 15 ? windowWidth - 2 : ((int)(0.6 * windowWidth) - 2));
            var toDisplay = isWindowToSmall
                ? new List<string>()
                : UpdateExtendedPage(leftSideCommits, keyInteractor);
            toDisplay.Add("'Esc' to return|Right arrow for next menu");
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
                isWindowToSmall = consoleWrapper.WindowHeight < 11;
                UpdateLeftSideVariables(ref commitList, ref leftSideCommits, isWindowToSmall, commits, new TextWidthSetter(consoleWrapper).SetTextWidth(true));
                toDisplay = isWindowToSmall
                    ? new List<string>()
                    : UpdateExtendedPage(leftSideCommits, keyInteractor);
                toDisplay.Add("'Esc' to return|Right arrow for next menu");
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
            var availableSpace = rowSize * (windowHeight - 4);
            var isWindowToSmall = windowHeight < 4;
            var modifiedFiles = new ModifiedFiles(gitManager.GetCommitModifiedFileNames(currentCommit.Id));
            var leftSideElements = new List<string>();
            var diff = new Diff(gitManager.GetDiffBetweenConsecutiveCommits(previousCommit.Id, currentCommit.Id, repoPath, modifiedFiles[0].GetElement()));
            var page = new List<IElement>();
            var keyInteractor = new KeyInteractor<IElement>(modifiedFiles);
            keyInteractor.KeyInfo = ConsoleKey.NoName;
            UpdateLeftSideVariables(ref page, ref leftSideElements, isWindowToSmall, modifiedFiles, rowSize);
            var rightSide = windowWidth < 50 && windowHeight < 15
                    ? diff.GetHighlightedSide(windowHeight - 4, windowWidth - 2, (windowHeight - 3) * (windowWidth - 2), false)
                    : diff.GetHighlightedSide(windowHeight - 4, windowWidth - (int)(0.6 * windowWidth) - 2, availableSpace, false);
            var toDisplay = isWindowToSmall
                ? new List<string>()
                : UpdateDiffPage(leftSideElements, rightSide, rowSize + 2, keyInteractor);
            toDisplay.Add("'Esc' or Left Arrow to return|Right Arrow for next menu'");
            Write(toDisplay, windowHeight < 4);
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
                isWindowToSmall = windowHeight < 4;
                rowSize = SetTextWidth(false);
                availableSpace = rowSize * (windowHeight - 4);
                windowSize = windowHeight * consoleWrapper.WindowWidth;
                WasWindowResizedSmaller = windowSize < windowSizeTemp;
                windowSizeTemp = windowSize;
                (modifiedFiles.Index, modifiedFiles.Current) = keyInteractor.KeySwitch(leftSideElements, page[^1].Id, page[0].Id, false, windowWidth, page[^1].GetElement().Length);
                UpdateLeftSideVariables(ref page, ref leftSideElements, isWindowToSmall, modifiedFiles, rowSize);
                var filePath = modifiedFiles[modifiedFiles.Current].GetElement();
                diff = new Diff(gitManager.GetDiffBetweenConsecutiveCommits(previousCommit.Id, currentCommit.Id, repoPath, filePath));
                rightSide = windowWidth < 50 && windowHeight < 15
                    ? diff.GetHighlightedSide(windowHeight - 4, windowWidth - 2, (windowHeight - 3) * (windowWidth - 2), false)
                    : diff.GetHighlightedSide(windowHeight - 4, windowWidth - (int)(0.6 * windowWidth) - 2, availableSpace, false);
                toDisplay = isWindowToSmall
                    ? new List<string>()
                    : UpdateDiffPage(leftSideElements, rightSide, rowSize + 2, keyInteractor);
                toDisplay.Add("'Esc' or Left Arrow to return|Right Arrow for next menu'");
                Write(toDisplay, windowHeight < 4);
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
            var availableSpace = rowSize * (windowHeight - 4);
            (var page, var pageSize) = diff.GetPageDataAndSize(availableSpace, rowSize, true);
            var highlightedDiff = diff.GetHighlightedSide(windowHeight - 4, rowSize, availableSpace, true);
            var keyInteractor = new KeyInteractor<IElement>(diff);
            var toDisplay = windowHeight < 4
                ? new List<string>()
                : UpdateDiffPage(leftSideElements, highlightedDiff, rowSize + 2, keyInteractor, true);
            toDisplay.Add("'Esc' or Left Arrow to return");
            Write(toDisplay, windowHeight < 4);
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
                availableSpace = rowSize * (windowHeight - 4);
                windowSize = windowHeight * consoleWrapper.WindowWidth;
                WasWindowResizedSmaller = windowSize < windowSizeTemp;
                windowSizeTemp = windowSize;
                (diff.Index, diff.Current) = keyInteractor.KeySwitch(highlightedDiff, page[^1].Id, page[0].Id, pageSize > availableSpace, rowSize - 2, diff[diff.Current].GetElement().Length);
                (page, pageSize) = diff.GetPageDataAndSize(availableSpace, rowSize, true);
                highlightedDiff = diff.GetHighlightedSide(windowHeight - 4, rowSize, availableSpace, true);
                toDisplay = windowHeight < 3
                ? new List<string>()
                : UpdateDiffPage(leftSideElements, highlightedDiff, rowSize + 2, keyInteractor, true);
                toDisplay.Add("'Esc' or Left Arrow to return");
                Write(toDisplay, windowHeight < 4);
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
                rightSide = new BoxDrawer(diff, "Diff").DrawBox(windowWidth, windowHeight - 2, false, shouldColorBoxLines);
                leftSide = new BoxDrawer(leftSide, "Files").DrawBox(windowWidth, windowHeight - 2);
            }
            else
            {
                rightSide = new BoxDrawer(diff, "Diff").DrawBox(rowSize, windowHeight - 2, false, shouldColorBoxLines);
                leftSide = new BoxDrawer(leftSide, "Files").DrawBox((int)(0.6 * windowWidth), windowHeight - 2);
            }

            return SelectScreen(leftSide, rightSide, keyInteractor);
        }

        private List<string> UpdateHighlightedHunksPage(
            List<string> diff,
            ModifiedFiles unstaged,
            ModifiedFiles staged,
            KeyInteractor<IElement> keyInteractor,
            bool unstagedAvailability)
        {
            var windowHeight = consoleWrapper.WindowHeight;
            var windowWidth = consoleWrapper.WindowWidth;
            var unstagedHeight = (windowHeight - 2) / 2 + (windowHeight - 2) % 2 - 2;
            var unstagedAvailableSpace = ((int)(0.6 * windowWidth) - 2) * unstagedHeight;
            List<string> rightSide;
            List<string> leftSide;
            if (windowHeight < 15 && windowWidth < 50)
            {
                rightSide = new BoxDrawer(diff, "Diff").DrawBox(windowWidth, windowHeight - 2, false, true);

                var unstagedToString = unstaged.GetHighlightedSide(unstagedHeight, windowWidth - 2, (unstagedHeight - 3) * (windowWidth - 2));
                var stagedToString = staged.GetHighlightedSide(
                        windowHeight - ((windowHeight - 2) / 2 + (windowHeight - 2) % 2 - 2) - 2,
                        (int)(0.6 * windowWidth) - 2,
                        ((windowHeight - 2) / 2 - 2) * (int)(0.6 * windowWidth) - 2);
                if (unstagedAvailability)
                {
                    stagedToString[staged.Current] = stagedToString[0].Replace("\u001b[7m", "");
                }
                else
                {
                    unstagedToString[unstaged.Current] = unstagedToString[0].Replace("\u001b[7m", "");
                }

                leftSide = new BoxDrawer(unstagedToString, "Unstaged").DrawBox(windowWidth, unstagedHeight + 2);
                stagedToString = new BoxDrawer(stagedToString, "Staged").DrawBox(windowWidth, windowHeight - unstagedHeight - 4, true);
                leftSide.AddRange(stagedToString);
            }
            else
            {
                var textwidth = new TextWidthSetter(consoleWrapper);
                var leftSideWidth = textwidth.SetTextWidth(true);
                rightSide = new BoxDrawer(diff, "Diff").DrawBox(textwidth.SetTextWidth(false) + 2, windowHeight - 2, false, true);

                var unstagedToString = unstaged.GetHighlightedSide(unstagedHeight, leftSideWidth, unstagedAvailableSpace);
                var stagedToString = staged.GetHighlightedSide(
                        windowHeight - ((windowHeight - 2) / 2 + (windowHeight - 2) % 2 - 2) - 2,
                        (int)(0.6 * windowWidth) - 2,
                        ((windowHeight - 2) / 2 - 2) * (int)(0.6 * windowWidth) - 2);
                if (unstagedAvailability)
                {
                    stagedToString[0] = stagedToString[0].Replace("\u001b[7m", "");
                    stagedToString[^1] = stagedToString[^1].Replace("\u001b[7m", "");
                }
                else
                {
                    unstagedToString[0] = unstagedToString[0].Replace("\u001b[7m", "");
                    unstagedToString[^1] = unstagedToString[^1].Replace("\u001b[7m", "");
                }

                leftSide = new BoxDrawer(unstagedToString, "Unstaged").DrawBox((int)(0.6 * windowWidth), unstagedHeight + 2);
                stagedToString = new BoxDrawer(stagedToString, "Staged").DrawBox((int)(0.6 * windowWidth), windowHeight - unstagedHeight - 4, true);
                leftSide.AddRange(stagedToString);
            }

            return SelectScreen(leftSide, rightSide, keyInteractor);
        }

        private List<string> UpdateExtendedPage(List<string> leftSideCommits, KeyInteractor<IElement> keyInteractor)
        {
            var width = consoleWrapper.WindowWidth;
            var height = consoleWrapper.WindowHeight;
            var rightSide = consoleWrapper.WindowHeight < 15 && consoleWrapper.WindowWidth < 50
                ? commits.FormateUnhighlightedSide(height - 2, width, gitManager)
                : commits.FormateUnhighlightedSide(height - 2, width - (int)(0.6 * width), gitManager);

            return SelectScreen(
                new BoxDrawer(leftSideCommits, "Commits").DrawBox(height < 15 && width < 50 ? width : (int)(0.6 * width), height - 2),
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
            toWrite.InsertRange(0, initialText.Split("\n"));
            return toWrite;
        }

        private List<string> UpdateStatusPage(
            ref List<IElement> unstagedElements,
            ref List<IElement> stagedElements,
            ref List<string> unstagedPageList,
            ref List<string> stagedPageList,
            ref ModifiedFiles unstaged,
            ref ModifiedFiles staged,
            bool isUnstagedInUse)
        {
            var windowHeight = consoleWrapper.WindowHeight;
            var windowWidth = consoleWrapper.WindowWidth;
            var unstagedHeight = (windowHeight - 2) / 2 + (windowHeight - 2) % 2 - 2;
            var stagedHeight = windowHeight - unstagedHeight;
            var unstagedToString = new List<string>();
            var stagedToString = new List<string>();
            var tempList = new List<string>();
            Diff diff;

            if (isUnstagedInUse)
            {
                var keyInteractor = new KeyInteractor<IElement>(unstaged);
                keyInteractor.KeyInfo = key;
                (unstaged.Index, unstaged.Current) = keyInteractor
                    .KeySwitch(unstagedPageList, unstagedElements[^1].Id, unstagedElements[0].Id, false, (int)(0.6 * windowWidth) - 2, unstaged[unstaged.Current].GetElement().Length);
                stagedPageList = staged.GetHighlightedSide(stagedHeight, (int)(0.6 * windowWidth) - 2, stagedHeight * (int)(0.6 * windowWidth) - 2);
                stagedToString.AddRange(stagedPageList);
                stagedToString[0] = stagedToString[0].Replace("\x1b[7m", "");
                stagedToString[^1] = stagedToString[^1].Replace("\x1b[7m", "");
                var unstagedAvailableSpace = ((int)(0.6 * windowWidth) - 2) * unstagedHeight;
                unstagedElements = unstaged.GetPageDataAndSize(unstagedAvailableSpace, (int)(0.6 * windowWidth) - 2).list;
                unstagedPageList = unstaged[unstaged.Current].GetElement() == ""
                       ? new List<string>()
                       : unstaged.GetHighlightedSide(unstagedHeight, (int)(0.6 * windowWidth) - 2, unstagedAvailableSpace);
                var currentUnstaged = unstaged[unstaged.Current].GetElement();
                diff = new Diff(gitManager.GetUnstagedDiff(currentUnstaged, gitManager.GetRepoPath(), currentUnstaged != "" && unstaged.GetText().Count >= 1));
                tempList.AddRange(new BoxDrawer(unstagedPageList, "Unstaged").DrawBox((int)(0.6 * windowWidth), unstagedHeight + 2));
                stagedToString = new BoxDrawer(stagedToString, "Staged").DrawBox((int)(0.6 * windowWidth), (windowHeight - 2) / 2, true);
                tempList.AddRange(stagedToString);
            }
            else
            {
                var keyInteractor = new KeyInteractor<IElement>(staged);
                keyInteractor.KeyInfo = key;
                (staged.Index, staged.Current) = keyInteractor
                    .KeySwitch(stagedPageList, stagedElements[^1].Id, stagedElements[0].Id, false, (int)(0.6 * windowWidth) - 2, staged[staged.Current].GetElement().Length);
                unstagedPageList = unstaged.GetHighlightedSide(unstagedHeight, (int)(0.6 * windowWidth) - 2, unstagedHeight * (int)(0.6 * windowWidth) - 2);
                unstagedToString.AddRange(unstagedPageList);
                unstagedToString[0] = unstagedToString[0].Replace("\x1b[7m", "");
                unstagedToString[^1] = unstagedToString[^1].Replace("\x1b[7m", "");
                var stagedAvailableSpace = ((int)(0.6 * windowWidth) - 2) * stagedHeight;
                stagedElements = staged.GetPageDataAndSize(stagedAvailableSpace, (int)(0.6 * windowWidth) - 2).list;
                stagedPageList = staged[staged.Current].GetElement() == ""
                       ? new List<string>()
                       : staged.GetHighlightedSide(stagedHeight, (int)(0.6 * windowWidth) - 2, stagedAvailableSpace);
                var currentStaged = staged[staged.Current].GetElement();
                diff = new Diff(gitManager.GetStagedDiff(currentStaged, gitManager.GetRepoPath()));
                tempList.AddRange(new BoxDrawer(unstagedToString, "Unstaged").DrawBox((int)(0.6 * windowWidth), unstagedHeight + 2));
                stagedToString = new BoxDrawer(stagedPageList, "Staged").DrawBox((int)(0.6 * windowWidth), (windowHeight - 2) / 2, true);
                tempList.AddRange(stagedToString);
            }

            var diffAvailableSpace = (windowWidth - (int)(0.6 * windowWidth) - 2) * (windowHeight - 4);
            var rightSide = new BoxDrawer(
                    diff[0].GetElement() == ""
                        ? new List<string>()
                        : diff.GetHighlightedSide(windowHeight - 4, windowWidth - (int)(0.6 * windowWidth) - 2, diffAvailableSpace, false), "Diff")
                            .DrawBox(windowWidth - (int)(0.6 * windowWidth), windowHeight - 2);
            return MergeSides(tempList, rightSide);
        }
    }
}