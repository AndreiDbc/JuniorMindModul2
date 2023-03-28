using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitApp
{
    public class FullCommits : IText
    {
        private readonly string rawLogData;

        public FullCommits(string rawLogData)
        {
            this.rawLogData = rawLogData;
            Current = 0;
            Index = 0;
        }

        public int Current { get; set; }

        public int Index { get; set; }

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
    }
}