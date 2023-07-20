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
using System.Text;
using System.Threading.Tasks;

namespace GitApp
{
    public class DisplayText
    {
        private readonly FullCommits commits;
        private readonly GitManager gitManager;
        private readonly IConsoleWrapper consoleWrapper;
        private TextWriter writer;

        public DisplayText(FullCommits commits, TextWriter writer, GitManager gitManager, IConsoleWrapper consoleWrapper)
        {
            this.commits = commits;
            this.writer = writer;
            this.gitManager = gitManager;
            this.consoleWrapper = consoleWrapper;
            IsRightSideInUse = false;
            WasWindowResizedSmaller = false;
        }

        private bool WasWindowResizedSmaller { get; set; }

        private bool IsRightSideInUse { get; set; }

        public void Display(string initialText)
        {
            var commitTextSize = 0;
            var windowWidth = consoleWrapper.WindowWidth;
            var windowSize = consoleWrapper.WindowHeight * windowWidth;
            var initialTextUtilities = new PageUtilities(initialText);
            var initialTextSize = initialTextUtilities.GetOccupancyRate(windowWidth);
            var page = new List<IElement>();
            var toDisplay = new List<string>();
            ConsoleKey key = ConsoleKey.NoName;
            var isWindowToSmall = initialTextSize + new PageUtilities("\nPress 'Esc' to exit.").GetOccupancyRate(windowWidth) >= windowSize;
            toDisplay = isWindowToSmall
                        ? new List<string>()
                        : UpdatePage(key, initialTextSize, ref commitTextSize, ref page);
            Write(toDisplay, isWindowToSmall, initialText);
            var windowSizeTemp = windowSize;

            do
            {
                key = consoleWrapper.ReadKey().Key;
                windowWidth = consoleWrapper.WindowWidth;
                windowSize = consoleWrapper.WindowHeight * windowWidth;
                WasWindowResizedSmaller = windowSize < windowSizeTemp;
                windowSizeTemp = windowSize;
                initialTextSize = initialTextUtilities.GetOccupancyRate(windowWidth);
                isWindowToSmall = initialTextSize + new PageUtilities("\nPress 'Esc' to exit.").GetOccupancyRate(windowWidth) >= windowSize;
                toDisplay = isWindowToSmall
                        ? new List<string>()
                        : UpdatePage(key, initialTextSize, ref commitTextSize, ref page);
                Write(toDisplay, isWindowToSmall, initialText);
            }
            while (!key.Equals(ConsoleKey.Escape));
        }

        private static string RecolorDiffLine(string previous, string current, string next)
        {
            if (current.StartsWith("+") && previous.StartsWith("\u001b[38;5;190m-"))
            {
                return "\x1b[38;5;190m" + current;
            }
            else if (current.StartsWith("+"))
            {
                return "\x1b[38;5;40m" + current;
            }
            else if (current.StartsWith("-") && next.StartsWith("+"))
            {
                return "\u001b[38;5;190m" + current;
            }
            else if (current.StartsWith("-"))
            {
                return "\x1b[38;5;160m" + current;
            }

            return current;
        }

        private static string[] SetColorsToDiffText(string[] lines, int availableSpace, int lineSize)
        {
            var startIndex = lines[3].StartsWith("+++") ? 5 : 6;
            var occupacyRate = 0;
            for (int i = startIndex; i < lines.Length - 1; i++)
            {
                var lineUtilities = new PageUtilities(lines[i]);
                occupacyRate += lineUtilities.GetOccupancyRate(lineSize);
                if (occupacyRate > availableSpace)
                {
                    return lines;
                }

                lines[i] = RecolorDiffLine(lines[i - 1], lines[i], lines[i + 1]);
            }

            var lastElement = lines[^1];
            var secondLastElement = lines[^2];
            lines[^1] = RecolorDiffLine(
                secondLastElement,
                lastElement,
                "");

            return lines;
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

        private void Write(List<string> toWrite, bool isWindowToSmall, string initialText = "")
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
                toWrite.InsertRange(0, initialText.Split("\n"));
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

        private void UpdateLeftSideVariables(ref List<IElement> elements, ref List<string> leftSide, bool isWindowToSmall, IInteractor<IElement> interactor)
        {
            var windowWidth = consoleWrapper.WindowWidth;
            var windowHeight = consoleWrapper.WindowHeight;
            if (isWindowToSmall)
            {
                return;
            }

            if (windowWidth < 50 && windowHeight < 15)
            {
                elements = interactor.GetPageDataAndSize(((int)(0.6 * windowWidth) - 2) * (windowHeight - 2), (int)(0.6 * windowWidth) - 2).list;
                leftSide = interactor.GetHighlightedSide(windowHeight, windowWidth - 2, (windowWidth - 2) * (windowHeight - 2), true);
            }
            else
            {
                elements = interactor.GetPageDataAndSize(((int)(0.6 * windowWidth) - 2) * (windowHeight - 2), (int)(0.6 * windowWidth) - 2, true).list;
                leftSide = interactor.GetHighlightedSide(windowHeight, (int)(0.6 * windowWidth - 2), (int)(0.6 * windowWidth - 2) * (windowHeight - 2), true);
            }
        }

        private void DisplayExtendedPage(KeyInteractor<IElement> keyInteractor)
        {
            var windowHeight = consoleWrapper.WindowHeight;
            var windowSize = windowHeight * consoleWrapper.WindowWidth;
            var windowSizeTemp = windowSize;
            var isWindowToSmall = windowHeight < 9;
            var commitList = new List<IElement>();
            var leftSideCommits = new List<string>();
            UpdateLeftSideVariables(ref commitList, ref leftSideCommits, isWindowToSmall, commits);
            var toDisplay = isWindowToSmall
                ? new List<string>()
                : UpdateExtendedPage(leftSideCommits, keyInteractor);
            Write(toDisplay, isWindowToSmall);
            ConsoleKey key;
            do
            {
                key = consoleWrapper.ReadKey().Key;
                if (key == ConsoleKey.RightArrow)
                {
                    DisplayDiffPage(commits[commits.Current], commits[commits.Current + 1]);
                }

                keyInteractor.KeyInfo = key;
                (commits.Index, commits.Current) = keyInteractor
                    .KeySwitch(leftSideCommits, commitList[^1].Id, commitList[0].Id, false, consoleWrapper.WindowWidth, commits[commits.Current].GetElement().Length - 40);
                windowSize = windowHeight * consoleWrapper.WindowWidth;
                WasWindowResizedSmaller = windowSize < windowSizeTemp;
                windowSizeTemp = windowSize;
                isWindowToSmall = consoleWrapper.WindowHeight < 9;
                UpdateLeftSideVariables(ref commitList, ref leftSideCommits, isWindowToSmall, commits);
                toDisplay = isWindowToSmall
                ? new List<string>()
                : UpdateExtendedPage(leftSideCommits, keyInteractor);
                Write(toDisplay, isWindowToSmall);
            }
            while (key != ConsoleKey.Escape);
        }

        private void DisplayDiffPage(IElement currentCommit, IElement previousCommit)
        {
            var repoPath = gitManager.GetRepoPath();
            var windowHeight = consoleWrapper.WindowHeight;
            var windowWidth = consoleWrapper.WindowWidth;
            var windowSize = windowHeight * windowWidth;
            var windowSizeTemp = windowSize;
            var rowSize = windowWidth - (int)(0.6 * windowWidth);
            var availableSpace = (rowSize - 2) * (windowHeight - 2);
            var isWindowToSmall = windowHeight < 3;
            var modifiedFiles = new ModifiedFiles(gitManager.GetCommitModifiedFileNames(currentCommit.Id));
            var leftSideElements = new List<string>();
            var diff = gitManager.GetDiffBetweenConsecutiveCommits(previousCommit.Id, currentCommit.Id, repoPath, modifiedFiles[0].GetElement());
            var page = new List<IElement>();
            var keyInteractor = new KeyInteractor<IElement>(modifiedFiles);
            keyInteractor.KeyInfo = ConsoleKey.NoName;
            UpdateLeftSideVariables(ref page, ref leftSideElements, isWindowToSmall, modifiedFiles);
            var toDisplay = isWindowToSmall
                ? new List<string>()
                : UpdateDiffPage(leftSideElements, diff, rowSize, availableSpace, keyInteractor);
            Write(toDisplay, windowHeight < 3);
            ConsoleKey key;
            do
            {
                key = consoleWrapper.ReadKey().Key;
                windowHeight = consoleWrapper.WindowHeight;
                windowWidth = consoleWrapper.WindowWidth;
                isWindowToSmall = windowHeight < 3;
                rowSize = windowWidth - (int)(0.6 * windowWidth);
                availableSpace = (rowSize - 2) * (windowHeight - 2);
                keyInteractor.KeyInfo = key;
                windowSize = windowHeight * consoleWrapper.WindowWidth;
                WasWindowResizedSmaller = windowSize < windowSizeTemp;
                windowSizeTemp = windowSize;
                (modifiedFiles.Index, modifiedFiles.Current) = keyInteractor.KeySwitch(leftSideElements, page[^1].Id, page[0].Id, false, windowWidth, page[^1].GetElement().Length);
                UpdateLeftSideVariables(ref page, ref leftSideElements, isWindowToSmall, modifiedFiles);
                var filePath = modifiedFiles[modifiedFiles.Current].GetElement();
                diff = gitManager.GetDiffBetweenConsecutiveCommits(previousCommit.Id, currentCommit.Id, repoPath, filePath);
                toDisplay = isWindowToSmall
                    ? new List<string>()
                    : UpdateDiffPage(leftSideElements, diff, rowSize, availableSpace, keyInteractor);
                Write(toDisplay, windowHeight < 3);
            }
            while (key != ConsoleKey.Escape && key != ConsoleKey.LeftArrow);
        }

        private List<string> UpdateDiffPage(
            List<string> leftSide,
            string diff,
            int rowSize,
            int availableSpace,
            KeyInteractor<IElement> keyInteractor)
        {
            var windowHeight = consoleWrapper.WindowHeight;
            var windowWidth = consoleWrapper.WindowWidth;
            List<string> rightSide;
            if (windowHeight < 15 && windowWidth < 50)
            {
                var maximumTextSize = (windowHeight - 2) * (windowWidth - 2);
                var coloredDiff = SetColorsToDiffText(diff.Split("\n"), maximumTextSize, windowWidth - 2);
                var splitText = new PageUtilities(coloredDiff).SplitRow(windowWidth - 2);
                rightSide = new BoxDrawer(splitText, "Diff").DrawBox(windowWidth, windowHeight, true);
                leftSide = new BoxDrawer(leftSide, "Files").DrawBox(windowWidth, windowHeight);
            }
            else
            {
                var coloredDiff = SetColorsToDiffText(diff.Split("\n"), availableSpace, rowSize - 2);
                var splitText = new PageUtilities(coloredDiff).SplitRow(rowSize - 2);
                rightSide = new BoxDrawer(splitText, "Diff").DrawBox(rowSize, windowHeight, true);
                leftSide = new BoxDrawer(leftSide, "Files").DrawBox((int)(0.6 * windowWidth), windowHeight);
            }

            return SelectScreen(leftSide, rightSide, keyInteractor);
        }

        private List<string> UpdateExtendedPage(List<string> leftSideCommits, KeyInteractor<IElement> keyInteractor)
        {
            var width = consoleWrapper.WindowWidth;
            var height = consoleWrapper.WindowHeight;
            var rightSide = consoleWrapper.WindowHeight < 15 && consoleWrapper.WindowWidth < 50
                ? commits.FormateUnhighlightedSide(height, width, gitManager)
                : commits.FormateUnhighlightedSide(height, width - (int)(0.6 * width), gitManager);

            return SelectScreen(
                new BoxDrawer(leftSideCommits, "Commits").DrawBox(height < 15 && width < 50 ? width : (int)(0.6 * width), height),
                rightSide,
                keyInteractor);
        }

        private List<string> UpdatePage(
            ConsoleKey keyInfo,
            int initialTextSize,
            ref int commitOcupancyRate,
            ref List<IElement> page)
        {
            var keyInteractor = new KeyInteractor<IElement>(commits)
            {
                KeyInfo = keyInfo
            };

            if (keyInfo.Equals(ConsoleKey.Enter))
            {
                DisplayExtendedPage(keyInteractor);
            }

            var windowWidth = consoleWrapper.WindowWidth;
            var windowSize = consoleWrapper.WindowHeight * windowWidth;
            var availableSpace = windowSize - initialTextSize - windowWidth;
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
            var toWrite = commits.GetHighlightedSide(consoleWrapper.WindowHeight - initialTextSize / windowWidth - 1, windowWidth, availableSpace);
            toWrite.Add("\u001b[0mPress 'Esc' to exit.");

            return toWrite;
        }
    }
}