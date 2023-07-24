using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitApp
{
    public class Diff : IInteractor<IElement>
    {
        private readonly string diff;
        private readonly List<IElement> diffList;

        public Diff(GitManager gitManager, string oldId, string newId, string repoPath, string filePath)
        {
            diff = gitManager.GetDiffBetweenConsecutiveCommits(oldId, newId, repoPath, filePath);
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
            var result = new List<IElement>();
            foreach (var item in diff.Split("\n"))
            {
                result.Add(new DiffLine(item));
            }

            return result;
        }

        public List<string> GetHighlightedSide(int height, int width, int availableSpace, bool isScreenSplit = false)
        {
            var lines = diff.Split("\n");
            var startingPoint = lines[3].StartsWith("+++") ? 5 : 6;
            var index = startingPoint < Index ? Index : startingPoint;
            var result = new List<string>();
            var occupacyRate = 0;
            (result, occupacyRate) = SetFirst(result, lines, startingPoint, index, width);

            for (int i = index; i < lines.Length - 1; i++)
            {
                if (occupacyRate >= availableSpace)
                {
                    result = SetLast(result, occupacyRate, availableSpace, width);
                    return result;
                }

                var lineUtilities = new PageUtilities(lines[i]);
                occupacyRate += lineUtilities.GetOccupancyRate(width);
                lines[i] = (i == Current && isScreenSplit ? "\u001b[7m" : "") + RecolorDiffLine(lines[i - 1], lines[i], lines[i + 1]);
                result.AddRange(new PageUtilities(lines[i]).SplitRow(width));
                index++;
            }

            return result;
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
                index++;
                textSize += new PageUtilities(diffList[index].GetElement()).GetOccupancyRate(lineSize);
            }
            while (textSize <= availableSpace);

            return (result, textSize);
        }

        private static string RecolorDiffLine(string previous, string current, string next)
        {
            if (current.StartsWith("+") && previous.StartsWith("\u001b[38;5;190m-"))
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

        private static (List<string> lines, int occupancy) SetFirst(List<string> list, string[] lines, int startingPoint, int index, int width)
        {
            var toInsert = new List<string>();
            var occupancy = 0;
            for (int i = 0; i < startingPoint; i++)
            {
                var utilities = new PageUtilities(lines[i]);
                occupancy += utilities.GetOccupancyRate(width);
                toInsert.AddRange(utilities.SplitRow(width));
            }

            list.InsertRange(0, toInsert);
            return (list, occupancy);
        }

        private static List<string> SetLast(List<string> list, int occupancyRate, int availableSpace, int width)
        {
            var utilities = new PageUtilities(list[^1][..^4]);
            if (occupancyRate <= availableSpace)
            {
                list[^1] = utilities.FormatingCodesStringUtilities().length <= width - 3
                    ? list[^1] + "..."
                    : utilities.TruncateText(width) + "\u001b[0m";
                return list;
            }

            do
            {
                list.RemoveAt(list.Count - 1);
                occupancyRate -= width;
            }
            while (occupancyRate > availableSpace);

            list[^1] = utilities.TruncateText(width + utilities.FormatingCodesStringUtilities().length) + "\u001b[0m";

            return list;
        }
    }
}
