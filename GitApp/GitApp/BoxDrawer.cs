using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
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

        public List<string> DrawBox(int boxWidth, int windowHeight, bool shouldFitText = false, bool shouldColorLines = false, string branchInfo = "", string overlayMenu = "")
        {
            const int lengthToSubstract = 2;
            var result = new List<string>();
            result.AddRange(text);
            result = shouldFitText ? FitTextInsideBox((boxWidth - 2) * (windowHeight - 2), boxWidth - 2) : result;
            var lineColor = shouldColorLines ? "\x1b[38;5;166m" : "\u001b[0m";
            result.Insert(0, lineColor + "┌" + boxName + new string('─', boxWidth - boxName.Length - lengthToSubstract) + "┐");
            var index = 1;

            do
            {
                if (index < result.Count)
                {
                    var padLength = boxWidth + 2 - "\u001b[0m".Length - (result[index].Length - new PageUtilities(result[index]).FormatingCodesStringUtilities().length);
                    var toPad = padLength <= 0 ? lineColor + "│" : lineColor + "│".PadLeft(padLength + 1);
                    result[index] = lineColor + "│\u001b[0m" + result[index] + toPad;
                }
                else
                {
                    result.Add(lineColor + "│" + "│".PadLeft(boxWidth - 1));
                }

                index++;
            }
            while (result.Count <= windowHeight - 2 - (overlayMenu == "" ? 0 : 1) || !result[^1].StartsWith(lineColor + "│"));

            AddOverlayMenu(ref result, overlayMenu, boxWidth, lineColor);

            result.Add(branchInfo == ""
                ? lineColor + "└" + new string('─', boxWidth - 2) + "┘"
                : lineColor + "└" + new string('─', boxWidth - 2 - branchInfo.Length) + $"{branchInfo}┘");
            return result;
        }

        private static void AddOverlayMenu(ref List<string> result, string overlayMenu, int boxWidth, string lineColor)
        {
            if (overlayMenu == "")
            {
                return;
            }

            var padLength = boxWidth + 2 - (overlayMenu.Length - new PageUtilities(overlayMenu).FormatingCodesStringUtilities().length);
            var toPad = padLength <= 0 ? lineColor + "│" : lineColor + "│".PadLeft(padLength + 1);
            result.Add(lineColor + "│" + overlayMenu + toPad);
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
                    result[^1] = result[^1].Length >= rowSize - 3
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