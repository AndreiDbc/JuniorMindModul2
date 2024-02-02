using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitApp
{
    public class Hunk : IElement
    {
        private readonly string rawData;
        private readonly string index;

        public Hunk(string rawData, string index)//test
        {
            this.rawData = rawData;
            this.index = index;
        }

        public string Id { get => index; }

        public string GetElement()
        {
            return rawData;
        }
    }
}
