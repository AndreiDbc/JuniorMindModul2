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

        public DiffLine(string rawDiff)
        {
            this.rawDiff = rawDiff;
        }

        public string Id
        {
            get => rawDiff;
        }

        public string GetElement()
        {
            return rawDiff;
        }
    }
}
