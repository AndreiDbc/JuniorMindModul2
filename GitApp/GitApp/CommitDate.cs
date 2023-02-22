using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitApp
{
    public class CommitDate
    {
        private readonly GitManager gitManager;

        public CommitDate(GitManager gitManager)
        {
            this.gitManager = gitManager;
        }

        public List<string> Dates { get => gitManager.GetCommitDate().Split("\n").ToList(); }
    }
}
