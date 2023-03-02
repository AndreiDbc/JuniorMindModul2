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

        public void Interract()
        {
            ConsoleKey keyinfo;
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
                var keyInteractor = new KeyInteractor(commits);
                keyinfo = keyInteractor.Key;
                windowWidth = Console.WindowWidth;
                windowSize = Console.WindowHeight * windowWidth;
                initialTextSize = initialData.GetTextSize();
                emptySpace = windowSize - initialTextSize - windowWidth;
                (commits.Index, commits.Current) = keyInteractor.KeySwitch(page, isPageLargerThanWindow);
                (page, commitTextSize) = pageOptions.GetPageDataAndSize(commits.Index, emptySpace);
                isPageLargerThanWindow = commitTextSize > emptySpace;
                temp.Clear();
                temp.AddRange(page);
                WriteCommits(initialText, isPageLargerThanWindow ? PageOptions.TruncateText(temp) : page, commits);
            }
            while (!keyinfo.Equals(ConsoleKey.Escape));

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
    }
}
