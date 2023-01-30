using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryMock
{
    public interface IGitRepository
    {
        bool Commit(string commit);

        bool Checkout(string newBranch);

        bool Push(string location);

        bool Pull(List<string> commits);
    }
}
