using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitApp
{
    public class CommitMessage
    {
        private readonly GitManager gitManager;

        public CommitMessage(GitManager gitManager)
        {
            this.gitManager = gitManager;
        }

        public List<string> Messages { get => gitManager.GetCommitMessage().Split("\n").ToList(); }
    }
}
