using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitApp
{
    public class Diff
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
    }
}
