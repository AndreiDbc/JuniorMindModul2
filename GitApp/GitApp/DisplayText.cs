using Castle.Components.DictionaryAdapter.Xml;
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
        private readonly IProcessRunner processRunner;
        private readonly FullCommits commits;
        private readonly GitManager gitManager;
        private TextWriter writer;

        public DisplayText(FullCommits commits, TextWriter writer, IProcessRunner processRunner, GitManager gitManager)
        {
            this.commits = commits;
            this.writer = writer;
            this.processRunner = processRunner;
            IsRightSideInUse = false;
            this.gitManager = gitManager;
        }

        private bool IsRightSideInUse { get; set; }

        public void Display(IConsoleWrapper consoleWrapper, string initialText)
        {
            var commitTextSize = 0;
            var windowWidth = consoleWrapper.WindowWidth;
            var windowSize = consoleWrapper.WindowHeight * windowWidth;
            var initialTextUtilities = new PageUtilities(initialText);
            var initialTextSize = initialTextUtilities.GetOccupancyRate(windowWidth);
            var page = new List<Commit>();
            var toDisplay = new List<string>();
            ConsoleKey key = ConsoleKey.NoName;
            toDisplay = initialTextSize + new PageUtilities("\nPress 'Esc' to exit.").GetOccupancyRate(windowWidth) >= windowSize
                        ? new List<string>()
                        : UpdatePage(key, initialText, initialTextSize, ref commitTextSize, ref page, consoleWrapper);
            Write(toDisplay, initialTextSize, consoleWrapper, initialText);

            do
            {
                if (consoleWrapper.KeyAvailable())
                {
                    toDisplay = UpdateBuffer(initialText, initialTextSize, ref commitTextSize, ref page, consoleWrapper);
                }
                else
                {
                    key = consoleWrapper.ReadKey().Key;
                }

                writer = new StringWriter();
                windowWidth = consoleWrapper.WindowWidth;
                windowSize = consoleWrapper.WindowHeight * windowWidth;
                initialTextSize = initialTextUtilities.GetOccupancyRate(windowWidth);
                toDisplay = initialTextSize + new PageUtilities("\nPress 'Esc' to exit.").GetOccupancyRate(windowWidth) >= windowSize
                        ? new List<string>()
                        : UpdatePage(key, initialText, initialTextSize, ref commitTextSize, ref page, consoleWrapper);
                Write(toDisplay, initialTextSize, consoleWrapper, initialText);
                Thread.Sleep(100);
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

        private static List<string> FitTextInsideBox(List<string> list, int availableSpace, int rowSize)
        {
            var result = new List<string>();
            var currentSize = 0;
            for (int i = 0; i < list.Count; i++)
            {
                var element = list[i];
                var elementUtilities = new PageUtilities(element);
                currentSize += elementUtilities.GetOccupancyRate(rowSize);
                if (currentSize > availableSpace)
                {
                    break;
                }

                result.Add(currentSize >= availableSpace && i < list.Count - 1
                    ? elementUtilities.TruncateText(rowSize)
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
                    list[index] = "\x1b[0m│" + list[index] + "│".PadLeft(limit - list[index].Length - 1);
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

        private static List<string> GetTextToRedraw(List<string> toWrite, IConsoleWrapper consoleWrapper)
        {
            var result = new List<string>();
            toWrite.ForEach(element =>
            {
                var elementUtilities = new PageUtilities(element);
                var clearedElement = elementUtilities.GetFormatingCodesLengthAndStringWithoutCodes().clearedString;
                var padLength = new PageUtilities(clearedElement).GetOccupancyRate(consoleWrapper.WindowWidth) - clearedElement.Length;
                var toPad = new string(' ', padLength);
                result.Add(clearedElement.Length % consoleWrapper.WindowWidth != 0 ? element + toPad : element);
            });

            return result;
        }

        private static List<string> DrawCommitBox(List<string> list, int windowHeight, int width)
        {
            const string boxName = "Commits";
            const int lengthToSubstract = 2;
            var result = new List<string>();
            result.AddRange(list);
            result.Insert(0, "┌" + boxName + new string('─', width - boxName.Length - lengthToSubstract) + "┐");
            var index = 1;
            do
            {
                if (index < result.Count)
                {
                    var padLength = width + 2 - (result[index].Length - new PageUtilities(result[index]).GetFormatingCodesLengthAndStringWithoutCodes().length);
                    var toPad = padLength <= 0 ? "\u001b[0m│" : "\u001b[0m│".PadLeft(padLength + 1);
                    result[index] = "\x1b[0m│" + result[index] + toPad;
                }
                else
                {
                    result.Add("\x1b[0m│" + "\u001b[0m│".PadLeft(width + 2));
                }

                index++;
            }
            while (result.Count % windowHeight != 0 && !result[^1].StartsWith("\x1b[0m│"));
            result.Add("\x1b[0m└" + new string('─', width - 2) + "┘");
            return result;
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

        private static List<string> DisplaySplitScreen(List<string> leftSide, List<string> rightSide, IConsoleWrapper consoleWrapper)
        {
            leftSide = DrawCommitBox(leftSide, consoleWrapper.WindowHeight, (int)(0.6 * consoleWrapper.WindowWidth));
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

        private List<string> UpdateBuffer(
            string initialText,
            int initialTextSize,
            ref int commitTextSize,
            ref List<Commit> page,
            IConsoleWrapper consoleWrapper)
        {
            var keyList = new List<ConsoleKey>();
            var result = new List<string>();
            while (consoleWrapper.KeyAvailable())
            {
                keyList.Add(consoleWrapper.ReadKey().Key);
            }

            foreach (var keyInUse in keyList)
            {
                result = UpdatePage(keyInUse, initialText, initialTextSize, ref commitTextSize, ref page, consoleWrapper);
            }

            return result;
        }

        private List<string> UpdateBufferForExtendedPage(
            IConsoleWrapper consoleWrapper,
            KeyInteractor keyInteractor,
            ref List<string> leftSideCommits,
            ref List<Commit> commitList)
        {
            var result = new List<string>();
            while (consoleWrapper.KeyAvailable())
            {
                keyInteractor.KeyInfo = consoleWrapper.ReadKey().Key;
                (commits.Index, commits.Current) = keyInteractor
                    .KeySwitch(leftSideCommits, commitList[^1].Id, commitList[0].Id, false, consoleWrapper.WindowWidth);
                commitList = GetCommitsForLeftSide(commits.Index, consoleWrapper.WindowHeight);
                result = GetLeftSide((int)(0.25 * (consoleWrapper.WindowWidth - 2)), commitList);
            }

            return result;
        }

        private List<string> SelectSide(
            KeyInteractor keyInteractor,
            IConsoleWrapper consoleWrapper,
            List<string> leftSide,
            string info,
            string message,
            string files)
        {
            var result = new List<string>();
            if (keyInteractor.KeyInfo == ConsoleKey.Spacebar)
            {
                IsRightSideInUse = !IsRightSideInUse;
            }

            result = IsRightSideInUse
                   ? FormateRightSide(info, message, files, consoleWrapper.WindowHeight, consoleWrapper.WindowWidth)
                   : leftSide;

            return result;
        }

        private (List<Commit> list, int textSize) GetPageDataAndSize(int index, int availableSpace, int lineSize)
        {
            var result = new List<Commit>();
            var commitsList = commits.GetText();
            int textSize = 0;
            do
            {
                if (index > commitsList.Count - 1 || textSize >= availableSpace)
                {
                    break;
                }

                result.Add(commitsList.ElementAt(index));
                index++;
                var element = result[^1].GetElement();
                var elementUtilities = new PageUtilities(element);
                textSize += elementUtilities.GetOccupancyRate(lineSize);
            }
            while (textSize <= availableSpace);

            return (result, textSize);
        }

        private List<Commit> GetCommitsForLeftSide(int index, int windowHeight)
        {
            var result = new List<Commit>();
            var commitsList = commits.GetText();
            var count = 0;
            do
            {
                if (index > commitsList.Count - 1)
                {
                    break;
                }

                result.Add(commitsList.ElementAt(index));
                index++;
                count++;
            }
            while (count != windowHeight - 2);

            return result;
        }

        private List<string> GetLeftSide(int rowWidth, List<Commit> commitList)
        {
            var additionalLength = new[] { 14, 10, 10 };
            var additionalLengthForCurrent = new[] { 14, 10, 10 };
            var leftSideCommits = AddCommitsToLeftSide(commitList);
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

        private List<List<string>> AddCommitsToLeftSide(List<Commit> commitList)
        {
            var result = new List<List<string>>();
            bool doesPageContainCurrent = false;
            var currentCommit = commits.GetElementAt(commits.Current);
            for (int i = 0; i < commitList.Count; i++)
            {
                var toAdd = commitList[i];

                if (toAdd.Id.Equals(currentCommit.Id))
                {
                    doesPageContainCurrent = true;
                }

                result.Add(toAdd.Id.Equals(currentCommit.Id)
                    ? new List<string>
                    {
                        "\x1b[7m\x1b[48;5;27m" + toAdd.Id,
                        "\x1b[48;5;46m" + toAdd.Author,
                        "\x1b[48;5;196m" + toAdd.Date,
                        "\x1b[0m\x1b[7m" + toAdd.Message
                    }
                    : new List<string>
                    {
                        "\x1b[0m\x1b[38;5;27m" + toAdd.Id,
                        "\x1b[38;5;46m" + toAdd.Author,
                        "\x1b[38;5;196m" + toAdd.Date,
                        "\x1b[0m" + toAdd.Message
                    });
            }

            result = SetCurrent(result, doesPageContainCurrent);

            return result;
        }

        private List<string> GetCommits(string initialText, List<Commit> page)
        {
            writer.WriteLine(initialText);
            var result = new List<string>();
            var currentCommit = commits.GetElementAt(commits.Current);
            page.ForEach(commit =>
            {
                result.Add(commit.Id.Equals(currentCommit.Id)
                        ? "\x1b[7m\x1b[48;5;27m" + commit.Id + " "
                        + "\x1b[48;5;46m" + commit.Author + " "
                        + "\x1b[48;5;196m" + commit.Date + " "
                        + "\x1b[0m\x1b[7m" + commit.Message + "\x1b[0m"
                        : "\x1b[0m\x1b[38;5;27m" + commit.Id + " "
                        + "\x1b[38;5;46m" + commit.Author + " "
                        + "\x1b[38;5;196m" + commit.Date + " "
                        + "\x1b[0m" + commit.Message + "\x1b[0m");
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

        private List<string> SelectScreen(
            IConsoleWrapper consoleWrapper,
            KeyInteractor keyInteractor,
            List<string> leftSideCommits,
            List<Commit> commitList,
            string[] data)
        {
            var height = consoleWrapper.WindowHeight;
            var width = consoleWrapper.WindowWidth;
            var leftSide = new List<string>();
            if (height < 15 && width < 50)
            {
                leftSide = DrawCommitBox(leftSideCommits, height, width);
                return SelectSide(keyInteractor, consoleWrapper, leftSide, data[0], data[1], data[2]);
            }

            leftSideCommits = GetLeftSide((int)(0.15 * (width - 2)), commitList);
            leftSide.AddRange(leftSideCommits);

            return DisplaySplitScreen(
                leftSide,
                FormateRightSide(data[0], data[1], data[2], height, width - (int)(0.6 * width)),
                consoleWrapper);
        }

        private void Write(List<string> toWrite, int initialTextSize, IConsoleWrapper consoleWrapper, string initialText = "")
        {
            var width = consoleWrapper.WindowWidth;
            var windowSize = consoleWrapper.WindowHeight * width;
            if (initialTextSize + new PageUtilities("\nPress 'Esc' to exit.").GetOccupancyRate(width) >= windowSize)
            {
                writer = new StringWriter();
                consoleWrapper.Clear();
                consoleWrapper.Write("Increase window size!");
            }
            else
            {
                writer = new StringWriter();
                toWrite.InsertRange(0, initialText.Split("\n"));
                var temp = GetTextToRedraw(toWrite, consoleWrapper);
                temp.ForEach(element => writer.WriteLine(element));
                consoleWrapper.SetCursorPosition(0, 0);
                consoleWrapper.Write(writer.ToString()[..^1]);
            }
        }

        private void DisplayExtendedPage(KeyInteractor keyInteractor, IConsoleWrapper consoleWrapper)
        {
            consoleWrapper.Clear();
            var currentCommit = commits.GetElementAt(commits.Current);
            var info = "Author: " + currentCommit.Author + "\nDate: " + currentCommit.Date + "\nSHA: " + currentCommit.Sha;
            var files = gitManager.GetCommitModifiedFileNames(currentCommit.Id);
            var leftSideCommits = new List<string>();
            var commitList = GetCommitsForLeftSide(commits.Index, consoleWrapper.WindowHeight);
            var data = new[] { info, currentCommit.Message, files };
            var toDisplay = SelectScreen(consoleWrapper, keyInteractor, leftSideCommits, commitList, data);
            Write(toDisplay, 0, consoleWrapper);
            ConsoleKey key = ConsoleKey.NoName;
            do
            {
                if (consoleWrapper.KeyAvailable())
                {
                   leftSideCommits = UpdateBufferForExtendedPage(consoleWrapper, keyInteractor, ref leftSideCommits, ref commitList);
                }
                else
                {
                    key = consoleWrapper.ReadKey().Key;
                    keyInteractor.KeyInfo = key;
                    (commits.Index, commits.Current) = keyInteractor
                        .KeySwitch(leftSideCommits, commitList[^1].Id, commitList[0].Id, false, consoleWrapper.WindowWidth);
                    commitList = GetCommitsForLeftSide(commits.Index, consoleWrapper.WindowHeight);
                    leftSideCommits = GetLeftSide((int)(0.25 * (consoleWrapper.WindowWidth - 2)), commitList);
                }

                toDisplay = UpdateExtendedPage(consoleWrapper, keyInteractor, ref leftSideCommits, commitList, commits.GetElementAt(commits.Current));
                Write(toDisplay, 0, consoleWrapper);
                Thread.Sleep(100);
            }
            while (key != ConsoleKey.Escape);
        }

        private List<string> UpdateExtendedPage(
            IConsoleWrapper consoleWrapper,
            KeyInteractor keyInteractor,
            ref List<string> leftSideCommits,
            List<Commit> commitList,
            Commit currentCommit)
        {
            var data = new[]
            {
                "Author: " + currentCommit.Author + "\nDate: " + currentCommit.Date + "\nSHA: " + currentCommit.Sha,
                currentCommit.Message,
                gitManager.GetCommitModifiedFileNames(currentCommit.Id)
            };
            return SelectScreen(consoleWrapper, keyInteractor, leftSideCommits, commitList, data);
        }

        private List<string> UpdatePage(
            ConsoleKey keyInfo,
            string initialText,
            int initialTextSize,
            ref int commitOcupancyRate,
            ref List<Commit> page,
            IConsoleWrapper consoleWrapper)
        {
            var keyInteractor = new KeyInteractor(commits)
            {
                KeyInfo = keyInfo
            };

            var windowWidth = consoleWrapper.WindowWidth;
            var windowSize = consoleWrapper.WindowHeight * windowWidth;

            if (keyInfo.Equals(ConsoleKey.Enter))
            {
                DisplayExtendedPage(keyInteractor, consoleWrapper);
            }

            var availableSpace = windowSize - initialTextSize - windowWidth;
            var isPageLargerThanWindow = commitOcupancyRate > availableSpace;
            var commitsToString = new List<string>();
            var commitsOcupancyRateExceptLast = 0;
            page.ForEach(line => commitsToString.Add(line.GetElement()));
            (commits.Index, commits.Current) = page.Count == 0
                ? (0, 0)
                : keyInteractor.KeySwitch(commitsToString, page[^1].Id, page[0].Id, isPageLargerThanWindow, consoleWrapper.WindowWidth);
            (page, commitOcupancyRate) = GetPageDataAndSize(commits.Index, availableSpace, consoleWrapper.WindowWidth);
            var currentCommit = commits.GetElementAt(commits.Current);
            commits.Current = page.Any(commit => currentCommit.Id.Equals(commit.Id)) ? commits.Current : commits.Index;
            var toWrite = GetCommits(initialText, page);
            isPageLargerThanWindow = commitOcupancyRate > availableSpace;
            for (int i = 0; i < toWrite.Count - 2; i++)
            {
                var temp = new PageUtilities(toWrite[i]).GetFormatingCodesLengthAndStringWithoutCodes().clearedString;
                commitsOcupancyRateExceptLast += new PageUtilities(temp).GetOccupancyRate(windowWidth);
            }

            var lastPageElementUtilities = new PageUtilities(toWrite[^2][..^4]);
            var truncationSize = availableSpace - commitsOcupancyRateExceptLast + lastPageElementUtilities.GetFormatingCodesLengthAndStringWithoutCodes().length;
            toWrite[^2] = isPageLargerThanWindow
                ? lastPageElementUtilities.TruncateText(truncationSize)
                : toWrite[^2];
            toWrite[^2] += "\u001b[0m";
            return toWrite;
        }
    }
}