using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace GitApp
{
    public class ModifiedFiles : IInteractor<IElement>
    {
        private readonly string rawData;
        private readonly List<IElement> list;

        public ModifiedFiles(string rawData)
        {
            this.rawData = rawData;
            list = GetText();
            Current = 0;
            Index = 0;
        }

        public int Current { get; set; }

        public int Index { get; set; }

        public IElement this[int index]
        {
            get
            {
                return list[index];
            }
        }

        public List<IElement> GetText()
        {
            var result = new List<IElement>();
            foreach (var item in rawData.Split("\n"))
            {
                result.Add(new File(item));
            }

            return result;
        }

        public (List<IElement> list, int textSize) GetPageDataAndSize(
            int availableSpace,
            int lineSize,
            bool isScreenSplit = false)
        {
            var result = new List<IElement>();
            int textSize = 0;
            var index = Index;
            do
            {
                if (index > list.Count - 1 || textSize >= availableSpace)
                {
                    break;
                }

                result.Add(list[index]);
                index++;
                textSize += lineSize;
            }
            while (textSize <= availableSpace);

            return (result, textSize);
        }

        public List<string> GetHighlightedSide(int height, int width, int availableSpace, bool isScreenSplit = false)
        {
            var result = new List<string>();
            var currentElement = list[Current];
            var occupancyRate = 0;
            var index = Index;
            var doesPageContainCurrent = false;
            do
            {
                if (list[index].Id.Equals(list[Current].Id))
                {
                    doesPageContainCurrent = true;
                }

                var toAdd = list[index].GetElement();
                var toAddUtilities = new PageUtilities(toAdd);
                result.Add((currentElement.Id == list[index].Id ? "\x1b[7m" : "\x1b[0m")
                    + (toAdd.Length > width ? toAddUtilities.TruncateText(width) : toAdd));
                occupancyRate += width;
                index++;
            }
            while (occupancyRate < availableSpace && index < list.Count);

            Current = doesPageContainCurrent ? Current : Index;
            result[0] = doesPageContainCurrent ? result[0] : "\x1b[7m" + result[0][4 ..];
            var lastElementUtilities = new PageUtilities(result[^1]);
            result[^1] = occupancyRate > availableSpace
                ? lastElementUtilities.TruncateText(availableSpace - occupancyRate - lastElementUtilities.GetOccupancyRate(width))
                : result[^1];
            return result;
        }
    }
}
