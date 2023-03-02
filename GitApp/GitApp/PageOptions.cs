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
    }
}
