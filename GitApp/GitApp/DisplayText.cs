using Castle.Components.DictionaryAdapter.Xml;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.ObjectModel;
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
        private readonly CommitsIDs commitsIDs;

        public DisplayText(CommitsIDs commitsIDs)
        {
            this.commitsIDs = commitsIDs;
        }

        public void Display()
        {
            ConsoleKey keyInfo;
            var gitManager = new GitManager("git", new GitProcessRunner());
            var initialData = new InitialData(gitManager);
            var initialText = initialData.GetText();
            var commitTextSize = 0;
            var windowWidth = Console.WindowWidth;
            var windowSize = Console.WindowHeight * windowWidth;
            var initialTextSize = initialData.GetTextSize();
            var emptySpace = windowSize - initialTextSize - windowWidth;
            var commits = new FullCommits(
                new CommitsIDs(gitManager.GetCommitId().Split("\n").ToList()),
                new CommitsAuthorsName(gitManager.GetCommitAuthorName().Split("\n").ToList()),
                new CommitsDates(gitManager.GetCommitDate().Split("\n").ToList()),
                new CommitsMessages(gitManager.GetCommitMessage().Split("\n").ToList()));
            var pageOptions = new PageOptions(commits);
            var page = pageOptions.GetPageDataAndSize(commits.Index, emptySpace).list;
            (page, commitTextSize) = pageOptions.GetPageDataAndSize(commits.Index, emptySpace);
            var isPageLargerThanWindow = commitTextSize > emptySpace;
            var temp = new List<string>();
            temp.AddRange(page);
            WriteCommits(initialText, isPageLargerThanWindow ? PageOptions.TruncateText(temp) : page, commits);
            do
            {
                keyInfo = Console.ReadKey().Key;
                Write(commits, keyInfo, pageOptions, initialData, commitTextSize, ref page);
            }
            while (!keyInfo.Equals(ConsoleKey.Escape));
            Console.ReadKey();
        }

        private static int GetElementEndIndex(string commit, char separator)
        {
            for (int i = 0; i < commit.Length; i++)
            {
                if (commit[i] == separator)
                {
                    return i + 1;
                }
            }

            return -1;
        }

        private void WriteCommits(string initialText, List<string> page, IText text)
        {
            Console.Clear();
            Console.WriteLine(initialText);
            page.ForEach(commit =>
            {
                var id = commit[..^(commit.Length - 7)];
                var nameEndIndex = GetElementEndIndex(commit, ',');
                var dateEndIndex = GetElementEndIndex(commit, ':');
                if (id == commitsIDs.GetIDAtIndex(text.Current))
                {
                    Console.Write(
                        $"\x1b[7m\x1b[48;5;27m{id + " - "}"
                        + $"\x1b[48;5;46m{commit[10..nameEndIndex]}"
                        + $"\x1b[48;5;196m{commit[nameEndIndex..dateEndIndex]}");
                    Console.ResetColor();
                    Console.WriteLine($"\x1b[7m{commit[dateEndIndex..]}\x1b[0m");
                    Console.ResetColor();
                }
                else
                {
                    Console.Write(
                        $"\x1b[38;5;27m{id + " - "}"
                        + $"\x1b[38;5;46m{commit[10..nameEndIndex]}"
                        + $"\x1b[38;5;196m{commit[nameEndIndex..dateEndIndex]}");
                    Console.ResetColor();
                    Console.WriteLine(commit[dateEndIndex..]);
                }
            });
            Console.Write("Press 'Esc' to exit.");
        }

        private void WriteCommitFiles(int index)
        {
            var commitModifiedFiles = new CommitModifiedFiles(commitsIDs.GetIDAtIndex(index));
            var list = commitModifiedFiles.GetText();
            int textSize = 0;
            var page = new List<string>();
            var pageOptions = new PageOptions(commitModifiedFiles);
            ConsoleKey keyinfo;
            (page, textSize) = pageOptions.GetPageDataAndSize(
                commitModifiedFiles.Index,
                Console.WindowHeight * Console.WindowHeight - Console.WindowHeight);
            var temp = new List<string>();
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
                (page, textSize) = pageOptions.GetPageDataAndSize(
                    commitModifiedFiles.Index,
                    Console.WindowHeight * Console.WindowHeight - Console.WindowHeight);
                isPageLargeThanWindow = textSize > Console.WindowHeight * Console.WindowHeight - Console.WindowHeight;
                (isPageLargeThanWindow ? PageOptions.TruncateText(page) : page).ForEach(line => Console.WriteLine(line == list[commitModifiedFiles.Current] ? "\x1b[7m" + line + "\x1b[0m" : line));
                Console.Write("Press 'Esc' to return.");
            }
            while (keyinfo != ConsoleKey.Escape);
        }

        private void Write(IText text, ConsoleKey keyInfo, PageOptions pageOptions, InitialData initialData, int commitTextSize, ref List<string> page)
        {
            var keyInteractor = new KeyInteractor(text, keyInfo);
            var windowWidth = Console.WindowWidth;
            var windowSize = Console.WindowHeight * windowWidth;
            var initialText = initialData.GetText();
            var initialTextSize = initialData.GetTextSize();
            var emptySpace = windowSize - initialTextSize - windowWidth;
            var isPageLargerThanWindow = commitTextSize > emptySpace;
            (text.Index, text.Current) = keyInteractor.KeySwitch(page, isPageLargerThanWindow);
            (page, commitTextSize) = pageOptions.GetPageDataAndSize(text.Index, emptySpace);
            isPageLargerThanWindow = commitTextSize > emptySpace;
            var temp = new List<string>();
            temp.AddRange(page);
            if (keyInfo.Equals(ConsoleKey.Enter))
            {
                WriteCommitFiles(text.Index);
            }

            WriteCommits(initialText, isPageLargerThanWindow ? PageOptions.TruncateText(temp) : page, text);
        }
    }
}
