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
            int index = 0;
            var current = index;
            var size = Console.WindowHeight * Console.WindowWidth;
            int initialTextSize = NoOfNewLines(initialText.Split("\n"));
            do
            {
                var page = GetPage(index, size - initialTextSize - Console.WindowWidth);
                WriteText(initialText, current, page);
                keyInfo = Console.ReadKey();
                (index, current) = keyInfo.Key switch
                {
                    ConsoleKey.DownArrow => InterractWithDownArrow(page, index, current),
                    ConsoleKey.UpArrow => InterractWithUpArrow(page, index, current),
                    ConsoleKey.PageDown => InterractWithPageDown(page, index, current),
                    ConsoleKey.PageUp => InterractWithPageUp(page, index, current),
                    _ => (index, current)
                };
            }
            while (!keyInfo.Key.Equals(ConsoleKey.Escape));

            Console.ReadKey();
        }

        private static (int index, int current) InterractWithPageUp(List<List<string>> page, int index, int current)
        {
            return (SetToZeroOrDontChange(index - page.Count), SetToZeroOrDontChange(current - page.Count));
        }

        private static int SetToZeroOrDontChange(int number)
        {
            return number < 0 ? 0 : number;
        }

        private static int NoOfNewLines(string[] text)
        {
            var lines = 0;
            foreach (var phrase in text)
            {
                lines += phrase.Length / Console.WindowWidth * Console.WindowWidth
                    + (phrase.Length % Console.WindowWidth == 0 ? 0 : 1) * Console.WindowWidth;
            }

            return lines;
        }

        private static List<List<string>> TruncateText(List<List<string>> page, int size)
        {
            string toAdd = "";
            var occupiedRowSize = size - (page[^1][0] + page[^1][1] + page[^1][2]).Length;
            var i = 0;
            do
            {
                toAdd += page[^1][3][i];
                i++;
            }
            while (toAdd.Length + occupiedRowSize != size);
            page[^1].RemoveAt(3);
            page[^1].Add(toAdd + "...");
            return page;
        }

        private List<List<string>> GetPage(int index, int size)
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
                string[] temp = result[^1][3].Split("\n");
                commitTextSize += NoOfNewLines(new[] { result[^1][0] + result[^1][1] + result[^1][2] + temp[0] })
                    + NoOfNewLines(temp[1..]);
                index++;
            }
            while (index <= commitsIDAuthorDateMessage.Count - 1);

            return commitTextSize > size ? TruncateText(result, commitTextSize - size - 3) : result;
        }

        private void WriteText(string initialText, int current, List<List<string>> page)
        {
            Console.Clear();
            Console.WriteLine(initialText);
            page.ForEach(commit =>
            {
                if (commit[0] == commitsIDAuthorDateMessage[current][0] + " - ")
                {
                    Console.BackgroundColor = ConsoleColor.White;
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.Write(commit[0]);
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write(commit[1]);
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write(commit[2]);
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.WriteLine(commit[3]);
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.ResetColor();
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.Write(commit[0]);
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write(commit[1]);
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write(commit[2]);
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine(commit[3]);
                    Console.ResetColor();
                }
            });
            Console.Write("Press 'Esc' to exit.");
        }

        private (int index, int current) InterractWithDownArrow(List<List<string>> page, int index, int current)
        {
            if (page[^1][0] == commitsIDAuthorDateMessage[^1][0] + " - ")
            {
                return (index, current);
            }

            if (commitsIDAuthorDateMessage[current][0] + " - " == page[^1][0])
            {
                return (index + 1, current + 1);
            }

            return (SetToLastOrDontChange(index), SetToLastOrDontChange(current + 1));
        }

        private (int index, int current) InterractWithUpArrow(List<List<string>> page, int index, int current)
        {
            if (commitsIDAuthorDateMessage[current][0] + " - " == page[0][0])
            {
                index--;
            }

            return (SetToZeroOrDontChange(index), SetToZeroOrDontChange(current - 1));
        }

        private (int index, int current) InterractWithPageDown(List<List<string>> page, int index, int current)
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
            return number > commitsIDAuthorDateMessage.Count - 1 ? commitsIDAuthorDateMessage.Count - 1 : number;
        }
    }
}
