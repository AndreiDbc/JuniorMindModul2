using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitApp
{
    public class CommitModifiedFiles : IText
    {
        private readonly string commitID;

        public CommitModifiedFiles(string commitID)
        {
            this.commitID = commitID;
            Current = 0;
            Index = 0;
        }

        public int Current { get; set; }

        public int Index { get; set; }

        public List<string> GetText()
        {
            return new GitManager("git", new GitProcessRunner()).GetCommitModifiedFileNames(commitID).Split("\n").ToList();
        }
    }
}
