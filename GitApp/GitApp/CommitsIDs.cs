using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitApp
{
    public class CommitsIDs
    {
        private readonly GitManager gitManager;

        public CommitsIDs(GitManager gitManager)
        {
            this.gitManager = gitManager;
        }

        public List<string> IDs { get => gitManager.GetCommitId().Split("\n").ToList(); }
    }
}
