using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitApp
{
    public class FullCommits
    {
        private readonly string rawLogData;
        private readonly List<Commit> list;

        public FullCommits(string rawLogData)
        {
            this.rawLogData = rawLogData;
            Current = 0;
            Index = 0;
            list = GetText();
        }

        public int Current { get; set; }

        public int Index { get; set; }

        public Commit this[int index]
        {
            get => list[index];
        }

        public List<Commit> GetText()
        {
            var commits = new List<Commit>();
            var str = rawLogData.Split("\n\n");
            for (int i = 0; i < str.Length; i += 2)
            {
                commits.Add(new Commit(str[i] + "\n" + str[i + 1].Trim()));
            }

            return commits;
        }
    }
}