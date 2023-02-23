using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitApp
{
    public class CommitsIDs
    {
        private readonly List<string> listOfIDs;

        public CommitsIDs(List<string> listOfIDs)
        {
            this.listOfIDs = listOfIDs;
        }

        public string GetIDAtIndex(int index)
        {
            return listOfIDs[index];
        }
    }
}
