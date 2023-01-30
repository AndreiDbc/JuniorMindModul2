using GitAppFacts;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitApp
{
    public class GitCommand
    {
        private readonly string branchName;
        private readonly GitRepository repository;

        public GitCommand(string branchName, GitRepository repository)
        {
            this.branchName = branchName;
            this.repository = repository;
        }
    }
}
