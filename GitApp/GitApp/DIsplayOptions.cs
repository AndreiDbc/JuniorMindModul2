using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace GitApp
{
    class DisplayOptions
    {
        private readonly List<string> commits;

        public DisplayOptions(List<string> commits)
        {
            this.commits = commits;
        }

        public void Interract(string initialText)
        {
            var size = Console.WindowHeight * Console.WindowWidth;
            int initialTextSize = initialText.Split("\n").Length * Console.WindowWidth;
            ConsoleKeyInfo keyInfo;
            int index = 0;
            var current = index;
            do
            {
                Console.Clear();
                Console.WriteLine(initialText);
                var page = GetPage(index, size - initialTextSize - Console.WindowWidth, current);
                page.ForEach(commit => Console.WriteLine(commit));
                Console.Write("Press 'Esc' to exit.");
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
            index = SetToZero(index - page.Count);
            current = SetToZero(current - page.Count);
            return (index, current);
        }

        private static int SetToZero(int number)
        {
            return number < 0 ? 0 : number;
        }

        private static int NoOfNewLines(string[] commit)
        {
            var lines = 0;
            foreach (var phrase in commit)
            {
                lines = phrase.Length / Console.WindowWidth * Console.WindowWidth
                    + (phrase.Length % Console.WindowWidth == 0 ? 0 : 1) * Console.WindowWidth;
            }

            return lines;
        }

        private List<string> GetPage(int index, int size, int current)
        {
            var result = new List<string>();
            int commitTextSize = 0;
            do
            {
                result.Add(commits[index]);
                commitTextSize += NoOfNewLines(commits[index].Split("\n"));
                if (commits[index] == commits[current])
                {
                    result[^1] = "> " + commits[current];
                }

                index++;
            }
            while (commitTextSize <= size && index <= commits.Count - 1);
            return result;
        }

        private (int index, int current) InterractWithDownArrow(List<string> page, int index, int current)
        {
            if ("> " + commits[current] == page[^1])
            {
                index++;
            }

            current++;
            index = SetToLast(index, page);
            current = SetToLast(current, page);
            return (index, current);
        }

        private (int index, int current) InterractWithUpArrow(List<string> page, int index, int current)
        {
            if ("> " + commits[current] == page[0])
            {
                index--;
            }

            current--;
            current = SetToZero(current);
            if (page[^1] == commits[^1])
            {
                return (index, current);
            }

            index = SetToZero(index);
            return (index, current);
        }

        private (int index, int current) InterractWithPageDown(List<string> page, int index, int current)
        {
            current = SetToLast(current + page.Count - 1, page);
            if (page[^1] == commits[^1])
            {
                return (index, current);
            }

            index = SetToLast(index + page.Count - 1, page);
            return (index, current);
        }

        private int SetToLast(int number, List<string> page)
        {
            return number > commits.Count - 1 ? commits.Count - 1 : number;
        }
    }
}
