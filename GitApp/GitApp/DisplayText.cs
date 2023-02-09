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
        private readonly List<List<string>> commitsIDAuthorDateMessage;

        public DisplayText(List<List<string>> commitsIDAuthorDateMessage)
        {
            this.commitsIDAuthorDateMessage = commitsIDAuthorDateMessage;
        }

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
            do
            {
                List<List<string>>? page;
                (page, commitTextSize) = GetPageTextAndSize(index, emptySpace);
                var isPageLargerThanWindow = commitTextSize > emptySpace;
                page = isPageLargerThanWindow
                    ? TruncateText(page, commitTextSize - emptySpace - 3)
                    : page;
                WriteText(initialText, current, page);
                keyInfo = Console.ReadKey();
                (index, current) = keyInfo.Key switch
                {
                    ConsoleKey.DownArrow =>
                        InterractWithDownArrow(page, index, current, isPageLargerThanWindow),
                    ConsoleKey.UpArrow => InterractWithUpArrow(page, index, current),
                    ConsoleKey.PageDown => InterractWithPageDown(page, index, current),
                    ConsoleKey.PageUp => InterractWithPageUp(page, index, current),
                    _ => (index, current)
                };
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

        private static List<List<string>> TruncateText(List<List<string>> page, int size)
        {
            string toAdd = "";
            var unoccupiedRowSize = size - (page[^1][0] + page[^1][1] + page[^1][2]).Length;
            var i = 0;
            do
            {
                toAdd += page[^1][3][i];
                i++;
            }
            while (toAdd.Length != unoccupiedRowSize);
            page[^1].RemoveAt(3);
            page[^1].Add(toAdd + "...");
            return page;
        }

        private (List<List<string>>, int commitTextSize) GetPageTextAndSize(int index, int size)
        {
            var result = new List<List<string>>();
            int commitTextSize = 0;
            do
            {
                if (commitTextSize >= size)
                {
                    break;
                }

                result.Add(new List<string>()
                {
                    commitsIDAuthorDateMessage[index][0] + " - ",
                    commitsIDAuthorDateMessage[index][1] + ", ",
                    commitsIDAuthorDateMessage[index][2] + ": ",
                    commitsIDAuthorDateMessage[index][3]
                });

                commitTextSize += GetTextSize(new[] { result[^1][0] + result[^1][1] + result[^1][2] + result[^1][3] });
                index++;
            }
            while (index <= commitsIDAuthorDateMessage.Count - 1);

            return (result, commitTextSize);
        }

        private void WriteText(string initialText, int current, List<List<string>> page)
        {
            Console.Clear();
            Console.WriteLine(initialText);
            page.ForEach(commit =>
            {
                if (commit[0] == commitsIDAuthorDateMessage[current][0] + " - ")
                {
                    var backgroundColor = Console.BackgroundColor;
                    Console.BackgroundColor = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.Write(commit[0]);
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write(commit[1]);
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write(commit[2]);
                    Console.ForegroundColor = backgroundColor;
                    Console.WriteLine(commit[3]);
                    Console.ResetColor();
                }
                else
                {
                    var foregroundColor = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.Write(commit[0]);
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write(commit[1]);
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write(commit[2]);
                    Console.ForegroundColor = foregroundColor;
                    Console.WriteLine(commit[3]);
                    Console.ResetColor();
                }
            });
            Console.Write("Press 'Esc' to exit.");
        }

        private (int index, int current) InterractWithDownArrow(
            List<List<string>> page, int index, int current, bool isPageLargerThanWindow)
        {
            if (page[^1][0] == commitsIDAuthorDateMessage[^1][0] + " - ")
            {
                return (index, current);
            }

            if (commitsIDAuthorDateMessage[current][0] + " - " == page[^1][0])
            {
                index++;
                index = isPageLargerThanWindow ? index + 1 : index;
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
