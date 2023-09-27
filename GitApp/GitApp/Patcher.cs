using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitApp
{
    public class Patcher
    {
        private readonly string filePath;
        private readonly Diff diff;
        private readonly GitManager gitManager;
        private string fileData;
        private string elementDiff;

        public Patcher(IElement hunk, string filePath)
        {
            var data = hunk.GetElement().Split("\n\x1b\n");
            fileData = data[0];
            elementDiff = data[1];
            this.filePath = filePath;
        }

        public Patcher(Diff diff, GitManager gitManager)
        {
            this.diff = diff;
            this.gitManager = gitManager;
        }
    }
}
