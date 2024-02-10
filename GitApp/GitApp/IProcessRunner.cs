using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitApp
{
    public interface IProcessRunner
    {
        public string RunCommand(GitCommand command);

        public void ApplyCommand(GitCommand command, string repositoryPath);
    }
}
