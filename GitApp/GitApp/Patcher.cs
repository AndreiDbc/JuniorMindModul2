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
        private readonly GitManager gitManager;
        private readonly string data;
        private readonly string repositoryPath;
        private readonly string fileName;

        public Patcher(string filePath, string repositoryPath, string fileName, GitManager gitManager, IElement hunk = null)
        {
            data = hunk == null ? "" : hunk.GetElement();
            this.filePath = filePath;
            this.gitManager = gitManager;
            this.repositoryPath = repositoryPath;
            this.fileName = fileName;
        }

        public void Stage(IElement commit)
        {
            string patchFileName = "patch_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".patch";
            System.IO.File.Create(repositoryPath + "/" + patchFileName).Close();
            using (StreamWriter patchFileWriter = new StreamWriter(patchFileName))
            {
                patchFileWriter.Write(WritePatchContent(commit));
                gitManager.Stage(patchFileName, repositoryPath);
                patchFileWriter.Close();
            }

            System.IO.File.Delete(repositoryPath + "/" + patchFileName);
        }

        public void StageAll()
        {
            gitManager.StageFile(repositoryPath, fileName);
        }

        private string WritePatchContent(IElement commit)
        {
            var commitData = commit.GetElement().Split("\n");
            var result = $"From {commitData[4]} {commitData[2][..10]}" + Environment.NewLine;
            result += $"From: {commitData[1]}" + Environment.NewLine;
            result += $"Date: {DateTime.Now.ToString("yyyyMMdd")}" + Environment.NewLine;
            result += "Subject: [PATCH] Contains updated data for the file." + Environment.NewLine;
            result += Environment.NewLine + data;
            if (data.EndsWith("\\ No newline at end of file"))
            {
                return result[..^27];
            }

            return result;
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