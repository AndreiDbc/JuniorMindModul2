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
        private readonly string[] data;

        public PageUtilities(string element)
        {
            this.element = element;
        }

        public PageUtilities(string[] data)
        {
            this.data = data;
        }

        public List<string> SplitRow(int limit)
        {
            var result = new List<string>();
            var codes = "";
            for (int i = 0; i < data.Length; i++)
            {
                var differenceLength = 0;
                codes = "";
                for (int j = 0, k = 1; j < data[i].Length; j += limit + differenceLength, k++)
                {
                    var splitElement = data[i][j..Math.Min(data[i].Length, limit * k)];
                    var elementUtilities = new PageUtilities(splitElement);
                    var previousDifferenceLength = differenceLength;
                    var elementCodesAndLength = elementUtilities.FormatingCodesStringUtilities();
                    differenceLength = elementCodesAndLength.length;
                    result.Add(
                        codes +
                        data[i][j..Math.Min(data[i].Length, limit * k + (previousDifferenceLength > differenceLength ? previousDifferenceLength : differenceLength))] +
                        "\u001b[0m");
                    codes = new PageUtilities(result[^1]).FormatingCodesStringUtilities().codes.Replace("\u001b[0m", "");
                }
            }

            return result;
        }

        public string TruncateText(int size)
        {
            string toAdd = "";
            var i = 0;
            do
            {
                if (size <= 3 || i == element.Length)
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

        public (int length, string clearedString, string codes) FormatingCodesStringUtilities()
        {
            var clearedString = new string(element);
            var codes = "";
            foreach (var code in new[]
            {
                "\x1b[0m",
                "\x1b[7m",
                "\x1b[48;5;27m",
                "\x1b[48;5;46m",
                "\x1b[48;5;196m",
                "\x1b[38;5;27m",
                "\x1b[38;5;46m",
                "\x1b[38;5;196m",
                "\x1b[38;5;190m",
                "\x1b[38;5;40m",
                "\x1b[38;5;160m"
            })
            {
                var temp = clearedString.Replace(code, "");
                codes += clearedString == temp ? "" : code;
                clearedString = temp;
            }

            return (element.Length - clearedString.Length, clearedString, codes);
        }
    }
}