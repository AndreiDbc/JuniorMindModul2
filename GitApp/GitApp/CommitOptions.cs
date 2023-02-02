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
    }
}
