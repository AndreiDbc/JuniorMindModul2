using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
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
            this.data = new string[0];
        }

        public PageUtilities(string[] data)
        {
            this.data = data;
            this.element = "";
        }

        public List<string> SplitRow(int limit, bool isMainLogPage = false)
        {
            var lines = element != "" ? new[] { element } : data;
            var result = new List<string>();
            var codes = "";
            var previousDifferenceLength = 0;
            var differenceLength = 0;
            for (int i = 0; i < lines.Length; i++)
            {
                codes = "";
                for (int j = 0, k = 1; j < lines[i].Length; j += limit + differenceLength, k++)
                {
                    var splitElement = lines[i][j..Math.Min(lines[i].Length, limit * k)];
                    var elementUtilities = new PageUtilities(splitElement);
                    previousDifferenceLength = previousDifferenceLength > differenceLength ? codes.Length : differenceLength;
                    var elementCodesAndLength = elementUtilities.FormatingCodesStringUtilities();
                    differenceLength = elementCodesAndLength.length;
                    result.Add(
                        codes +
                        lines[i][j..Math.Min(lines[i].Length, limit * k + (previousDifferenceLength > differenceLength ? previousDifferenceLength : differenceLength))] +
                        "\u001b[0m");
                    codes = SetCodesValue(codes, result[^1], isMainLogPage);
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

        public List<string> PadElement(int width)
        {
            var result = new List<string>();
            foreach (var line in data)
            {
                var elementUtilities = new PageUtilities(line);
                var clearedElement = elementUtilities.FormatingCodesStringUtilities().clearedString;
                var padLength = new PageUtilities(clearedElement).GetOccupancyRate(width) - clearedElement.Length;
                var toPad = new string(' ', padLength);
                result.Add(clearedElement.Length % width != 0 ? line + toPad : line);
            }

            return result;
        }

        public int GetOccupancyRate(int size)
        {
            int result = 0;
            foreach (var line in element == "" ? data : element.Split("\n"))
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
                "\x1b[38;5;160m",
                "\x1b[38;5;166m",
                "\u001b[32m",
                "\u001b[33m",
                "\u001b[34m",
                "\u001b[35m",
                "\u001b[36m",
                "\u001b[37m",
                "\u001b[38m"
            })
            {
                var temp = clearedString.Replace(code, "");
                codes += clearedString == temp ? "" : code;
                clearedString = temp;
            }

            codes += element.EndsWith("\x1b[0m") ? "\x1b[0m" : "";

            return (element.Length - clearedString.Length, clearedString, codes);
        }

        private static string SetCodesValue(string codes, string lastAdded, bool isMainLogPage)
        {
            var tempCodes = new PageUtilities(lastAdded).FormatingCodesStringUtilities().codes;
            var tempCodesLengthDifferenceIsZero = tempCodes.Replace("\u001b[0m", "").Length + 4 == tempCodes.Length;
            var shouldClearCodes = isMainLogPage ? !tempCodesLengthDifferenceIsZero : isMainLogPage;
            return shouldClearCodes ? codes + "\x1b[0m" : tempCodes.Replace("\u001b[0m", "");
        }
    }
}