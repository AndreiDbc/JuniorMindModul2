using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitApp
{
    public class GitCommand
    {
        public GitCommand(string fileName, string argument, string path = "", MemoryStream streamData = null)
        {
            this.FileName = fileName;
            this.Argument = argument;
            this.Path = path;
            this.StreamData = streamData;
        }

        public string FileName { get; }

        public string Argument { get; }

        public string Path { get; }

        public MemoryStream StreamData { get; }
    }
}