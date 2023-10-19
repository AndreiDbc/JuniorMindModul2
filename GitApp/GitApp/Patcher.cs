using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

        public Patcher(string repositoryPath, GitManager gitManager, Diff diff)
        {
            this.diff = diff;
            elements = diff.GetText();
            headers = GetHeaders();
            this.gitManager = gitManager;
            this.repositoryPath = repositoryPath;
        }

        public void StageLine()
        {
            if (elements.Count == 0)
            {
                return;
            }

            Stage(false);
        }

        public void StageHunk()
        {
            if (elements.Count == 0)
            {
                return;
            }

            Stage(true);
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

        private (int start, int end, int hunkIndex) GetStartLineNumberAndHunkIndex()
        {
            var currentHeaderIndex = headers.Last(index => index <= diff.Current);
            var header = diff[currentHeaderIndex].GetElement();
            var temp = header.Split("@@ ")[1].Split(",");
            var start = int.Parse(temp[0][1..]);
            var end = int.Parse(temp[1].Split(" ")[0]);
            return (start, start + end, currentHeaderIndex);
        }

        private void GeneratePatch(bool stageHunk, (string path, string name) patchData)
        {
            var indeexes = GetStartLineNumberAndHunkIndex();
            var lineNumber = indeexes.start + (diff.Current - indeexes.hunkIndex) + 1;
            var toWrite = GetFileData();
            if (!elements[diff.Current].GetElement().StartsWith("-") || !elements[diff.Current].GetElement().StartsWith("+"))
            {
                return;
            }

            var startLine = indeexes.start + (stageHunk ? 0 : lineNumber);

            for (int i = startLine; i < (stageHunk ? indeexes.end : startLine + 1); i++)
            {
                toWrite += diff[i].GetElement() + "\n";
            }

            System.IO.File.Create(patchData.path).Close();
            using (StreamWriter patchFileWriter = new StreamWriter(patchData.name))
            {
                patchFileWriter.Write(toWrite.TrimEnd());
                patchFileWriter.Close();
            }
        }

        private string GetFileData()
        {
            var result = "";
            for (int i = 0; i < headers[0]; i++)
            {
                result += diff[0].GetElement() + "\n";
            }

            return result;
        }

        private (string path, string name) GetPatchPathAndName()
        {
            string patchFileName = "patch_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".patch";

            return (new PageUtilities(repositoryPath).GetPreviousPath() + "/" + patchFileName, patchFileName);
        }

        private void Stage(bool stageHunk)
        {
            var patchData = GetPatchPathAndName();
            GeneratePatch(stageHunk, patchData);
            gitManager.Stage(patchData.path, repositoryPath);
            System.IO.File.Delete(patchData.path);
        }
    }
}