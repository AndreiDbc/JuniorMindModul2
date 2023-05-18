using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitApp
{
    public class PageUtilities
    {
        private readonly string element;

        public PageUtilities(string element)
        {
            this.element = element;
        }

        public string TruncateText(int size)
        {
            string toAdd = "";
            var i = 0;
            do
            {
                if (size <= 3 || i == element.Length - 1)
                {
                    break;
                }

                toAdd += element[i];
                i++;
            }
            while (toAdd.Length != size - 3);
            return toAdd + "...";
        }

        public int GetOccupancyRate(int size)
        {
            int result = 0;
            foreach (var line in element.Split("\n"))
            {
                result += line.Length / size * size
                    + (line.Length % size == 0 ? 0 : 1) * size;
            }

            return result;
        }

        public int GetFormatingCodesLength()
        {
            IEnumerable<string> codes = new[] { "\x1b[0m", "\x1b[7m", "\x1b[48;5;27m", "\x1b[48;5;46m", "\x1b[48;5;196m", "\u001b[38;5;27m", "\u001b[38;5;46m", "\u001b[38;5;196m" };
            return codes.Aggregate(0, (result, code) => element.Contains(code) ? code.Length + result : result);
        }
    }
}