using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitApp
{
    public class CommitsAuthorsName
    {
        private readonly List<string> nameList;

        public CommitsAuthorsName(List<string> nameList)
        {
            this.nameList = nameList;
        }

        public string GetNameAtIndex(int index)
        {
            return nameList[index];
        }
    }
}
