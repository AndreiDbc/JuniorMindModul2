using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace GitApp
{
    public class InitialData
    {
        private readonly GitManager manager;

        public InitialData(GitManager manager)
        {
            this.manager = manager;
        }
    }
}
