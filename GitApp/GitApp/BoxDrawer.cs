using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace GitApp
{
    public class BoxDrawer
    {
        private readonly string boxName;
        private readonly List<string> text;

        public BoxDrawer(List<string> text, string boxName)
        {
            this.text = text;
            this.boxName = boxName;
        }

        public List<string> DrawBox(int boxWidth, int windowHeight, bool shouldFitText = false)
        {
            const int lengthToSubstract = 2;
            var result = new List<string>();
            result.AddRange(text);
            result = shouldFitText ? FitTextInsideBox((boxWidth - 2) * (windowHeight - 2), boxWidth - 2) : result;
            result.Insert(0, "┌" + boxName + new string('─', boxWidth - boxName.Length - lengthToSubstract) + "┐");
            var index = 1;
            do
            {
                if (index < result.Count)
                {
                    var padLength = boxWidth + 2 - (result[index].Length - new PageUtilities(result[index]).FormatingCodesStringUtilities().length);
                    var toPad = padLength <= 0 ? "\u001b[0m│" : "\u001b[0m│".PadLeft(padLength + 1);
                    result[index] = "\x1b[0m│" + result[index] + toPad;
                }
                else
                {
                    result.Add("\x1b[0m│" + "\u001b[0m│".PadLeft(boxWidth + 3));
                }

                index++;
            }
            while (result.Count <= windowHeight - 2 || !result[^1].StartsWith("\x1b[0m│"));
            result.Add("\x1b[0m└" + new string('─', boxWidth - 2) + "┘");
            return result;
        }

        private List<string> FitTextInsideBox(int availableSpace, int rowSize)
        {
            var result = new List<string>();
            var currentSize = 0;
            for (int i = 0; i < text.Count; i++)
            {
                var element = text[i];
                var elementUtilities = new PageUtilities(element);
                var clearedElement = elementUtilities.FormatingCodesStringUtilities().clearedString;
                var clearedElementUtilities = new PageUtilities(clearedElement);
                currentSize += clearedElement == "" ? rowSize : clearedElementUtilities.GetOccupancyRate(rowSize);
                if (currentSize > availableSpace)
                {
                    result[^1] = result[^1].Length == rowSize
                        ? new PageUtilities(result[^1]).TruncateText(rowSize)
                        : result[^1] + "...";
                    break;
                }

                result.Add(element);
            }

            return result;
        }
    }
}
