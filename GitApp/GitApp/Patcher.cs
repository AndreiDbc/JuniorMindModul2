using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitApp
{
    public class Patcher
    {
        private readonly string filePath;
        private readonly Diff diff;
        private readonly GitManager gitManager;
        private string fileData;
        private string elementDiff;

        public Patcher(IElement hunk, string filePath)
        {
            var data = hunk.GetElement().Split("\n\x1b\n");
            fileData = data[0];
            elementDiff = data[1];
            this.filePath = filePath;
        }

        public Patcher(Diff diff, GitManager gitManager)
        {
            this.diff = diff;
            this.gitManager = gitManager;
        }

        private void ApplyPatch(StreamWriter patchFileWriter)
        {
            var startLine = GetStartingLineIndex();
            var fileLines = System.IO.File.ReadAllLines(filePath);
            var hunkLines = elementDiff.Split('\n');

            for (int i = 0; i < hunkLines.Length; i++)
            {
                int lineIndex = startLine - 1 + i;

                if (lineIndex < fileLines.Length)
                {
                    if (hunkLines[i].StartsWith("-"))
                    {
                        fileLines[lineIndex] = null;
                    }
                    else if (hunkLines[i].StartsWith("+"))
                    {
                        fileLines[lineIndex] = hunkLines[i][1 ..];
                    }
                }
            }

            string patchedContent = string.Join(Environment.NewLine, fileLines);
            patchFileWriter.WriteLine(patchedContent);
        }

        private int GetStartingLineIndex()
        {
            var temp = fileData.Split("@@ ")[1];
            var result = "";

            foreach (var character in temp)
            {
                if (character.Equals(','))
                {
                    break;
                }

                result += character;
            }

            return int.Parse(result[1 ..]);
        }

        private string GetPathWithoutFileName()
        {
            for (int i = filePath.Length - 1; i > -1; i--)
            {
                if (filePath[i] == '/')
                {
                    return filePath[.. (i + 1)];
                }
            }

            return "";
        }
    }
}
