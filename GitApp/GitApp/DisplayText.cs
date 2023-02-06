using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata.Ecma335;
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
            int initialTextSize = initialText.Split("\n").Length * Console.WindowWidth;
            do
            {
                var page = GetPage(index, size - initialTextSize - Console.WindowWidth * 2);
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

        private static (int index, int current) InterractWithPageUp(List<string> page, int index, int current)
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
                lines = phrase.Length / Console.WindowWidth * Console.WindowWidth
                    + (phrase.Length % Console.WindowWidth == 0 ? 0 : 1) * Console.WindowWidth;
            }

            return lines;
        }

        private List<string> GetPage(int index, int size)
        {
            var result = new List<string>();
            int commitTextSize = 0;
            do
            {
                result.Add(commits[index]);
                commitTextSize += NoOfNewLines(commits[index].Split("\n"));
                index++;
            }
            while (commitTextSize <= size && index <= commits.Count - 1);
            return result;
        }

        private void WriteText(string initialText, int current, List<string> page)
        {
            Console.Clear();
            Console.WriteLine(initialText);
            page.ForEach(commit =>
            {
                if (commit == commits[current])
                {
                    Console.BackgroundColor = ConsoleColor.White;
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.WriteLine(commit);
                    Console.ResetColor();
                }
                else
                {
                    Console.WriteLine(commit);
                }
            });
            Console.Write("Press 'Esc' to exit.");
        }

        private (int index, int current) InterractWithDownArrow(List<string> page, int index, int current)
        {
            current = SetToLastOrDontChange(current + 1);
            if (page[^1] == commits[^1])
            {
                return (index, current);
            }

            if (commits[current] == page[^1])
            {
                index++;
            }

            return (SetToLastOrDontChange(index), current);
        }

        private (int index, int current) InterractWithUpArrow(List<string> page, int index, int current)
        {
            if (commits[current] == page[0])
            {
                index--;
            }

            return (SetToZeroOrDontChange(index), SetToZeroOrDontChange(current - 1));
        }

        private (int index, int current) InterractWithPageDown(List<string> page, int index, int current)
        {
            current = SetToLastOrDontChange(current + page.Count);
            if (page[^1] == commits[^1])
            {
                return (index, current);
            }

            return (SetToLastOrDontChange(index + page.Count), current);
        }

        private int SetToLastOrDontChange(int number)
        {
            return number > commits.Count - 1 ? commits.Count - 1 : number;
        }
    }
}
