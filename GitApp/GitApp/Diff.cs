using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GitApp
{
    public class Diff : IInteractor<IElement>
    {
        private readonly List<IElement> diffList;
        private readonly string diff;

        public Diff(string diff)
        {
            this.diff = diff;
            diffList = GetText();
            Current = 0;
            Index = 0;
        }

        public int Current { get; set; }

        public int Index { get; set; }

        public IElement this[int index]
        {
            get => diffList[index];
        }

        public List<IElement> GetText()
        {
            return GenerateElements();
        }

        public List<string> GetHighlightedSide(int height, int width, int availableSpace, bool isScreenSplit = false)
        {
            return GenerateHighlightedSide(height, width, availableSpace, isScreenSplit);
        }

        public (List<IElement> list, int textSize) GetPageDataAndSize(int availableSpace, int lineSize, bool isScreenSplit = false)
        {
            var result = new List<IElement>();
            int textSize = 0;
            var index = Index;
            do
            {
                if (index > diffList.Count - 1 || textSize >= availableSpace)
                {
                    break;
                }

                result.Add(diffList[index]);
                textSize += new PageUtilities(diffList[index].GetElement()).GetOccupancyRate(lineSize);
                index++;
            }
            while (textSize <= availableSpace);

            return (result, textSize);
        }

        private static List<string> RemoveLines(List<string> list, int occupancyRate, int width, int availableSpace)
        {
            if (occupancyRate <= availableSpace)
            {
                return list;
            }

            do
            {
                var lastElementUtilities = new PageUtilities(new PageUtilities(list[^1][..^4]).FormatingCodesStringUtilities().clearedString);
                list.RemoveAt(list.Count - 1);
                occupancyRate -= lastElementUtilities.GetOccupancyRate(width);
            }
            while (occupancyRate > availableSpace);

            var tmp = new PageUtilities(list[^1][..^4]);
            list[^1] = tmp.TruncateText(width + tmp.FormatingCodesStringUtilities().length) + "\u001b[0m";

            return list;
        }

        private static string RecolorDiffLine(string previous, string current, string next, bool isScreenSplit = false)
        {
            if (current.StartsWith("+") && (previous.StartsWith("\u001b[38;5;190m-") || previous.StartsWith("\u001b[7m\u001b[38;5;190m-")))
            {
                return "\x1b[38;5;190m" + current;
            }
            else if (current.StartsWith("+"))
            {
                return "\x1b[38;5;40m" + current;
            }
            else if (current.StartsWith("-") && next.StartsWith("+"))
            {
                return "\u001b[38;5;190m" + current;
            }
            else if (current.StartsWith("-"))
            {
                return "\x1b[38;5;160m" + current;
            }

            return current;
        }

        private List<string> GenerateHighlightedSide(int height, int width, int availableSpace, bool isScreenSplit = false)
        {
            var lines = diff.Split("\n");
            var startingPoint = lines[3].StartsWith("+++") ? 5 : 6;
            var index = startingPoint < Index ? Index : startingPoint;
            var result = new List<string>();
            var occupacyRate = 0;
            var doesPageContainCurrent = false;
            (result, occupacyRate, doesPageContainCurrent) = SetFirstLines(result, lines, startingPoint, width, isScreenSplit);
            index = occupacyRate == 0 ? Index : index;
            for (int i = index; i < lines.Length; i++)
            {
                if (diffList[index].Id.Equals(diffList[Current].Id))
                {
                    doesPageContainCurrent = true;
                }

                if (occupacyRate >= availableSpace)
                {
                    result = SetCurrent(result, doesPageContainCurrent);
                    result[^1] = SetLastLine(result, occupacyRate, availableSpace, width, index);
                    result = RemoveLines(result, occupacyRate, width, availableSpace);
                    return result;
                }

                var lineUtilities = new PageUtilities(lines[i]);
                occupacyRate += lineUtilities.GetOccupancyRate(width);
                lines[i] = (diffList[index].Id.Equals(diffList[Current].Id) && isScreenSplit ? "\u001b[7m" : "")
                    + RecolorDiffLine(lines[i - 1], lines[i], index == diffList.Count - 1 ? "" : lines[i + 1]);
                result.AddRange(new PageUtilities(lines[i]).SplitRow(width));
                index++;
            }

            result = SetCurrent(result, doesPageContainCurrent);
            result[^1] = SetLastLine(result, occupacyRate, availableSpace, width, index);
            result = RemoveLines(result, occupacyRate, width, availableSpace);

            return result;
        }

        private List<IElement> GenerateElements()
        {
            var result = new List<IElement>();
            var temp = diff.Split("\n");
            for (int i = 0; i < temp.Length; i++)
            {
                result.Add(new DiffLine(temp[i], i.ToString()));
            }

            return result;
        }

        private string SetLastLine(List<string> list, int occupancyRate, int availableSpace, int width, int index)
        {
            var utilities = new PageUtilities(list[^1][..^4]);

            if (occupancyRate > availableSpace || index < diffList.Count)
            {
                list[^1] = list[^1].Length - utilities.FormatingCodesStringUtilities().length <= width - 3
                    ? list[^1] + "..."
                    : utilities.TruncateText(width + utilities.FormatingCodesStringUtilities().length) + "\u001b[0m";
            }

            return list[^1];
        }

        private List<string> SetCurrent(List<string> list, bool doesPageContainCurrent)
        {
            Current = doesPageContainCurrent ? Current : Index;
            list[0] = doesPageContainCurrent ? list[0] : "\u001b[7m" + list[0];
            return list;
        }

        private (List<string> lines, int occupancy, bool doesPageContainCurrent) SetFirstLines(
            List<string> list,
            string[] lines,
            int startingPoint,
            int width,
            bool isScreenSplit)
        {
            var occupancy = 0;
            if (startingPoint < Index)
            {
                return (list, occupancy, false);
            }

            var doesPageContainCurrent = false;
            var toInsert = new List<string>();
            var index = Index;
            for (int i = index; i < startingPoint; i++)
            {
                if (diffList[index].Id.Equals(diffList[Current].Id))
                {
                    doesPageContainCurrent = true;
                }

                var utilities = new PageUtilities(lines[i]);
                occupancy += utilities.GetOccupancyRate(width);
                toInsert.AddRange(diffList[index].Id.Equals(diffList[Current].Id)
                    ? new PageUtilities((isScreenSplit ? "\u001b[7m" : "") + lines[i]).SplitRow(width)
                    : utilities.SplitRow(width));
                index++;
            }

            list.InsertRange(0, toInsert);
            return (list, occupancy, doesPageContainCurrent);
        }
    }
}