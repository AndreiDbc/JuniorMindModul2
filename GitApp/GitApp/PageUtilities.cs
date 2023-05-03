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
                toAdd += element[i];
                i++;
            }
            while (toAdd.Length != size - 3);
            return toAdd + "...";
        }

        public (List<IElement> list, int textSize) GetPageDataAndSize(int index, int availableSpace)
        {
            var result = new List<IElement>();
            int textSize = 0;
            do
            {
                if (index > list.Count - 1)
                {
                    break;
                }

                result.Add(list[index]);
                index++;
                textSize += GetTextSize(result[^1].GetElement()
                    + (result[0].GetType().ToString() == "GitApp.Commit" ? "- :," : ""));
            }
            while (textSize < availableSpace);

            return (result, textSize);
        }

        private static int GetTextSize(string phrase)
        {
            return phrase.Length / Console.WindowWidth * Console.WindowWidth
                    + (phrase.Length % Console.WindowWidth == 0 ? 0 : 1) * Console.WindowWidth;
        }

        private static IElement GetNewElement(string typeName, string data)
        {
            return typeName switch
            {
                "GitApp.Commit" => new Commit(data),
                "GitApp.CommitModifiedFile" => new CommitModifiedFile(data)
            };
        }
    }
}