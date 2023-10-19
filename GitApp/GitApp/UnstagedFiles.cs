using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace GitApp
{
    public class UnstagedFiles : ModifiedFiles
    {
        private bool[] stagedFiles;

        public UnstagedFiles(string unstagedRawData) : base(unstagedRawData)
        {
        }

        protected override List<IElement> GetElements()
        {
            var result = new List<IElement>();
            var files = rawData.Split("\n");
            InitializeArrayForStagedFiles(files);
            for (int i = 0; i < files.Length; i++)
            {
                var item = files[i];
                if (item.StartsWith("M") || item.StartsWith("AD"))
                {
                    stagedFiles[i] = true;
                    continue;
                }

                result.Add(new File(item, i.ToString()));
            }

            return result;
        }

        private void InitializeArrayForStagedFiles(string[] files)
        {
            if (stagedFiles != null)
            {
                return;
            }

            stagedFiles = new bool[files.Length];
            Array.Fill(stagedFiles, false);
        }
    }
}
