using Castle.Components.DictionaryAdapter.Xml;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.ObjectModel;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata.Ecma335;
using System.Reflection.PortableExecutable;
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

        private void WriteCommits(string initialText, int current, List<List<string>> page)
        {
            Console.Clear();
            Console.WriteLine(initialText);
            page.ForEach(commit =>
            {
                if (commit[0] == commitsIDAuthorDateMessage[current][0] + " - ")
                {
                    Console.Write(
                        $"\x1b[7m\x1b[48;5;27m{commit[0]}"
                        + $"\x1b[48;5;46m{commit[1]}"
                        + $"\x1b[48;5;196m{commit[2]}");
                    Console.ResetColor();
                    Console.WriteLine($"\x1b[7m{commit[3]}\x1b[0m");
                    Console.ResetColor();
                }
                else
                {
                    Console.Write(
                        $"\x1b[38;5;27m{commit[0]}"
                        + $"\x1b[38;5;46m{commit[1]}"
                        + $"\x1b[38;5;196m{commit[2]}");
                    Console.ResetColor();
                    Console.WriteLine(commit[3]);
                }
            });
            Console.Write("Press 'Esc' to exit.");
        }

        private (int index, int current) InterractWithDownArrow(
            List<List<string>> page, int index, int current, bool isPageLargerThanWindow)
        {
            if (page[^1][0] == commitsIDAuthorDateMessage[^1][0] + " - ")
            {
                return (index, SetToLastOrDontChange(current + 1));
            }

            if (commitsIDAuthorDateMessage[current][0] + " - " == page[^1][0])
            {
                index++;
                index = isPageLargerThanWindow
                    ? index +
                        (commitsIDAuthorDateMessage[current][0] + " - " +
                        commitsIDAuthorDateMessage[current][1] + ", " +
                        commitsIDAuthorDateMessage[current][2] + ": " +
                        commitsIDAuthorDateMessage[current][3]).Length
                            / Console.WindowWidth
                    : index;
            }

            return (SetToLastOrDontChange(index), SetToLastOrDontChange(current + 1));
        }

        private (int index, int current) InterractWithUpArrow(
            List<List<string>> page, int index, int current)
        {
            if (commitsIDAuthorDateMessage[current][0] + " - " == page[0][0])
            {
                index--;
            }

            return (SetToZeroOrDontChange(index), SetToZeroOrDontChange(current - 1));
        }

        private (int index, int current) InterractWithPageDown(
            List<List<string>> page, int index, int current)
        {
            current = SetToLastOrDontChange(current + page.Count);
            if (page[^1][0] == commitsIDAuthorDateMessage[^1][0] + " - ")
            {
                return (index, current);
            }

            return (SetToLastOrDontChange(index + page.Count), current);
        }

        private int SetToLastOrDontChange(int number)
        {
            return number > commitsIDAuthorDateMessage.Count - 1
                ? commitsIDAuthorDateMessage.Count - 1
                : number;
        }
    }
}
