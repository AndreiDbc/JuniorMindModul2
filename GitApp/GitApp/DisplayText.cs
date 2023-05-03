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
            result.AddRange(SplitRow(info.Split("\n"), limit - 2));
            result = DrawBox(result, "Info", ref index, limit);
            result.AddRange(SplitRow(message.Split("\n"), limit - 2));
            index++;
            result = DrawBox(result, "Message", ref index, limit);
            result.AddRange(SplitRow(files.Split("\n"), limit - 2));
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
            list.Insert(index, "┌" + boxName + new string('─', limit - boxName.Length - 2) + "┐");
            index++;
            do
            {
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
            while (((list.Count + 1) % (Console.WindowHeight / 3) != 0 && list.Count + 1 < Console.WindowHeight) || !list[^1].StartsWith("\x1b[0m│"));
            list.Add("\x1b[0m└" + new string('─', limit - 2) + "┘");
            return list;
        }

        private void WriteCommits(string initialText, List<Commit> page)
        {
            Console.Clear();
            Console.WriteLine(initialText);
            page.ForEach(commit =>
            {
                if (commit.Id.Equals(commits.GetElementAt(commits.Current).Id))
                {
                    Console.Write(
                        $"\x1b[7m\x1b[48;5;27m{commit.Id + " - "}"
                        + $"\x1b[48;5;46m{commit.Author + ", "}"
                        + $"\x1b[48;5;196m{commit.Date + ": "}");
                    Console.ResetColor();
                    Console.WriteLine($"\x1b[7m{commit.Message}\x1b[0m");
                    Console.ResetColor();
                }
                else
                {
                    Console.Write(
                        $"\x1b[38;5;27m{commit.Id + " - "}"
                        + $"\x1b[38;5;46m{commit.Author + ", "}"
                        + $"\x1b[38;5;196m{commit.Date + ": "}");
                    Console.ResetColor();
                    Console.WriteLine(commit.Message);
                }
            });
            Console.Write("Press 'Esc' to exit.");
        }

        private void WriteCommitFiles()
        {
            var commitModifiedFiles = new CommitModifiedFiles(commits.GetElementAt(commits.Current).Id);
            var list = commitModifiedFiles.GetText();
            int textSize = 0;
            var page = new List<IElement>();
            var pageOptions = new PageUtilities(list, 0);
            var availableSpace = Console.WindowWidth * Console.WindowHeight;
            ConsoleKey keyinfo;
            (page, textSize) = pageOptions.GetPageDataAndSize(commitModifiedFiles.Index, availableSpace);
            var temp = new List<IElement>();
            temp.AddRange(page);
            Console.Clear();
            page.ForEach(line => Console.WriteLine(line == list[commitModifiedFiles.Current] ? "\x1b[7m" + line + "\x1b[0m" : line));
            Console.Write("Press 'Esc' to return.");
            do
            {
                keyinfo = Console.ReadKey().Key;
                Console.Clear();
                var keyInteractor = new KeyInteractor(commitModifiedFiles, keyinfo);
                var isPageLargeThanWindow = textSize > Console.WindowHeight * Console.WindowHeight - Console.WindowHeight;
                (commitModifiedFiles.Index, commitModifiedFiles.Current) = keyInteractor.KeySwitch(page, isPageLargeThanWindow);
                (page, textSize) = pageOptions.GetPageDataAndSize(commitModifiedFiles.Index, availableSpace);
                isPageLargeThanWindow = textSize > Console.WindowHeight * Console.WindowHeight - Console.WindowHeight;
                (isPageLargeThanWindow ? PageUtilities.TruncateText(page) : page).ForEach(line => Console.WriteLine(line == list[commitModifiedFiles.Current] ? "\x1b[7m" + line + "\x1b[0m" : line));
                Console.Write("Press 'Esc' to return.");
            }
            while (keyinfo != ConsoleKey.Escape);
        }

        private void Write(ConsoleKey keyInfo, PageUtilities pageOptions, InitialData initialData, ref int commitTextSize, ref List<IElement> page)
        {
            var keyInteractor = new KeyInteractor(commits, keyInfo);
            var windowWidth = Console.WindowWidth;
            var windowSize = Console.WindowHeight * windowWidth;
            var initialTextSize = initialData.GetTextSize();
            var availableSpace = windowSize - initialTextSize - windowWidth;
            var initialText = initialData.GetText();
            var isPageLargerThanWindow = commitTextSize > availableSpace;
            (commits.Index, commits.Current) = keyInteractor.KeySwitch(page, isPageLargerThanWindow);
            (page, commitTextSize) = pageOptions.GetPageDataAndSize(commits.Index, availableSpace);
            isPageLargerThanWindow = commitTextSize > availableSpace;
            var temp = new List<IElement>();
            temp.AddRange(page);
            if (keyInfo.Equals(ConsoleKey.Enter))
            {
                WriteCommitFiles();
            }

            WriteCommits(initialText, isPageLargerThanWindow ? PageUtilities.TruncateText(temp).Cast<Commit>().ToList() : page.Cast<Commit>().ToList());
        }
    }
}
