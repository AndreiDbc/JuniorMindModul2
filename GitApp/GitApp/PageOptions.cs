using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitApp
{
    public class PageOptions
    {
        private readonly IText text;

        public PageOptions(IText text)
        {
            this.text = text;
        }

        public static List<string> TruncateText(List<string> list)
        {
            var line = list[^1];
            string toAdd = "";
            var i = 0;
            do
            {
                toAdd += line[i];
                i++;
            }
            while (toAdd.Length != Console.WindowWidth - 3);
            list.RemoveAt(list.Count - 1);
            list.Add(toAdd + "...");
            return list;
        }

        public (List<string> list, int textSize) GetPageDataAndSize(int index, int emptySpace)
        {
            var list = text.GetText();
            var result = new List<string>();
            int textSize = 0;
            do
            {
                if (index > list.Count - 1)
                {
                    break;
                }

                result.Add(list[index]);
                index++;
                textSize += GetTextSize(result[^1]);
            }
            while (textSize < emptySpace);

            return (result, textSize);
        }

        private static int GetTextSize(string phrase)
        {
            return phrase.Length / Console.WindowWidth * Console.WindowWidth
                    + (phrase.Length % Console.WindowWidth == 0 ? 0 : 1) * Console.WindowWidth;
        }
    }
}