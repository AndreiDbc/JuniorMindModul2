using GitAppFacts;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace GitApp
{
    public class GitManager
    {
        private readonly string fileName;
        private readonly GitProcessRunner commandRunner;

        public GitManager(string fileName, GitProcessRunner processRunner)
        {
            this.fileName = fileName;
            this.commandRunner = processRunner;
        }
    }
}
