using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitApp
{
    public class CommitsDates
    {
        private readonly GitManager gitManager;

        public CommitsDates(GitManager gitManager)
        {
            this.gitManager = gitManager;
        }

        public List<string> Dates { get => gitManager.GetCommitDate().Split("\n").ToList(); }
    }
}
