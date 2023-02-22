using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitApp
{
    public class CommitID
    {
        private readonly GitManager gitManager;

        public CommitID(GitManager gitManager)
        {
            this.gitManager = gitManager;
        }

        public List<string> IDs { get => gitManager.GetCommitId().Split("\n").ToList(); }
    }
}
