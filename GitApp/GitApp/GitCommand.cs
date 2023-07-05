using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitApp
{
    public class GitCommand
    {
        public GitCommand(string fileName, string argument, string path = "")
        {
            this.FileName = fileName;
            this.Argument = argument;
            this.Path = path;
        }

        public string FileName { get; }

        public string Argument { get; }

        public string Path { get; }
    }
}
