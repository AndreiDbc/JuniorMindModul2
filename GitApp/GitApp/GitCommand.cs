using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitApp
{
    public class GitCommand
    {
        public readonly string FileName;
        public readonly string Argument;

        public GitCommand(string fileName, string argument)
        {
            this.FileName = fileName;
            this.Argument = argument;
        }
    }
}
