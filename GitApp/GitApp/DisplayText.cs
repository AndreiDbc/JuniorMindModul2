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

        public void Display()
        {
            ConsoleKey keyInfo;
            var gitManager = new GitManager("git", new GitProcessRunner());
            var initialData = new InitialData(gitManager);
            var initialText = initialData.GetText();
            var commitTextSize = 0;
            var windowWidth = Console.WindowWidth;
            var windowSize = Console.WindowHeight * windowWidth - windowWidth;
            var initialTextSize = initialData.GetTextSize();
            var availableSpace = windowSize - initialTextSize;
            var pageOptions = new PageUtilities(commits.GetText());
            (var page, commitTextSize) = pageOptions.GetPageDataAndSize(commits.Index, availableSpace);
            var temp = new List<Commit>();
            temp.AddRange(page);
            var toWrite = GetCommits(initialText, page);
            var codeLength = GetFormatingCodeAndLength(toWrite[^2]);
            toWrite[^2] = commitTextSize > availableSpace
                ? PageUtilities.TruncateText(toWrite[^2], windowWidth + codeLength.length)
                : toWrite[^2];
            toWrite.ForEach(line => Console.WriteLine(line));
            do
            {
                keyInfo = Console.ReadKey().Key;
                Write(keyInfo, pageOptions, initialData, ref commitTextSize, ref page);
            }
            while (!keyInfo.Equals(ConsoleKey.Escape));
            Console.ReadKey();
        }

        private static List<string> FormateRightSide(int limit, string info, string message, string files)
        {
            var result = new List<string>();
            int index = 0;
            result.AddRange(SplitRow(info.Split("\n"), limit - 1));
            result = DrawBox(result, "Info", ref index, limit);
            result.AddRange(SplitRow(message.Split("\n"), limit - 1));
            index++;
            result = DrawBox(result, "Message", ref index, limit);
            result.AddRange(SplitRow(files.Split("\n"), limit - 1));
            index++;
            result = DrawBox(result, "Files", ref index, limit);

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

        private static List<string> DrawBox(List<string> list, string boxName, ref int index, int limit)
        {
            list.Insert(index, "┌" + boxName + new string('─', limit - boxName.Length - 1) + "┐");
            index++;
            do
            {
                if (index < list.Count)
                {
                    list[index] = "\x1b[0m│" + list[index] + "│".PadLeft(limit - list[index].Length);
                }
                else
                {
                    list.Add("\x1b[0m│" + "│".PadLeft(limit));
                }

                index++;
            }
            while (((list.Count + 1) % (Console.WindowHeight / 3) != 0 && list.Count + 1 < Console.WindowHeight) || !list[^1].StartsWith("\x1b[0m│"));
            list.Add("\x1b[0m└" + new string('─', limit - 1) + "┘");
            return list;
        }

        private static List<string> DrawCommitBox(List<string> list)
        {
            var limit = (int)(0.6 * Console.WindowWidth);
            const string boxName = "Commits";
            const int lengthToSubstract = 3;
            list.Insert(0, "┌" + boxName + new string('─', limit - boxName.Length - lengthToSubstract) + "┐");
            var index = 1;
            do
            {
                if (index < list.Count)
                {
                    var padLength = limit + 5 - (list[index].Length - GetFormatingCodeAndLength(list[index]).length);
                    var toPad = padLength <= 0 ? "\u001b[0m│" : "\u001b[0m│".PadLeft(padLength + 1);
                    list[index] = "\x1b[0m│" + list[index] + toPad;
                }
                else
                {
                    list.Add("\x1b[0m│" + "\u001b[0m│".PadLeft(limit + 5));
                }

                index++;
            }
            while (list.Count % Console.WindowHeight != 0 && !list[^1].StartsWith("\x1b[0m│"));
            list.Add("\x1b[0m└" + new string('─', limit - 3) + "┘");
            return list;
        }

        private static (int length, IEnumerable<string> codes) GetFormatingCodeAndLength(string element)
        {
            IEnumerable<string> codes = new[] { "\x1b[0m", "\x1b[7m", "\x1b[48;5;27m", "\x1b[48;5;46m", "\x1b[48;5;196m", "\u001b[38;5;27m", "\u001b[38;5;46m", "\u001b[38;5;196m" };
            return (codes.Aggregate(0, (result, code) => element.Contains(code) ? code.Length + result : result), codes.Where(code => element.Contains(code)));
        }

        private static void DisplaySplitScreen(List<string> leftSide, List<string> rightSide)
        {
            leftSide = DrawCommitBox(leftSide);

            for (int i = 0; i < leftSide.Count; i++)
            {
                Console.Write(i < rightSide.Count ? leftSide[i] : leftSide[i] + "\n");
                if (i < rightSide.Count)
                {
                    Console.WriteLine(rightSide[i]);
                }
            }
        }

        private static string FitTextInsideBox(string[] lines, int size)
        {
            var result = "";
            var i = 0;
            do
            {
                result += lines[i] + "\n";
                i++;
            }
            while (i < size);
            result = i != size - 1 ? PageUtilities.TruncateText(result.TrimEnd() + "  ", result.Length + 2) : result.TrimEnd();
            return result;
        }

        private List<string> GetLeftSide()
        {
            var width = Console.WindowWidth;
            var additionalLength = new[] { 14, 10, 9, 4 };
            var additionalLengthForCurrent = new[] { 14, 10, 9, 8 };
            var leftSideColumnSize = (int)(float)(0.15 * width);
            var leftSideCommits = AddCommitsToLeftSide();
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
                        ? PageUtilities.TruncateText(element, leftSideColumnSize + additionalLengthForCurrent[i]) + " "
                        : element + " ";
                    }
                    else if (element.Length > leftSideColumnSize + additionalLength[i])
                    {
                        toAdd += PageUtilities.TruncateText(element, leftSideColumnSize + additionalLength[i]) + " ";
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

        private List<List<string>> AddCommitsToLeftSide()
        {
            var result = new List<List<string>>();
            for (int i = 0; i < Console.WindowHeight - 2; i++)
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

        private void WriteCommitFiles(KeyInteractor keyInteractor)
        {
            Console.Clear();

            var width = Console.WindowWidth;
            var height = Console.WindowHeight;
            var currentCommit = commits.GetElementAt(commits.Current);
            var rightSideLineSize = (int)(0.4 * (float)width);
            var rightBoxSize = (int)(0.33 * rightSideLineSize * height);
            var info = "Author: " + currentCommit.Author + "\nDate: " + currentCommit.Date + "\nSHA: " + currentCommit.Sha;
            info = PageUtilities.GetLinesSizeForRightBox(info, rightSideLineSize) > rightBoxSize
                ? FitTextInsideBox(info.Split("\n"), rightBoxSize / rightSideLineSize)
                : info;
            var files = new GitManager("git", new GitProcessRunner()).GetCommitModifiedFileNames(currentCommit.Id);
            files = PageUtilities.GetLinesSizeForRightBox(files, rightSideLineSize) > rightBoxSize
                ? FitTextInsideBox(files.Split("\n"), rightBoxSize / rightSideLineSize)
                : files;
            var message = PageUtilities.GetLinesSizeForRightBox(currentCommit.Message, rightSideLineSize) > rightBoxSize
                ? FitTextInsideBox(currentCommit.Message.Split("\n"), rightBoxSize / rightSideLineSize)
                : currentCommit.Message;
            var leftSide = GetLeftSide();
            DisplaySplitScreen(leftSide, FormateRightSide(rightSideLineSize, info, message, files));

            do
            {
                keyInteractor.KeyInfo = Console.ReadKey().Key;
                (commits.Index, commits.Current) = keyInteractor.KeySwitch(leftSide, false);
                leftSide = GetLeftSide();
                width = Console.WindowWidth;
                height = Console.WindowHeight;
                rightSideLineSize = (int)(0.4 * (float)width);
                rightBoxSize = (int)(0.33 * rightSideLineSize * height);
                currentCommit = commits.GetElementAt(commits.Current);
                info = "Author: " + currentCommit.Author + "\nDate: " + currentCommit.Date + "\nSHA: " + currentCommit.Sha;
                info = PageUtilities.GetLinesSizeForRightBox(info, rightSideLineSize) > rightBoxSize
                    ? FitTextInsideBox(info.Split("\n"), rightBoxSize / rightSideLineSize)
                    : info;
                files = new GitManager("git", new GitProcessRunner()).GetCommitModifiedFileNames(currentCommit.Id);
                files = PageUtilities.GetLinesSizeForRightBox(files, rightSideLineSize) > rightBoxSize
                    ? FitTextInsideBox(files.Split("\n"), rightBoxSize / rightSideLineSize)
                    : files;
                message = PageUtilities.GetLinesSizeForRightBox(currentCommit.Message, rightSideLineSize) > rightBoxSize
                    ? FitTextInsideBox(currentCommit.Message.Split("\n"), rightBoxSize / rightSideLineSize)
                    : currentCommit.Message;
                Console.Clear();
                DisplaySplitScreen(leftSide, FormateRightSide(rightSideLineSize, info, message, files));
            }
            while (keyInteractor.KeyInfo != ConsoleKey.Escape);
        }

        private void Write(ConsoleKey keyInfo, PageUtilities pageOptions, InitialData initialData, ref int commitTextSize, ref List<Commit> page)
        {
            var keyInteractor = new KeyInteractor(commits)
            {
                KeyInfo = keyInfo
            };
            var windowWidth = Console.WindowWidth;
            var windowSize = Console.WindowHeight * windowWidth;
            var initialTextSize = initialData.GetTextSize();
            var availableSpace = windowSize - initialTextSize - windowWidth;
            var initialText = initialData.GetText();
            var isPageLargerThanWindow = commitTextSize > availableSpace;
            var commitsToString = new List<string>();
            page.ForEach(line => commitsToString.Add(line.GetElement()));
            (commits.Index, commits.Current) = keyInteractor.KeySwitch(commitsToString, isPageLargerThanWindow);
            (page, commitTextSize) = pageOptions.GetPageDataAndSize(commits.Index, availableSpace);
            var temp = new List<Commit>();
            temp.AddRange(page);

            if (keyInfo.Equals(ConsoleKey.Enter))
            {
                WriteCommitFiles(keyInteractor);
            }

            var toWrite = GetCommits(initialText, page);
            isPageLargerThanWindow = commitTextSize > availableSpace;
            toWrite[^2] = isPageLargerThanWindow ? PageUtilities.TruncateText(toWrite[^2], windowWidth + GetFormatingCodeAndLength(toWrite[^2]).length) : toWrite[^2];
            toWrite.ForEach(line => Console.WriteLine(line));
        }
    }
}
