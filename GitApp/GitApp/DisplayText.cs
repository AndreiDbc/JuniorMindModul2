using Castle.Components.DictionaryAdapter.Xml;
using System;
using System.Collections.Generic;
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
    class DisplayText
    {
        private readonly FullCommits commits;

        public DisplayText(FullCommits commits)
        {
            this.commits = commits;
        }

        public void Display(IConsoleWrapper consoleWrapper)
        {
            var gitManager = new GitManager("git", new GitProcessRunner());
            var initialData = new InitialData(gitManager);
            var initialText = initialData.GetText();
            var commitTextSize = 0;
            var windowWidth = consoleWrapper.WindowWidth;
            var windowSize = consoleWrapper.WindowHeight * windowWidth - windowWidth;
            var initialTextSize = initialData.GetTextSize();
            var availableSpace = windowSize - initialTextSize;
            var pageOptions = new PageUtilities(commits.GetText());
            (var page, commitTextSize) = pageOptions.GetPageDataAndSize(commits.Index, availableSpace);
            var temp = new List<Commit>();
            temp.AddRange(page);
            var toWrite = GetCommits(initialText, page);
            var codeLength = PageUtilities.GetFormatingCodesLength(toWrite[^2]);
            toWrite[^2] = commitTextSize > availableSpace
                ? PageUtilities.TruncateText(toWrite[^2], windowWidth + codeLength)
                : toWrite[^2];
            toWrite.ForEach(line => Console.WriteLine(line));
            ConsoleKey key;
            do
            {
                key = consoleWrapper.ReadKey().Key;
                Write(key, pageOptions, initialData, ref commitTextSize, ref page, consoleWrapper);
            }
            while (!key.Equals(ConsoleKey.Escape));
            Console.ReadKey();
        }

        private static List<string> FormateRightSide(string info, string message, string files, IConsoleWrapper consoleWrapper)
        {
            var width = consoleWrapper.WindowWidth;
            var height = consoleWrapper.WindowHeight;
            var rightSideLineSize = width - (int)(0.6 * width);
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
                currentSize += PageUtilities.GetLinesSizeForRightBox(element, rowSize);
                if (currentSize >= availableSpace - 3)
                {
                    result.Add(currentSize <= availableSpace && i == list.Count - 1 ? element : PageUtilities.TruncateText(element.Trim() + " ", rowSize));
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

        private static List<string> DrawBox(List<string> list, string boxName, int limit, int boxHeight, int windowHeight, int currentSpaceOccupation)
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

        private static List<string> DrawCommitBox(List<string> list, IConsoleWrapper consoleWrapper)
        {
            var limit = (int)(0.6 * consoleWrapper.WindowWidth);
            const string boxName = "Commits";
            const int lengthToSubstract = 2;
            list.Insert(0, "┌" + boxName + new string('─', limit - boxName.Length - lengthToSubstract) + "┐");
            var index = 1;
            do
            {
                if (index < list.Count)
                {
                    var padLength = limit + 6 - (list[index].Length - PageUtilities.GetFormatingCodesLength(list[index]));
                    var toPad = padLength <= 0 ? "\u001b[0m│" : "\u001b[0m│".PadLeft(padLength + 1);
                    list[index] = "\x1b[0m│" + list[index] + toPad;
                }
                else
                {
                    list.Add("\x1b[0m│" + "\u001b[0m│".PadLeft(limit + 6));
                }

                index++;
            }
            while (list.Count % consoleWrapper.WindowHeight != 0 && !list[^1].StartsWith("\x1b[0m│"));
            list.Add("\x1b[0m└" + new string('─', limit - 2) + "┘");
            return list;
        }

        private static void DisplaySplitScreen(List<string> leftSide, List<string> rightSide, IConsoleWrapper consoleWrapper)
        {
            leftSide = DrawCommitBox(leftSide, consoleWrapper);

            for (int i = 0; i < leftSide.Count; i++)
            {
                Console.Write(i < rightSide.Count ? leftSide[i] : leftSide[i] + "\n");
                if (i < rightSide.Count)
                {
                    Console.WriteLine(rightSide[i]);
                }
            }
        }

        private List<string> GetLeftSide(IConsoleWrapper consoleWrapper)
        {
            var leftSideColumnSize = (int)(0.15 * (consoleWrapper.WindowWidth - 2));
            var additionalLength = new[] { 14, 10, 10, 4 };
            var additionalLengthForCurrent = new[] { 14, 10, 10, 8 };
            var leftSideCommits = AddCommitsToLeftSide(consoleWrapper.WindowHeight - 2);
            var leftSideTemp = new List<string>();
            leftSideCommits.ForEach(line =>
            {
                var toAdd = "";
                for (int i = 0; i < line.Count; i++)
                {
                    var element = line[i];
                    if (element.Contains("\u001b[7m"))
                    {
                        toAdd += element.Length > leftSideColumnSize + additionalLengthForCurrent[i]
                        ? PageUtilities.TruncateText(element, leftSideColumnSize + additionalLengthForCurrent[i] - 1) + " "
                        : element + " ";
                    }
                    else if (element.Length > leftSideColumnSize + additionalLength[i])
                    {
                        toAdd += PageUtilities.TruncateText(element, leftSideColumnSize + additionalLength[i] - 1) + " ";
                    }
                    else
                    {
                        toAdd += element + " ";
                    }
                }

                leftSideTemp.Add(toAdd.Trim());
            });

            var leftSide = new List<string>();
            leftSideTemp.ForEach(commit => leftSide.Add(commit));

            return leftSide;
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
            Console.Clear();
            Console.WriteLine(initialText);
            var result = new List<string>();
            page.ForEach(commit =>
            {
                result.Add(commit.Id.Equals(commits.GetElementAt(commits.Current).Id)
                        ? "\x1b[7m\x1b[48;5;27m" + commit.Id + " "
                        + "\x1b[48;5;46m" + commit.Author + " "
                        + "\x1b[48;5;196m" + commit.Date + " "
                        + "\x1b[0m\x1b[7m" + commit.Message + " "
                        : "\u001b[0m\x1b[38;5;27m" + commit.Id + " "
                        + "\x1b[38;5;46m" + commit.Author + " "
                        + "\x1b[38;5;196m" + commit.Date + " "
                        + "\u001b[0m" + commit.Message);
            });
            result.Add("Press 'Esc' to exit.");

            return result;
        }

        private void WriteCommitFiles(KeyInteractor keyInteractor, IConsoleWrapper initialConsoleWrapper)
        {
            Console.Clear();
            var currentCommit = commits.GetElementAt(commits.Current);
            var info = "Author: " + currentCommit.Author + "\nDate: " + currentCommit.Date + "\nSHA: " + currentCommit.Sha;
            var files = new GitManager("git", new GitProcessRunner()).GetCommitModifiedFileNames(currentCommit.Id);
            var leftSide = GetLeftSide(initialConsoleWrapper);
            DisplaySplitScreen(leftSide, FormateRightSide(info, currentCommit.Message, files, initialConsoleWrapper), initialConsoleWrapper);
            ConsoleKey key;
            do
            {
                var consoleWrapper = new ConsoleWrapper();
                key = consoleWrapper.ReadKey().Key;
                keyInteractor.KeyInfo = key;
                (commits.Index, commits.Current) = keyInteractor.KeySwitch(leftSide, false, consoleWrapper.WindowWidth);
                leftSide = GetLeftSide(consoleWrapper);
                currentCommit = commits.GetElementAt(commits.Current);
                info = "Author: " + currentCommit.Author + "\nDate: " + currentCommit.Date + "\nSHA: " + currentCommit.Sha;
                files = new GitManager("git", new GitProcessRunner()).GetCommitModifiedFileNames(currentCommit.Id);
                Console.Clear();
                DisplaySplitScreen(leftSide, FormateRightSide(info, currentCommit.Message, files, consoleWrapper), consoleWrapper);
            }
            while (key != ConsoleKey.Escape);
        }

        private void Write(
            ConsoleKey keyInfo,
            PageUtilities pageOptions,
            InitialData initialData,
            ref int commitTextSize,
            ref List<Commit> page,
            IConsoleWrapper consoleWrapper)
        {
            var keyInteractor = new KeyInteractor(commits)
            {
                KeyInfo = keyInfo
            };
            var windowWidth = consoleWrapper.WindowWidth;
            var windowSize = consoleWrapper.WindowHeight * windowWidth;
            var initialTextSize = initialData.GetTextSize();
            var availableSpace = windowSize - initialTextSize - windowWidth;
            var initialText = initialData.GetText();
            var isPageLargerThanWindow = commitTextSize > availableSpace;
            var commitsToString = new List<string>();
            page.ForEach(line => commitsToString.Add(line.GetElement()));
            (commits.Index, commits.Current) = keyInteractor.KeySwitch(commitsToString, isPageLargerThanWindow, consoleWrapper.WindowWidth);
            (page, commitTextSize) = pageOptions.GetPageDataAndSize(commits.Index, availableSpace);
            var temp = new List<Commit>();
            temp.AddRange(page);

            if (keyInfo.Equals(ConsoleKey.Enter))
            {
                WriteCommitFiles(keyInteractor, consoleWrapper);
            }

            var toWrite = GetCommits(initialText, page);
            isPageLargerThanWindow = commitTextSize > availableSpace;
            toWrite[^2] = isPageLargerThanWindow ? PageUtilities.TruncateText(toWrite[^2], windowWidth + PageUtilities.GetFormatingCodesLength(toWrite[^2])) : toWrite[^2];
            toWrite.ForEach(line => Console.WriteLine(line));
        }
    }
}