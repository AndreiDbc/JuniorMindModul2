using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitApp
{
    public class DiffLine : IElement
    {
        private readonly string rawDiff;
        private readonly string index;

        public DiffLine(string rawDiff, string index)
        {
            this.index = index;
            this.rawDiff = rawDiff;
        }

        public string Id
        {
            get => index;
        }

        public string GetElement()
        {
            return rawDiff;
        }
    }
}
