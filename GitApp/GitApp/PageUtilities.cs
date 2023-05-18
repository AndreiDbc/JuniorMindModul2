using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitApp
{
    public class PageUtilities
    {
        private readonly List<Commit> list;

        public PageUtilities(List<Commit> list)
        {
            this.list = list;
        }

        public static string TruncateText(string element, int size)
        {
            string toAdd = "";
            var i = 0;
            do
            {
                if (size < 3 || i == element.Length - 1)
                {
                    break;
                }

                toAdd += element[i];
                i++;
            }
            while (toAdd.Length != size - 3);
            return toAdd + "...";
        }

        public static int GetSize(List<string> page)
        {
            int result = 0;
            page.ForEach(line => result += GetTextSize(line) - GetFormatingCodesLength(line));
            return result;
        }

        public static int GetLinesSizeForRightBox(string element, int size)
        {
            int result = 0;
            foreach (var line in element.Split("\n"))
            {
                result += line.Length / size * size
                    + (line.Length % size == 0 ? 0 : 1) * size;
            }

            return result;
        }

        public static int GetFormatingCodesLength(string element)
        {
            IEnumerable<string> codes = new[] { "\x1b[0m", "\x1b[7m", "\x1b[48;5;27m", "\x1b[48;5;46m", "\x1b[48;5;196m", "\u001b[38;5;27m", "\u001b[38;5;46m", "\u001b[38;5;196m" };
            return codes.Aggregate(0, (result, code) => element.Contains(code) ? code.Length + result : result);
        }

        public static int GetTextSize(string phrase)
        {
            return phrase.Length / Console.WindowWidth * Console.WindowWidth
                    + (phrase.Length % Console.WindowWidth == 0 ? 0 : 1) * Console.WindowWidth;
        }
    }
}