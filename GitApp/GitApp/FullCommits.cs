using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace GitApp
{
    public class FullCommits : IInteractor<IElement>
    {
        private readonly string rawLogData;
        private readonly List<IElement> list;

        public FullCommits(string rawLogData)
        {
            this.rawLogData = rawLogData;
            Current = 0;
            Index = 0;
            list = GetText();
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
            var commits = new List<IElement>();
            var str = rawLogData.Split("\n\n");
            for (int i = 0; i < str.Length; i += 2)
            {
                commits.Add(new Commit(str[i] + "\n" + str[i + 1].Trim()));
            }

            return commits;
        }

        public List<string> GetHighlightedSide(int height, int width, int availableSpace, bool isScreenSplit = false)
        {
            var result = new List<string>();
            var index = Index;
            var doesPageContainCurrent = false;
            var occupancyRate = 0;
            do
            {
                if (list[index].Id.Equals(list[Current].Id))
                {
                    doesPageContainCurrent = true;
                }

                var processedElement = isScreenSplit
                    ? ProcessElement(index, width)
                    : ProcessElementForMainPage(index, width, height, availableSpace, occupancyRate);
                occupancyRate += processedElement.length;
                result.Add(processedElement.element);

                index++;
            }
            while (occupancyRate < availableSpace && index < list.Count);

            Current = doesPageContainCurrent ? Current : Index;
            result[0] = doesPageContainCurrent ? result[0] : ProcessElement(Current, width).element;
            return result;
        }

        public (List<IElement> list, int textSize) GetPageDataAndSize(int availableSpace, int lineSize, bool isScreenSplit = false)
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
                var element = result[^1].GetElement().Split("\n");
                var elementUtilities = new PageUtilities(element[0] + " " + element[1] + " " + element[2] + " " + element[3]);
                textSize += isScreenSplit ? lineSize : elementUtilities.GetOccupancyRate(lineSize);
            }
            while (textSize < availableSpace);

            return (result, textSize);
        }

        private static string GetMessage(bool isCurrentElementAdded, int rowWidth, string toAdd, string line)
        {
            const int totalFormatingCodesElementsLength = 35;
            var messageLength = rowWidth - line.Length + totalFormatingCodesElementsLength;
            toAdd = toAdd.Length > messageLength
                ? (isCurrentElementAdded ? "\u001b[0m\u001b[7m" : "\u001b[0m") + new PageUtilities(toAdd).TruncateText(messageLength)
                : (isCurrentElementAdded ? "\u001b[0m\u001b[7m" : "\u001b[0m") + toAdd;

            return toAdd;
        }

        private (string element, int length) ProcessElement(int index, int width)
        {
            var formatingCodesForCurrent = new[] { "\x1b[48;5;27m", "\x1b[48;5;46m", "\x1b[48;5;196m" };
            var formatingCodes = new[] { "\x1b[38;5;27m", "\x1b[38;5;46m", "\x1b[38;5;196m" };
            var currentCommit = list[Current];
            var toAdd = list[index];
            var toAddDetails = toAdd.GetElement().Split("\n");
            var isCurrentElementAdded = toAdd.Id.Equals(currentCommit.Id);
            var processedElement = isCurrentElementAdded ? "\u001b[7m" : "\u001b[0m";
            for (int i = 0; i < toAddDetails.Length - 2; i++)
            {
                var element = toAddDetails[i];
                var utilities = new PageUtilities(element);
                processedElement += element.Length + 1 > width / 4
                    ? (isCurrentElementAdded ? formatingCodesForCurrent[i] : formatingCodes[i]) + utilities.TruncateText(width / 4) + " "
                    : (isCurrentElementAdded ? formatingCodesForCurrent[i] : formatingCodes[i]) + element + " ";
            }

            processedElement += GetMessage(isCurrentElementAdded, width, toAddDetails[3], processedElement);

            return (processedElement, width);
        }

        private (string element, int length) ProcessElementForMainPage(int index, int width, int height, int availableSpace, int currentOccupancy)
        {
            var formatingCodesForCurrent = new[] { "\x1b[48;5;27m", "\x1b[48;5;46m", "\x1b[48;5;196m" };
            var formatingCodes = new[] { "\x1b[38;5;27m", "\x1b[38;5;46m", "\x1b[38;5;196m" };
            var currentCommit = list[Current];
            var toAdd = list[index];
            var toAddDetails = toAdd.GetElement().Split("\n");
            var isCurrentElementAdded = toAdd.Id.Equals(currentCommit.Id);
            var processedElement = isCurrentElementAdded ? "\u001b[7m" : "\u001b[0m";
            for (int i = 0; i < toAddDetails.Length - 2; i++)
            {
                var element = toAddDetails[i];
                processedElement += (isCurrentElementAdded ? formatingCodesForCurrent[i] : formatingCodes[i]) + element + " ";
            }

            processedElement += (isCurrentElementAdded ? "\u001b[0m\u001b[7m" : "\u001b[0m") + toAddDetails[3];
            var utilities = new PageUtilities(processedElement);
            var occupancyRate = new PageUtilities(toAddDetails[0] + toAddDetails[1] + toAddDetails[2] + toAddDetails[3] + "   ").GetOccupancyRate(width);
            processedElement = currentOccupancy + occupancyRate > availableSpace
                ? utilities.TruncateText(availableSpace - currentOccupancy + utilities.FormatingCodesStringUtilities().length)
                : processedElement;

            return (processedElement, occupancyRate);
        }
    }
}