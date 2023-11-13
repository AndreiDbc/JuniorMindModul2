using Castle.Components.DictionaryAdapter.Xml;
using Moq;
using System;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using System.IO;
using System.IO.Enumeration;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace GitApp
{
    public class Patcher
    {
        private readonly GitManager gitManager;
        private readonly string repositoryPath;
        private readonly Diff diff;
        private readonly int[] headers;
        private readonly List<IElement> elements;
        private readonly string firstElement;
        private readonly (int start, int end, int currentHeaderStartIndex, int currentHeaderEndIndex) indeexes;

        public Patcher(string repositoryPath, GitManager gitManager, Diff diff)
        {
            this.diff = diff;
            elements = diff.GetText();
            firstElement = elements[0].GetElement();
            if (firstElement != "")
            {
                headers = GetHeaders();
                indeexes = GetStartLineNumberAndHunkIndex();
            }

            this.gitManager = gitManager;
            this.repositoryPath = repositoryPath;
        }

        public string GetLinePatchPath(bool isLineReplaced, bool isFileInTheStagingArea, string fileName)
        {
            if (firstElement == "" || indeexes.currentHeaderStartIndex == -1)
            {
                return "";
            }

            return GetPatchPath(false, isFileInTheStagingArea, isLineReplaced, fileName);
        }

        public string GetHunkPatchPath()
        {
            if (firstElement == "" || indeexes.currentHeaderStartIndex == -1)
            {
                return "";
            }

            return GetPatchPath(true);
        }

        public void StageAll(string fileName)
        {
            gitManager.StageFile(repositoryPath, fileName);
        }

        private int[] GetHeaders()
        {
            var result = new int[0];
            var count = 0;
            for (int i = 0; i < elements.Count; i++)
            {
                var temp = elements[i].GetElement();
                if (temp.StartsWith("@@"))
                {
                    Array.Resize(ref result, result.Length + 1);
                    result[count] = i;
                    count++;
                }
            }

            return result;
        }

        private (int start, int end, int currentHeaderStartIndex, int currentHeaderEndIndex) GetStartLineNumberAndHunkIndex()
        {
            var currentHeaderStartIndex = headers[0] > diff.Current ? -1 : headers.Last(index => index <= diff.Current);
            if (currentHeaderStartIndex < 0)
            {
                return (-1, -1, -1, -1);
            }

            var header = diff[currentHeaderStartIndex].GetElement();
            var temp = header.Split("@@ ")[1].Split(",");
            var start = int.Parse(temp[0][1..]);
            var end = int.Parse(temp[1].Split(" ")[0]);
            var currentHeaderEndIndex = headers[headers.Length - 1] == currentHeaderStartIndex ? elements.Count : headers.First(index => index > diff.Current);
            return (start, start + end, currentHeaderStartIndex, currentHeaderEndIndex);
        }

        private string FormateHeaderForSingleLine(
            (int start, int end, int currentHeaderStartIndex, int currentHeaderEndIndex) indeexes,
            (int unchanged, int changed) lineCount,
            bool isLineReplaced,
            string header)
        {
            (var previousLines, var nextLines) = GetDiffPatchLineCount(lineCount.unchanged);
            var currentElement = diff[diff.Current].GetElement();
            var isLineRemoved = currentElement.StartsWith("-") && !isLineReplaced;
            var first = indeexes.start + (lineCount.unchanged - previousLines) - (isLineReplaced && currentElement.StartsWith("+") ? 1 : 0);
            var second = isLineRemoved || (isLineReplaced && currentElement.StartsWith("+")) ? 1 + previousLines + nextLines : previousLines + nextLines;
            var third = first;
            var fourth = isLineRemoved || (isLineReplaced && currentElement.StartsWith("-")) ? previousLines + nextLines : 1 + previousLines + nextLines;

            return $"@@ -{first},{second} +{third},{fourth} @@" + header.Split("@@")[2] + "\n";
        }

        private void GeneratePatch(bool stageHunk, bool isFileInTheStagingArea, string patchPath, bool isLineReplaced = false, string fileName = "")
        {
            var toWrite = GetFileData();
            if (SetExitCondition(stageHunk))
            {
                return;
            }

            var header = elements[indeexes.currentHeaderStartIndex].GetElement();
            var lineCount = GeLineCount(indeexes.currentHeaderStartIndex, isLineReplaced);
            var linesData = stageHunk
                ? AddPatchLines((0, int.Parse(header.Split("@@")[1].Split(",")[2].Trim())), stageHunk)
                : AddPatchLines(lineCount, stageHunk, isFileInTheStagingArea, isLineReplaced, fileName);

            toWrite += stageHunk
                ? header + "\n"
                : FormateHeaderForSingleLine(indeexes, lineCount, isLineReplaced, header);

            toWrite += linesData + "\n";

            System.IO.File.Create(patchPath).Close();
            using (StreamWriter patchFileWriter = new StreamWriter(patchPath))
            {
                patchFileWriter.Write(toWrite);
                patchFileWriter.Close();
            }
        }

        private string GetFileData()
        {
            var result = "";
            for (int i = 0; i < headers[0]; i++)
            {
                result += diff[i].GetElement() + "\n";
            }

            return elements[elements.Count - 1].GetElement() == "\n" ? result[..^1] : result;
        }

        private string GetPatchPath(bool stageHunk, bool isFileInTheStagingArea = false, bool isLineReplaced = false, string fileName = "")
        {
            string patchFileName = "patch_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".patch";
            var patchPath = Path.GetTempPath() + patchFileName;
            GeneratePatch(stageHunk, isFileInTheStagingArea, patchPath, isLineReplaced, fileName);
            return patchPath;
        }

        private bool SetExitCondition(bool isHunkBeingStaged)
        {
            return isHunkBeingStaged ? !isHunkBeingStaged : !(elements[diff.Current].GetElement().StartsWith("-") || elements[diff.Current].GetElement().StartsWith("+"));
        }

        private string AddPatchLines(
            (int unchanged, int changed) lineCount,
            bool stageHunk,
            bool isFileInTheStagingArea = false,
            bool isLineReplaced = false,
            string fileName = "")
        {
            return stageHunk ? AddHunkLines() : FormateDiffForSingleLineAdded(lineCount, isFileInTheStagingArea, isLineReplaced, fileName);
        }

        private string AddHunkLines()
        {
            var result = "";
            for (int i = indeexes.currentHeaderStartIndex + 1; i < indeexes.currentHeaderEndIndex; i++)
            {
                result += diff[i].GetElement() + "\n";
            }

            return result;
        }

        private string FormateDiffForSingleLineAdded(
            (int unchanged, int changed) lineCount,
            bool isFileInTheStagingArea,
            bool isLineReplaced,
            string fileName)
        {
            var originalFile = isFileInTheStagingArea
                ? gitManager.GetStagedFileLines(fileName, repositoryPath).Split("\n")
                : gitManager.GetOriginalFileLines(fileName, repositoryPath).Split("\n");

            return FormateDiffForPatch(lineCount, originalFile, isLineReplaced);
        }

        private (int unchanged, int changed) GeLineCount(int header, bool isLineReplaced)
        {
            var changed = 0;
            var unchanged = 0;
            for (int i = header + 1; i < diff.Current; i++)
            {
                if (diff[i].GetElement().StartsWith("-"))
                {
                    changed--;
                    unchanged++;
                }
                else if (diff[i].GetElement().StartsWith("+"))
                {
                    changed++;
                }
                else
                {
                    unchanged++;
                }
            }

            if (isLineReplaced)
            {
                changed += diff[diff.Current].GetElement().StartsWith("-") ? 1 : (-1);
            }

            return (unchanged, changed);
        }

        private string FormateDiffForPatch((int unchanged, int changed) lineCount, string[] originalLines, bool isLineReplaced)
        {
            var whiteLineCount = GetDiffPatchLineCount(lineCount.unchanged);
            var previousLines = whiteLineCount.previousLines;
            var endLines = whiteLineCount.nextLines;
            var currentElement = diff[diff.Current].GetElement();
            var isLineRemoved = currentElement.StartsWith("-") && !isLineReplaced;
            var replacedCurrentThatStartsWithPlus = currentElement.StartsWith("+") && isLineReplaced;
            var start = indeexes.start + (lineCount.unchanged - previousLines) - 1 - (replacedCurrentThatStartsWithPlus ? 1 : 0);
            var result = "";
            var additionalNumber = 0;
            var count = 0;
            for (int i = start; i < start + previousLines + endLines + 1; i++)
            {
                var hasReachedCurrent = count == previousLines;
                if (hasReachedCurrent && isLineReplaced)
                {
                    result += GetReplacedLines(currentElement);
                    additionalNumber++;
                }
                else if (hasReachedCurrent)
                {
                    result += diff[diff.Current].GetElement() + "\n";
                    additionalNumber += isLineRemoved ? 1 : 0;
                }

                if (i + additionalNumber > originalLines.Length - 1)
                {
                    break;
                }

                result += " " + originalLines[i + additionalNumber] + "\n";
                count++;
            }

            return result;
        }

        private (int previousLines, int nextLines) GetDiffPatchLineCount(int unchangedLines)
        {
            var previousLines = indeexes.start - 3 < 0 ? 3 + (indeexes.start - 4) : 3;
            var endLines = Math.Min(3, indeexes.end - indeexes.start - unchangedLines);
            return (previousLines, endLines);
        }

        private string GetReplacedLines(string currentElement)
        {
            return currentElement.StartsWith("-")
                ? currentElement + "\n" + diff[diff.Current + 1].GetElement() + "\n"
                : diff[diff.Current - 1].GetElement() + "\n" + currentElement + "\n";
        }
    }
}