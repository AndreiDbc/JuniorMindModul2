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
            IsRightSideInUse = true;
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

        private static List<string> FormateRightSide(string info, string message, string files, int height, int rightSideLineSize)
        {
            int boxHeight = height / 3;
            var rightBoxSize = rightSideLineSize * boxHeight;
            var rightBoxMarginDimension = 2 * boxHeight + 2 * (rightSideLineSize - 2);
            var result = new List<string>();
            var infoTemp = SplitRow(info.Split("\n"), rightSideLineSize - 2);
            var infoBox = DrawBox(
                FitTextInsideBox(infoTemp, rightBoxSize - rightBoxMarginDimension, rightSideLineSize - 2),
                "Info",
                rightSideLineSize,
                boxHeight,
                height,
                0);
            result.AddRange(infoBox);
            var messageTemp = SplitRow(message.Split("\n"), rightSideLineSize - 2);
            var additionalHeight = 0;
            additionalHeight += height % 3 == 0 ? 0 : height % 3;
            var messageBox = DrawBox(
                FitTextInsideBox(messageTemp, rightBoxSize - rightBoxMarginDimension + additionalHeight * (rightSideLineSize - 2), rightSideLineSize - 2),
                "Message",
                rightSideLineSize,
                boxHeight + additionalHeight,
                height,
                infoBox.Count);
            result.AddRange(messageBox);
            var filesTemp = SplitRow(files.Split("\n"), rightSideLineSize - 2);
            var filesBox = DrawBox(
                FitTextInsideBox(filesTemp, rightBoxSize - rightBoxMarginDimension, rightSideLineSize - 2),
                "Files",
                rightSideLineSize,
                boxHeight,
                height,
                infoBox.Count + messageBox.Count);
            result.AddRange(filesBox);

            return result;
        }

        private static (List<IElement> list, int textSize) GetPageDataAndSize(
            IInteractor<IElement> interactor,
            int index,
            int availableSpace,
            int lineSize,
            bool isElementCommit)
        {
            var result = new List<IElement>();
            var commitsList = interactor.GetText();
            int textSize = 0;
            do
            {
                if (index > commitsList.Count - 1 || textSize >= availableSpace)
                {
                    break;
                }

                result.Add(commitsList[index]);
                index++;
                var element = isElementCommit ? result[^1].GetElement().Split("\n") : new[] { result[^1].GetElement() };
                var elementUtilities = isElementCommit
                    ? new PageUtilities(element[0] + " " + element[1] + " " + element[2] + " " + element[3])
                    : new PageUtilities(element[0]);
                textSize += elementUtilities.GetOccupancyRate(lineSize);
            }
            while (textSize <= availableSpace);

            return (result, textSize);
        }

        private static List<string> DrawLeftBox(List<string> list, int windowHeight, int width, string boxName)
        {
            const int lengthToSubstract = 2;
            var result = new List<string>();
            result.AddRange(list);
            result.Insert(0, "┌" + boxName + new string('─', width - boxName.Length - lengthToSubstract) + "┐");
            var index = 1;
            do
            {
                if (index < result.Count)
                {
                    var padLength = width + 2 - (result[index].Length - new PageUtilities(result[index]).FormatingCodesStringUtilities().length);
                    var toPad = padLength <= 0 ? "\u001b[0m│" : "\u001b[0m│".PadLeft(padLength + 1);
                    result[index] = "\x1b[0m│" + result[index] + toPad;
                }
                else
                {
                    result.Add("\x1b[0m│" + "\u001b[0m│".PadLeft(width + 3));
                }

                index++;
            }
            while (result.Count <= windowHeight - 2 || !result[^1].StartsWith("\x1b[0m│"));
            result.Add("\x1b[0m└" + new string('─', width - 2) + "┘");
            return result;
        }

        private static List<string> FitTextInsideBox(List<string> list, int availableSpace, int rowSize)
        {
            var result = new List<string>();
            var currentSize = 0;
            for (int i = 0; i < list.Count; i++)
            {
                var element = list[i];
                var elementUtilities = new PageUtilities(element);
                var clearedElement = elementUtilities.FormatingCodesStringUtilities().clearedString;
                var clearedElementUtilities = new PageUtilities(clearedElement);
                currentSize += clearedElementUtilities.GetOccupancyRate(rowSize);
                if (currentSize > availableSpace)
                {
                    break;
                }

                result.Add(currentSize >= availableSpace && i < list.Count - 1
                    ? elementUtilities.TruncateText(rowSize) + "\x1b[0m"
                    : element);
            }

            return result;
        }

        private static List<string> SplitRow(string[] data, int limit)
        {
            var result = new List<string>();
            for (int i = 0; i < data.Length; i++)
            {
                for (int j = 0, k = 1; j < data[i].Length; j += limit, k++)
                {
                    result.Add(data[i][j..Math.Min(data[i].Length, limit * k)]);
                }
            }

            return result;
        }

        private static List<string> DrawBox(
            List<string> list,
            string boxName,
            int limit,
            int boxHeight,
            int windowHeight,
            int currentSpaceOccupation)
        {
            int index = 1;
            list.Insert(0, "┌" + boxName + new string('─', limit - boxName.Length - 2) + "┐");
            do
            {
                if (currentSpaceOccupation + 1 == windowHeight)
                {
                    break;
                }

                if (index < list.Count)
                {
                    list[index] = "\x1b[0m│" +
                        list[index] +
                        "│".PadLeft(limit - list[index].Length - 1 + new PageUtilities(list[index]).FormatingCodesStringUtilities().length);
                }
                else
                {
                    list.Add("\x1b[0m│" + "│".PadLeft(limit - 1));
                }

                index++;
            }
            while (index + 1 != boxHeight);
            list.Add("\x1b[0m└" + new string('─', limit - 2) + "┘");
            return list;
        }

        private static string GetMessage(
            List<string> line,
            int rowWidth,
            int[] additionalLengthForCurrent,
            int[] additionalLength,
            string toAdd)
        {
            var selectAditionalLengthForMessage = line[0].Contains("\u001b[7m") ? 8 : 4;
            var additionalLengthForMessage = rowWidth * 3 +
            (line[0].Contains("\u001b[7m")
                ? additionalLengthForCurrent.Sum()
                : additionalLength.Sum()) -
            toAdd.Length;
            toAdd += line[3].Length + selectAditionalLengthForMessage > rowWidth + selectAditionalLengthForMessage + additionalLengthForMessage
                ? new PageUtilities(line[3]).TruncateText(rowWidth + additionalLengthForMessage + selectAditionalLengthForMessage)
                : line[3];

            return toAdd;
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

        private List<string> SelectSide(List<string> leftSide, List<string> rightSide)
        {
            IsRightSideInUse = !IsRightSideInUse;
            return IsRightSideInUse ? rightSide : leftSide;
        }

        private List<IElement> GetCommitsForLeftSide(int index, int windowHeight)
        {
            var result = new List<IElement>();
            var commitsList = commits.GetText();
            var count = 0;
            do
            {
                if (index > commitsList.Count - 1)
                {
                    break;
                }

                result.Add(commitsList[index]);
                index++;
                count++;
            }
            while (count != windowHeight - 2);

            return result;
        }

        private List<string> GetLeftSide(int rowWidth, List<IElement> commitList, IElement currentCommit)
        {
            var additionalLength = new[] { 14, 10, 10 };
            var additionalLengthForCurrent = new[] { 14, 10, 10 };
            var leftSideCommits = AddCommitsToLeftSide(commitList, currentCommit);
            var leftSideTemp = new List<string>();
            leftSideCommits.ForEach(line =>
            {
                var toAdd = "";
                for (int i = 0; i < line.Count - 1; i++)
                {
                    var element = line[i];
                    var elementUtilities = new PageUtilities(element);
                    if (line[0].Contains("\u001b[7m"))
                    {
                        toAdd += element.Length + 1 > rowWidth + additionalLengthForCurrent[i]
                        ? elementUtilities.TruncateText(rowWidth + additionalLengthForCurrent[i] - 1) + " "
                        : element + " ";
                    }
                    else if (element.Length + 1 > rowWidth + additionalLength[i])
                    {
                        toAdd += elementUtilities.TruncateText(rowWidth + additionalLength[i] - 1) + " ";
                    }
                    else
                    {
                        toAdd += element + " ";
                    }
                }

                leftSideTemp.Add(GetMessage(line, rowWidth, additionalLengthForCurrent, additionalLength, toAdd).Trim());
            });

            return leftSideTemp;
        }

        private List<List<string>> AddCommitsToLeftSide(List<IElement> commitList, IElement currentCommit)
        {
            var result = new List<List<string>>();
            bool doesPageContainCurrent = false;
            for (int i = 0; i < commitList.Count; i++)
            {
                var toAdd = commitList[i];
                var toAddDetails = toAdd.GetElement().Split("\n");
                if (toAdd.Id.Equals(currentCommit.Id))
                {
                    doesPageContainCurrent = true;
                }

                result.Add(toAdd.Id.Equals(currentCommit.Id)
                    ? new List<string>
                    {
                        "\x1b[7m\x1b[48;5;27m" + toAddDetails[0],
                        "\x1b[48;5;46m" + toAddDetails[1],
                        "\x1b[48;5;196m" + toAddDetails[2],
                        "\x1b[0m\x1b[7m" + toAddDetails[3]
                    }
                    : new List<string>
                    {
                        "\x1b[0m\x1b[38;5;27m" + toAddDetails[0],
                        "\x1b[38;5;46m" + toAddDetails[1],
                        "\x1b[38;5;196m" + toAddDetails[2],
                        "\x1b[0m" + toAddDetails[3]
                    });
            }

            result = SetCurrent(result, doesPageContainCurrent);

            return result;
        }

        private List<string> GetCommits(List<IElement> page)
        {
            var result = new List<string>();
            var currentCommit = commits[commits.Current];
            page.ForEach(commit =>
            {
                var commitDetails = commit.GetElement().Split("\n");
                result.Add(commit.Id.Equals(currentCommit.Id)
                        ? "\x1b[7m\x1b[48;5;27m" + commitDetails[0] + " "
                        + "\x1b[48;5;46m" + commitDetails[1] + " "
                        + "\x1b[48;5;196m" + commitDetails[2] + " "
                        + "\x1b[0m\x1b[7m" + commitDetails[3] + "\x1b[0m"
                        : "\x1b[0m\x1b[38;5;27m" + commitDetails[0] + " "
                        + "\x1b[38;5;46m" + commitDetails[1] + " "
                        + "\x1b[38;5;196m" + commitDetails[2] + " "
                        + "\x1b[0m" + commitDetails[3] + "\x1b[0m");
            });
            result.Add("\u001b[0mPress 'Esc' to exit.");

            return result;
        }

        private List<List<string>> SetCurrent(List<List<string>> list, bool doesPageContainCurrent)
        {
            if (!doesPageContainCurrent)
            {
                var temp = list[0][0].ToCharArray();
                temp[2] = '7';
                temp[6] = '4';
                list[0][0] = new string(temp);
                temp = list[0][1].ToCharArray();
                temp[2] = '4';
                list[0][1] = new string(temp);
                temp = list[0][2].ToCharArray();
                temp[2] = '4';
                list[0][2] = new string(temp);
                list[0][3] = list[0][3].Insert(4, "\x1b[7m");
                commits.Current = commits.Index;
            }

            return list;
        }

        private List<string> SelectScreen(List<string> leftSide, List<string> rightSide)
        {
            var height = consoleWrapper.WindowHeight;
            var width = consoleWrapper.WindowWidth;
            if (height < 15 && width < 50)
            {
                return SelectSide(leftSide, rightSide);
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

        private void UpdateLeftSideVariables(ref List<IElement> elements, ref List<string> leftSide, bool isWindowToSmall, IInteractor<IElement> interactor = null)
        {
            var windowWidth = consoleWrapper.WindowWidth;
            var windowHeight = consoleWrapper.WindowHeight;
            if (isWindowToSmall)
            {
                return;
            }

            if (interactor == null)
            {
                elements = GetCommitsForLeftSide(commits.Index, consoleWrapper.WindowHeight);
                leftSide = consoleWrapper.WindowHeight < 15 && consoleWrapper.WindowWidth < 50
                    ? GetLeftSide((int)(0.25 * (consoleWrapper.WindowWidth - 2)), elements, commits[commits.Current])
                    : GetLeftSide((int)(0.15 * (consoleWrapper.WindowWidth - 2)), elements, commits[commits.Current]);
            }
            else
            {
                elements = GetPageDataAndSize(interactor, interactor.Index, ((int)(0.6 * windowWidth) - 2) * (windowHeight - 2), (int)(0.6 * windowWidth) - 2, false).list;
                leftSide = GetLeftSideModifiedFiles(interactor, interactor.Current, interactor.Index);
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
            UpdateLeftSideVariables(ref commitList, ref leftSideCommits, isWindowToSmall);
            var toDisplay = isWindowToSmall
                ? new List<string>()
                : UpdateExtendedPage(leftSideCommits);
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
                UpdateLeftSideVariables(ref commitList, ref leftSideCommits, isWindowToSmall);
                toDisplay = isWindowToSmall
                ? new List<string>()
                : UpdateExtendedPage(leftSideCommits);
                Write(toDisplay, isWindowToSmall);
            }
            while (key != ConsoleKey.Escape);
        }

        private void DisplayDiffPage(IElement currentCommit, IElement previousCommit)
        {
            consoleWrapper.Clear();
            var repoPath = gitManager.GetRepoPath();
            var windowHeight = consoleWrapper.WindowHeight;
            var windowWidth = consoleWrapper.WindowWidth;
            var windowSize = windowHeight * windowWidth;
            var windowSizeTemp = windowSize;
            var rowSize = windowWidth - (int)(0.6 * windowWidth);
            var availableSpace = rowSize * (windowHeight - 2);
            var isWindowToSmall = windowHeight < 3;
            var modifiedFiles = new ModifiedFiles(gitManager.GetCommitModifiedFileNames(currentCommit.Id));
            var leftSideElements = new List<string>();
            var diff = gitManager.GetDiffBetweenConsecutiveCommits(previousCommit.Id, currentCommit.Id, repoPath, modifiedFiles[0].GetElement());
            var page = new List<IElement>();
            UpdateLeftSideVariables(ref page, ref leftSideElements, isWindowToSmall, modifiedFiles);
            var toDisplay = isWindowToSmall
                ? new List<string>()
                : UpdateDiffPage(leftSideElements, diff, rowSize, availableSpace);
            Write(toDisplay, windowHeight < 3);
            ConsoleKey key;
            do
            {
                var keyInteractor = new KeyInteractor<IElement>(modifiedFiles);
                key = consoleWrapper.ReadKey().Key;
                windowHeight = consoleWrapper.WindowHeight;
                windowWidth = consoleWrapper.WindowWidth;
                isWindowToSmall = windowHeight < 3;
                rowSize = windowWidth - (int)(0.6 * windowWidth);
                availableSpace = rowSize * (windowHeight - 2);
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
                    : UpdateDiffPage(leftSideElements, diff, rowSize, availableSpace);
                Write(toDisplay, windowHeight < 3);
            }
            while (key != ConsoleKey.Escape && key != ConsoleKey.LeftArrow);
        }

        private List<string> UpdateDiffPage(List<string> leftSide, string diff, int rowSize, int availableSpace)
        {
            var windowHeight = consoleWrapper.WindowHeight;
            var windowWidth = consoleWrapper.WindowWidth;
            var windowSize = windowHeight * windowWidth;
            var toFit = "";
            List<string> fittedText;
            List<string> rightSide;
            if (windowHeight < 15 && windowWidth < 50)
            {
                var maximumTextSize = (windowHeight - 2) * (windowWidth - 2);
                toFit = new PageUtilities(diff).GetOccupancyRate(windowWidth - 2) > maximumTextSize
                ? new PageUtilities(diff).TruncateText(maximumTextSize)
                : diff;
                fittedText = FitTextInsideBox(SplitRow(toFit.Split("\n"), windowWidth - 2), maximumTextSize, windowWidth - 2);
                rightSide = DrawBox(fittedText, "Diff", windowWidth, windowHeight, windowSize, fittedText.Count);
            }
            else
            {
                toFit = new PageUtilities(diff).GetOccupancyRate(rowSize) > availableSpace
                ? new PageUtilities(diff).TruncateText(availableSpace)
                : diff;
                fittedText = FitTextInsideBox(SplitRow(toFit.Split("\n"), rowSize), availableSpace, rowSize);
                rightSide = DrawBox(fittedText, "Diff", rowSize + 2, windowHeight, availableSpace, fittedText.Count);
            }

            return SelectScreen(DrawLeftBox(leftSide, windowHeight, (int)(0.6 * windowWidth - 2), "Files"), rightSide);
        }

        private List<string> GetLeftSideModifiedFiles(IInteractor<IElement> modifiedFiles, int current, int index)
        {
            var result = new List<string>();
            var modifiedFilesList = modifiedFiles.GetText();
            var currentElement = modifiedFiles[current];
            var occupancyRate = 0;
            var height = consoleWrapper.WindowHeight;
            var width = consoleWrapper.WindowWidth;
            var rowSize = (int)(0.6 * width - 2);
            var availableSpace = rowSize * (height - 2);
            do
            {
                if (occupancyRate >= availableSpace || index > modifiedFilesList.Count - 1)
                {
                    break;
                }

                var toAdd = modifiedFiles[index].GetElement();
                var toAddUtilities = new PageUtilities(toAdd);
                result.Add((currentElement.Id == modifiedFiles[index].Id ? "\x1b[7m" : "\x1b[0m")
                    + (toAdd.Length > rowSize ? toAddUtilities.TruncateText(rowSize - 2) : toAdd));
                occupancyRate += toAddUtilities.GetOccupancyRate(rowSize);
                index++;
            }
            while (occupancyRate <= availableSpace);
            var lastElementUtilities = new PageUtilities(result[^1]);
            result[^1] = occupancyRate > availableSpace
                ? lastElementUtilities.TruncateText(availableSpace - occupancyRate - lastElementUtilities.GetOccupancyRate(rowSize))
                : result[^1];
            return result;
        }

        private List<string> UpdateExtendedPage(List<string> leftSideCommits)
        {
            var currentCommit = commits[commits.Current];
            var currentCommitDetails = currentCommit.GetElement().Split("\n");
            var data = new[]
            {
                "Author: " + currentCommitDetails[1] + "\nDate: " + currentCommitDetails[2] + "\nSHA: " + currentCommitDetails[4],
                currentCommitDetails[3],
                gitManager.GetCommitModifiedFileNames(currentCommit.Id)
            };

            var width = consoleWrapper.WindowWidth;
            var height = consoleWrapper.WindowHeight;
            var rightSide = consoleWrapper.WindowHeight < 15 && consoleWrapper.WindowWidth < 50
                ? FormateRightSide(data[0], data[1], data[2], height, width)
                : FormateRightSide(data[0], data[1], data[2], height, width - (int)(0.6 * width));

            return SelectScreen(
                DrawLeftBox(leftSideCommits, height, height < 15 && width < 50 ? width : (int)(0.6 * width), "Commits"),
                rightSide);
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
            var commitsOcupancyRateExceptLast = 0;
            page.ForEach(line =>
            {
                var commitDetails = line.GetElement().Split("\n");
                commitsToString.Add(commitDetails[0] + " " + commitDetails[1] + " " + commitDetails[2] + " " + commitDetails[3]);
            });
            (commits.Index, commits.Current) = page.Count == 0
                ? (0, 0)
                : keyInteractor.KeySwitch(commitsToString, page[^1].Id, page[0].Id, isPageLargerThanWindow, consoleWrapper.WindowWidth, commits[commits.Current].GetElement().Length - 40);
            (page, commitOcupancyRate) = GetPageDataAndSize(commits, commits.Index, availableSpace, consoleWrapper.WindowWidth, true);
            var currentCommit = commits[commits.Current];
            commits.Current = page.Any(commit => currentCommit.Id.Equals(commit.Id)) ? commits.Current : commits.Index;
            var toWrite = GetCommits(page);
            isPageLargerThanWindow = commitOcupancyRate > availableSpace;
            for (int i = 0; i < toWrite.Count - 2; i++)
            {
                var temp = new PageUtilities(toWrite[i]).FormatingCodesStringUtilities().clearedString;
                commitsOcupancyRateExceptLast += new PageUtilities(temp).GetOccupancyRate(windowWidth);
            }

            var lastPageElementUtilities = new PageUtilities(toWrite[^2][..^4]);
            var truncationSize = availableSpace - commitsOcupancyRateExceptLast + lastPageElementUtilities.FormatingCodesStringUtilities().length;
            toWrite[^2] = isPageLargerThanWindow
                ? lastPageElementUtilities.TruncateText(truncationSize)
                : toWrite[^2];
            toWrite[^2] += "\u001b[0m";
            return toWrite;
        }
    }
}