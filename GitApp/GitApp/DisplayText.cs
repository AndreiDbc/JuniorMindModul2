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
            var index = 0;
            var current = index;
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
            WriteCommits(initialText, current, page);
            do
            {
                keyInfo = Console.ReadKey();
                windowWidth = Console.WindowWidth;
                windowSize = Console.WindowHeight * windowWidth;
                initialTextSize = GetTextSize(initialText.Split("\n"));
                emptySpace = windowSize - initialTextSize - windowWidth;
                (index, current) = SetIndexAndCurrent(keyInfo, page, index, current, isPageLargerThanWindow);
                (page, commitTextSize) = GetCommitsTextAndSize(emptySpace);
                isPageLargerThanWindow = commitTextSize > emptySpace;
                page = isPageLargerThanWindow
                    ? TruncateText(page)
                    : page;
                WriteCommits(initialText, current, page);
            }
            while (!keyInfo.Key.Equals(ConsoleKey.Escape));

            Console.ReadKey();
        }

        private static int GetStringEndIndex(string commit, char separator)
        {
            for (int i = 0; i < commit.Length; i++)
            {
                if (commit[i] == separator)
                {
                    return i;
                }
            }

            return -1;
        }

        private static (int index, int current) InterractWithPageUp(
            List<List<string>> page, int index, int current)
        {
            return (SetToZeroOrDontChange(index - page.Count),
                SetToZeroOrDontChange(current - page.Count));
        }

        private static int SetToZeroOrDontChange(int number)
        {
            return number < 0 ? 0 : number;
        }

        private static int GetTextSize(string[] text)
        {
            var textSize = 0;
            foreach (var phrase in text)
            {
                textSize += phrase.Length / Console.WindowWidth * Console.WindowWidth
                    + (phrase.Length % Console.WindowWidth == 0 ? 0 : 1) * Console.WindowWidth;
            }

            return textSize;
        }

        private static List<string> TruncateText(List<string> page)
        {
            string toAdd = "";
            var i = 0;
            do
            {
                toAdd += page[^1][i];
                i++;
            }
            while (toAdd.Length != Console.WindowWidth - 3);
            page.RemoveAt(page.Count - 1);
            page.Add(toAdd + "...");
            return page;
        }

        private (int index, int current) SetIndexAndCurrent(
            ConsoleKeyInfo keyInfo, List<List<string>> page, int index, int current, bool isPageLargerThanWindow)
        {
            return keyInfo.Key switch
            {
                ConsoleKey.DownArrow =>
                    InterractWithDownArrow(page, index, current, isPageLargerThanWindow),
                ConsoleKey.UpArrow => InterractWithUpArrow(page, index, current),
                ConsoleKey.PageDown => InterractWithPageDown(page, index, current),
                ConsoleKey.PageUp => InterractWithPageUp(page, index, current),
                _ => (index, current)
            };
        }

        private string InterractWithEnter(int current)
        {
            return new GitManager("git").GetCommitModifiedFileNames(commitsIDAuthorDateMessage[current][0]);
        }

        private (List<string>, int commitTextSize) GetCommitsTextAndSize(int size)
        {
            var result = new List<string>();
            var temp = Index;
            int commitTextSize = 0;
            do
            {
                if (commitTextSize >= size)
                {
                    break;
                }

                result.Add(
                    commitsIDs.GetIDAtIndex(temp) + " - " +
                    commitsAuthorsNames.GetNameAtIndex(temp) + ", " +
                    commitsDates.GetDateAtIndex(temp) + ": " +
                    commitsMessages.GetMessageAtIndex(temp));

                commitTextSize += GetTextSize(new[] { result[^1] });
                temp++;
            }
            while (temp <= commitsIDs.Count - 1);

            return (result, commitTextSize);
        }

        private void WriteCommits(string initialText, int current, List<string> page)
        {
            Console.Clear();
            Console.WriteLine(initialText);
            page.ForEach(commit =>
            {
                var id = commit[.. (commit.Length - (commit.Length - 5))];
                var nameEndIndex = GetStringEndIndex(commit, ',');
                var dateEndIndex = GetStringEndIndex(commit, ':');
                if (id == commitsIDs.GetIDAtIndex(current) + " - ")
                {
                    Console.Write(
                        $"\x1b[7m\x1b[48;5;27m{id + " - "}"
                        + $"\x1b[48;5;46m{commit[8.. nameEndIndex]}"
                        + $"\x1b[48;5;196m{commit[(nameEndIndex + 1) .. dateEndIndex]}");
                    Console.ResetColor();
                    Console.WriteLine($"\x1b[7m{commit[(dateEndIndex + 1) ..]}\x1b[0m");
                    Console.ResetColor();
                }
                else
                {
                    Console.Write(
                        $"\x1b[38;5;27m{id + " - "}"
                        + $"\x1b[38;5;46m{commit[8 .. nameEndIndex]}"
                        + $"\x1b[38;5;196m{commit[(nameEndIndex + 1) .. dateEndIndex]}");
                    Console.ResetColor();
                    Console.WriteLine(commit[(dateEndIndex + 1) ..]);
                }
            });
            Console.Write("Press 'Esc' to exit.");
        }

        private void InterractWithDownArrow(List<string> page, bool isPageLargerThanWindow)
        {
            var lastCommit = page[^1];
            var lastCommitId = lastCommit[.. (lastCommit.Length - (lastCommit.Length - 5))];
            var currentID = commitsIDs.GetIDAtIndex(commitsIDs.Count - 1);
            if (lastCommitId == currentID)
            {
                Current = SetToLastOrDontChange(Current + 1);
            }

            if (currentID == lastCommitId)
            {
                Index++;
                Index = isPageLargerThanWindow
                    ? Index +
                        ((currentID + " - " +
                        commitsAuthorsNames.GetNameAtIndex(Current) + ", " +
                        commitsDates.GetDateAtIndex(Current) + ": " +
                        commitsMessages.GetMessageAtIndex(Current)).Length
                            / Console.WindowWidth)
                    : Index;
            }

            Index = SetToLastOrDontChange(Index);
            Current = SetToLastOrDontChange(Current + 1);
        }

        private void InterractWithUpArrow(List<string> page)
        {
            var firstCommit = page[0];
            if (commitsIDs.GetIDAtIndex(Current) + " - "
                == firstCommit[.. (firstCommit.Length - (firstCommit.Length - 5))])
            {
                Index--;
            }

            Index = SetToZeroOrDontChange(Index);
            Current = SetToZeroOrDontChange(Current - 1);
        }

        private void InterractWithPageDown(List<string> page)
        {
            Current = SetToLastOrDontChange(Current + page.Count);
            var lastCommit = page[^1];
            if (lastCommit[.. (lastCommit.Length - lastCommit.Length - 5)]
                == commitsIDs.GetIDAtIndex(commitsIDs.Count - 1))
            {
                return;
            }

            Index = SetToLastOrDontChange(Index + page.Count);
        }

        private int SetToLastOrDontChange(int number)
        {
            return number > commitsIDAuthorDateMessage.Count - 1
                ? commitsIDAuthorDateMessage.Count - 1
                : number;
        }
    }
}
