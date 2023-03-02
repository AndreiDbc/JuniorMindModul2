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
        private readonly CommitsAuthorsName commitsAuthorsNames;
        private readonly CommitsDates commitsDates;
        private readonly CommitsMessages commitsMessages;

        public DisplayText(
            CommitsIDs commitsIDs,
            CommitsAuthorsName commitsAuthorsNames,
            CommitsDates commitsDates,
            CommitsMessages commitsMessages)
        {
            this.commitsIDs = commitsIDs;
            this.commitsAuthorsNames = commitsAuthorsNames;
            this.commitsDates = commitsDates;
            this.commitsMessages = commitsMessages;
            Current = 0;
            Index = 0;
        }

        private int Current { get; set; }

        private int Index { get; set; }

        public void Interract(string initialText)
        {
            ConsoleKeyInfo keyInfo;
            var windowWidth = Console.WindowWidth;
            var windowSize = Console.WindowHeight * windowWidth;
            var initialTextSize = GetTextSize(initialText.Split("\n"));
            var emptySpace = windowSize - initialTextSize - windowWidth;
            var commitTextSize = 0;
            List<string>? page;
            (page, commitTextSize) = GetCommitsTextAndSize(emptySpace);
            var isPageLargerThanWindow = commitTextSize > emptySpace;
            page = isPageLargerThanWindow
                ? TruncateText(page)
                : page;
            WriteCommits(initialText, page);
            do
            {
                keyInfo = Console.ReadKey();
                windowWidth = Console.WindowWidth;
                windowSize = Console.WindowHeight * windowWidth;
                initialTextSize = GetTextSize(initialText.Split("\n"));
                emptySpace = windowSize - initialTextSize - windowWidth;
                (Index, Current) = SetIndexAndCurrent(keyInfo, page, isPageLargerThanWindow);
                (page, commitTextSize) = GetCommitsTextAndSize(emptySpace);
                isPageLargerThanWindow = commitTextSize > emptySpace;
                page = isPageLargerThanWindow
                    ? TruncateText(page)
                    : page;
                WriteCommits(initialText, page);
            }
            while (!keyInfo.Key.Equals(ConsoleKey.Escape));

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

        private void WriteCommits(string initialText, List<string> page)
        {
            Console.Clear();
            Console.WriteLine(initialText);
            page.ForEach(commit =>
            {
                var id = commit[.. (commit.Length - (commit.Length - 7))];
                var nameEndIndex = GetElementEndIndex(commit, ',');
                var dateEndIndex = GetElementEndIndex(commit, ':');
                if (id == commitsIDs.GetIDAtIndex(Current))
                {
                    Console.Write(
                        $"\x1b[7m\x1b[48;5;27m{id + " - "}"
                        + $"\x1b[48;5;46m{commit[10 .. nameEndIndex]}"
                        + $"\x1b[48;5;196m{commit[nameEndIndex .. dateEndIndex]}");
                    Console.ResetColor();
                    Console.WriteLine($"\x1b[7m{commit[dateEndIndex ..]}\x1b[0m");
                    Console.ResetColor();
                }
                else
                {
                    Console.Write(
                        $"\x1b[38;5;27m{id + " - "}"
                        + $"\x1b[38;5;46m{commit[10 .. nameEndIndex]}"
                        + $"\x1b[38;5;196m{commit[nameEndIndex .. dateEndIndex]}");
                    Console.ResetColor();
                    Console.WriteLine(commit[dateEndIndex ..]);
                }
            });
            Console.Write("Press 'Esc' to exit.");
        }
    }
}
