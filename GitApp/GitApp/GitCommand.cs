using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitApp
{
    public class GitCommand
    {
        public GitCommand(string fileName, string argument)
        {
            this.FileName = fileName;
            this.Argument = argument;
        }

        public string FileName { get; }

        public string Argument { get; }
    }
}
