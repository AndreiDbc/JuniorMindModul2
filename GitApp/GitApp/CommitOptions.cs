using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace GitApp
{
    class CommitOptions
    {
        private readonly List<string> commits;

        public CommitOptions(List<string> commits)
        {
            this.commits = commits;
        }

        public void Interract(string initialText, int size)
        {
            int initialTextSize = initialText.Split("\n").Length * Console.LargestWindowWidth;
            ConsoleKeyInfo keyInfo;
            int index = 0;
            var current = index;
            do
            {
                Console.Clear();
                Console.WriteLine(initialText);
                var page = GetPage(index, size + initialTextSize, current);
                page.ForEach(commit => Console.WriteLine(commit));
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

        private List<string> GetPage(int index, int size, int current)
        {
            var result = new List<string>();
            int commitTextSize = 0;
            do
            {
                result.Add(commits[index]);
                commitTextSize += commits[index].Split("\n").Length * Console.LargestWindowWidth;
                if (commits[index] == commits[current])
                {
                    result[^1] = "> " + commits[current];
                }

                index++;
            }
            while (commitTextSize < size);
            return result;
        }

        private (int index, int current) InterractWithDownArrow(List<string> page, int index, int current)
        {
            if ("> " + commits[current] == page[^1])
            {
                index++;
            }

            current++;
            index = SetToLast(index);
            current = SetToLast(current);
            return (index, current);
        }

        private (int index, int current) InterractWithUpArrow(List<string> page, int index, int current)
        {
            if ("> " + commits[current] == page[0])
            {
                index--;
            }

            current--;
            index = SetToZero(index);
            current = SetToZero(current);

            return (index, current);
        }

        private (int index, int current) InterractWithPageDown(List<string> page, int index, int current)
        {
            index = SetToLast(index + page.Count);
            current = SetToLast(current + page.Count);
            return (index, current);
        }
    }
}
