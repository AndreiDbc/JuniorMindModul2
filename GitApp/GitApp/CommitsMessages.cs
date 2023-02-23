using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitApp
{
    public class CommitsMessages
    {
        private readonly List<string> commitList;

        public CommitsMessages(List<string> commitList)
        {
            this.commitList = commitList;
        }

        public string GetMessageAtIndex(int index)
        {
            return commitList[index];
        }
    }
}
