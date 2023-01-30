using RepositoryMock;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitAppFacts
{
    public class GitRepository
    {
        private readonly Dictionary<string, List<string>> branches;
        private string branch;

        public GitRepository(Dictionary<string, List<string>> branches, string branch)
        {
            this.branches = branches;
            this.branch = branch;
        }
    }
}
