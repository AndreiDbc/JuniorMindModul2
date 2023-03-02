using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitApp
{
    public class FullCommits : IText
    {
        private readonly CommitsIDs commitsIDs;
        private readonly CommitsAuthorsName commitsAuthorsNames;
        private readonly CommitsDates commitsDates;
        private readonly CommitsMessages commitsMessages;

        public FullCommits(
            CommitsIDs commitsIDs,
            CommitsAuthorsName commitsAuthorsNames,
            CommitsDates commitsDates,
            CommitsMessages commitsMessages)
        {
            this.commitsIDs = commitsIDs;
            this.commitsAuthorsNames = commitsAuthorsNames;
            this.commitsDates = commitsDates;
            this.commitsMessages = commitsMessages;
            Current = 0;
            Index = 0;
        }

        public int Current { get; set; }

        public int Index { get; set; }
    }
}
