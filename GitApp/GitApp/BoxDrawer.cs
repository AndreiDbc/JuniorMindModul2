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

        public List<string> DrawBox(int boxWidth, int windowHeight)
        {
            const int lengthToSubstract = 2;
            var result = new List<string>();
            result.AddRange(text);
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
    }
}
