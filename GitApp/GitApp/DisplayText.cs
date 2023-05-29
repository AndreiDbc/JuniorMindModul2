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
        private TextWriter writer;

        public DisplayText(FullCommits commits, TextWriter writer, IProcessRunner processRunner)
        {
            this.commits = commits;
            this.writer = writer;
            this.processRunner = processRunner;
            IsRightSideInUse = false;
        }

        private bool IsRightSideInUse { get; set; }

        public void Display(IConsoleWrapper consoleWrapper, string initialData)
        {
            var commitTextSize = 0;
            var windowWidth = consoleWrapper.WindowWidth;
            var windowSize = consoleWrapper.WindowHeight * windowWidth - windowWidth;
            var initialTextSize = new PageUtilities(initialData).GetOccupancyRate(consoleWrapper.WindowWidth);
            var availableSpace = windowSize - initialTextSize;
            (var page, commitTextSize) = GetPageDataAndSize(commits.Index, availableSpace, consoleWrapper.WindowWidth);
            var temp = new List<Commit>();
            temp.AddRange(page);
            var toWrite = GetCommits(initialData, page);
            var lastPageElementUtilities = new PageUtilities(toWrite[^2]);
            var codeLength = lastPageElementUtilities.GetFormatingCodesLength();
            toWrite[^2] = commitTextSize > availableSpace
                ? lastPageElementUtilities.TruncateText(commitTextSize - availableSpace + codeLength)
                : toWrite[^2];
            toWrite.ForEach(line => writer.WriteLine(line));
            consoleWrapper.Write(writer.ToString().Trim());
            ConsoleKey key;
            do
            {
                key = consoleWrapper.ReadKey().Key;
                consoleWrapper.Clear();
                writer = new StringWriter();
                Write(key, initialData, initialTextSize, ref commitTextSize, ref page, consoleWrapper);
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
                FitTextInsideBox(messageTemp, rightBoxSize - rightBoxMarginDimension + additionalHeight * rightSideLineSize, rightSideLineSize - 2),
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
                if (currentSize >= availableSpace - 3)
                {
                    result.Add(currentSize <= availableSpace && i == list.Count - 1 ? element : elementUtilities.TruncateText(rowSize));
                    break;
                }

                result.Add(element);
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

        private static List<string> DrawCommitBox(List<string> list, int windowHeight, int width)
        {
            const string boxName = "Commits";
            const int lengthToSubstract = 2;
            list.Insert(0, "┌" + boxName + new string('─', width - boxName.Length - lengthToSubstract) + "┐");
            var index = 1;
            do
            {
                if (index < list.Count)
                {
                    var padLength = width + 2 - (list[index].Length - new PageUtilities(list[index]).GetFormatingCodesLength());
                    var toPad = padLength <= 0 ? "\u001b[0m│" : "\u001b[0m│".PadLeft(padLength + 1);
                    list[index] = "\x1b[0m│" + list[index] + toPad;
                }
                else
                {
                    list.Add("\x1b[0m│" + "\u001b[0m│".PadLeft(width + 2));
                }

                index++;
            }
            while (list.Count % windowHeight != 0 && !list[^1].StartsWith("\x1b[0m│"));
            list.Add("\x1b[0m└" + new string('─', width - 2) + "┘");
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
            (line[0].Contains("\u001b[7m") ? additionalLengthForCurrent.Sum()
            : additionalLength.Sum()) -
            toAdd.Length;
            toAdd += line[3].Length + selectAditionalLengthForMessage > rowWidth + selectAditionalLengthForMessage + additionalLengthForMessage
            ? new PageUtilities(line[3]).TruncateText(rowWidth + additionalLengthForMessage + selectAditionalLengthForMessage)
            : line[3];

            return toAdd;
        }

        private void SelectSideAndWrite(KeyInteractor keyInteractor, IConsoleWrapper consoleWrapper, List<string> leftSide, string info, string message, string files)
        {
            var result = new List<string>();
            if (keyInteractor.KeyInfo == ConsoleKey.Spacebar)
            {
                IsRightSideInUse = !IsRightSideInUse;
            }

            result = IsRightSideInUse
                   ? FormateRightSide(info, message, files, consoleWrapper.WindowHeight, consoleWrapper.WindowWidth)
                   : leftSide;

            foreach (var line in result)
            {
                writer.WriteLine(line);
            }

            consoleWrapper.Write(writer.ToString().Trim());
        }

        private void DisplaySplitScreen(
            List<string> leftSide,
            List<string> rightSide,
            IConsoleWrapper consoleWrapper)
        {
            leftSide = DrawCommitBox(leftSide, consoleWrapper.WindowHeight, (int)(0.6 * consoleWrapper.WindowWidth));

            for (int i = 0; i < leftSide.Count; i++)
            {
                writer.Write(i < rightSide.Count ? leftSide[i] : leftSide[i] + "\n");
                if (i < rightSide.Count)
                {
                    writer.WriteLine(rightSide[i]);
                }
            }

            consoleWrapper.Write(writer.ToString().Trim());
        }

        private (List<Commit> list, int textSize) GetPageDataAndSize(int index, int availableSpace, int lineSize)
        {
            var result = new List<Commit>();
            var commitsList = commits.GetText();
            int textSize = 0;
            do
            {
                if (index > commitsList.Count - 1)
                {
                    break;
                }

                result.Add(commitsList.ElementAt(index));
                index++;
                var element = result[^1].GetElement();
                var elementUtilities = new PageUtilities(element);
                textSize += elementUtilities.GetOccupancyRate(lineSize) - elementUtilities.GetFormatingCodesLength();
            }
            while (textSize < availableSpace);

            return (result, textSize);
        }

        private List<string> GetLeftSide(int rowWidth, int windowHeight)
        {
            var additionalLength = new[] { 14, 10, 10 };
            var additionalLengthForCurrent = new[] { 14, 10, 10 };
            var leftSideCommits = AddCommitsToLeftSide(windowHeight - 2);
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

        private List<List<string>> AddCommitsToLeftSide(int limit)
        {
            var result = new List<List<string>>();
            for (int i = 0; i < limit; i++)
            {
                var toAdd = commits.GetElementAt(i + commits.Index);
                result.Add(toAdd.Id.Equals(commits.GetElementAt(commits.Current).Id)
                    ? new List<string>
                    {
                        "\x1b[7m\x1b[48;5;27m" + toAdd.Id,
                        "\x1b[48;5;46m" + toAdd.Author,
                        "\x1b[48;5;196m" + toAdd.Date,
                        "\x1b[0m\x1b[7m" + toAdd.Message
                    }
                    : new List<string>
                    {
                        "\u001b[0m\x1b[38;5;27m" + toAdd.Id,
                        "\x1b[38;5;46m" + toAdd.Author,
                        "\x1b[38;5;196m" + toAdd.Date,
                        "\x1b[0m" + toAdd.Message
                    });
            }

            return result;
        }

        private List<string> GetCommits(string initialText, List<Commit> page)
        {
            writer.WriteLine(initialText);
            var result = new List<string>();
            page.ForEach(commit =>
            {
                result.Add(commit.Id.Equals(commits.GetElementAt(commits.Current).Id)
                        ? "\x1b[7m\x1b[48;5;27m" + commit.Id + " "
                        + "\x1b[48;5;46m" + commit.Author + " "
                        + "\x1b[48;5;196m" + commit.Date + " "
                        + "\x1b[0m\x1b[7m" + commit.Message
                        : "\u001b[0m\x1b[38;5;27m" + commit.Id + " "
                        + "\x1b[38;5;46m" + commit.Author + " "
                        + "\x1b[38;5;196m" + commit.Date + " "
                        + "\u001b[0m" + commit.Message);
            });
            result.Add("\u001b[0mPress 'Esc' to exit.");

            return result;
        }

        private void SelectScreen(IConsoleWrapper consoleWrapper, KeyInteractor keyInteractor, ref List<string> leftSideCommits, string info, string message, string files)
        {
            var height = consoleWrapper.WindowHeight;
            var width = consoleWrapper.WindowWidth;

            if (height < 15 && width < 50)
            {
                leftSideCommits = GetLeftSide((int)(0.25 * (width - 2)), height);
                var leftSide = DrawCommitBox(leftSideCommits, height, width);
                SelectSideAndWrite(keyInteractor, consoleWrapper, leftSide, info, message, files);
            }
            else
            {
                leftSideCommits = GetLeftSide((int)(0.15 * (width - 2)), height);
                DisplaySplitScreen(
                    leftSideCommits,
                    FormateRightSide(info, message, files, height, width - (int)(0.6 * width)),
                    consoleWrapper);
            }
        }

        private void WriteCommitFiles(KeyInteractor keyInteractor, IConsoleWrapper consoleWrapper)
        {
            consoleWrapper.Clear();
            var currentCommit = commits.GetElementAt(commits.Current);
            var info = "Author: " + currentCommit.Author + "\nDate: " + currentCommit.Date + "\nSHA: " + currentCommit.Sha;
            var files = new GitManager("git", processRunner).GetCommitModifiedFileNames(currentCommit.Id);
            var leftSide = new List<string>();
            SelectScreen(consoleWrapper, keyInteractor, ref leftSide, info, currentCommit.Message, files);
            ConsoleKey key;
            do
            {
                key = consoleWrapper.ReadKey().Key;
                consoleWrapper.Clear();
                writer = new StringWriter();
                keyInteractor.KeyInfo = key;
                (commits.Index, commits.Current) = keyInteractor.KeySwitch(leftSide, false, consoleWrapper.WindowWidth);
                currentCommit = commits.GetElementAt(commits.Current);
                info = "Author: " + currentCommit.Author + "\nDate: " + currentCommit.Date + "\nSHA: " + currentCommit.Sha;
                files = new GitManager("git", processRunner).GetCommitModifiedFileNames(currentCommit.Id);
                consoleWrapper.Clear();
                SelectScreen(consoleWrapper, keyInteractor, ref leftSide, info, currentCommit.Message, files);
            }
            while (key != ConsoleKey.Escape);
        }

        private void Write(
            ConsoleKey keyInfo,
            string initialText,
            int initialTextSize,
            ref int commitTextSize,
            ref List<Commit> page,
            IConsoleWrapper consoleWrapper)
        {
            var keyInteractor = new KeyInteractor(commits)
            {
                KeyInfo = keyInfo
            };

            if (keyInfo.Equals(ConsoleKey.Enter))
            {
                WriteCommitFiles(keyInteractor, consoleWrapper);
            }

            consoleWrapper.Clear();
            writer = new StringWriter();
            var windowWidth = consoleWrapper.WindowWidth;
            var windowSize = consoleWrapper.WindowHeight * windowWidth;
            var availableSpace = windowSize - initialTextSize - windowWidth;
            (page, commitTextSize) = GetPageDataAndSize(commits.Index, availableSpace, consoleWrapper.WindowWidth);
            var isPageLargerThanWindow = commitTextSize > availableSpace;
            var commitsToString = new List<string>();
            page.ForEach(line => commitsToString.Add(line.GetElement()));
            (commits.Index, commits.Current) = keyInteractor.KeySwitch(commitsToString, isPageLargerThanWindow, consoleWrapper.WindowWidth);
            var temp = new List<Commit>();
            temp.AddRange(page);
            var toWrite = GetCommits(initialText, page);
            isPageLargerThanWindow = commitTextSize > availableSpace;
            var lastPageElementUtilities = new PageUtilities(toWrite[^2]);
            toWrite[^2] = isPageLargerThanWindow
                ? lastPageElementUtilities.TruncateText(commitTextSize - availableSpace + lastPageElementUtilities.GetFormatingCodesLength())
                : toWrite[^2];
            toWrite.ForEach(line => writer.WriteLine(line));
            consoleWrapper.Write(writer.ToString().Trim());
        }
    }
}