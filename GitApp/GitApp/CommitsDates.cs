using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitApp
{
    public class CommitsDates
    {
        private readonly List<string> dateList;

        public CommitsDates(List<string> dateList)
        {
            this.dateList = dateList;
        }

        public string GetDateAtIndex(int index)
        {
            return dateList[index];
        }
    }
}
