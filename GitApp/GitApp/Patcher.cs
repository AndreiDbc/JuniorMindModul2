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
        private readonly string fileData;
        private readonly string elementDiff;
        private readonly string repositoryPath;

        public Patcher(IElement hunk, string filePath, string repositoryPath, GitManager gitManager)
        {
            var data = hunk.GetElement().Split("\n\x1b\n");
            fileData = data[0];
            elementDiff = data[1];
            this.filePath = filePath;
            this.gitManager = gitManager;
            this.repositoryPath = repositoryPath;
        }

        public Patcher(Diff diff, GitManager gitManager)
        {
            this.diff = diff;
            this.gitManager = gitManager;
        }

        public void Stage(IElement commit)
        {
            string patchFileName = "patch_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".patch";
            var path = GetPathWithoutFileName();
            System.IO.File.Create(path).Close();
            using (StreamWriter patchFileWriter = new StreamWriter(patchFileName))
            {
                patchFileWriter.Write(WritePatchContent(commit));
                gitManager.Stage(patchFileName);
                patchFileWriter.Close();
            }

            System.IO.File.Delete(path + patchFileName);
        }

        public void StageAll()
        {
            gitManager.StageFile(repositoryPath);
        }

        private string WritePatchContent(IElement commit)
        {
            var commitData = commit.GetElement().Split("\n");
            var result = $"From {commitData[4]} {commitData[2]}" + Environment.NewLine;
            result += $"From: {commitData[1]}" + Environment.NewLine;
            result += $"Date: {DateTime.Now.ToString("yyyyMMdd")}" + Environment.NewLine;
            result += "Subject: [PATCH] Contains updated data for the file." + Environment.NewLine;
            result += Environment.NewLine + fileData + Environment.NewLine + elementDiff;
            return result;
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

            return int.Parse(result[1..]);
        }

        private string GetPathWithoutFileName()
        {
            for (int i = filePath.Length - 1; i > -1; i--)
            {
                if (filePath[i] == '/')
                {
                    return filePath[..(i + 1)];
                }
            }

            return "";
        }
    }
}