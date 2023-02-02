using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace GitApp
{
    class CommitOptions
    {
        private readonly List<string> commits;

        public CommitOptions(List<string> commits)
        {
            this.commits = commits;
        }
    }
}
