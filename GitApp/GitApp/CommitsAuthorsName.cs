using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitApp
{
    public class CommitsAuthorsName
    {
        private readonly GitManager gitManager;

        public CommitsAuthorsName(GitManager gitManager)
        {
            this.gitManager = gitManager;
        }

        public List<string> Names { get => gitManager.GetCommitAuthorName().Split("\n").ToList(); }
    }
}
