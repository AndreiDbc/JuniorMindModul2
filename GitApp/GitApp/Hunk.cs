using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//test

namespace GitApp
{
    public class Hunk : IElement
    {
        private readonly string rawData;
        private readonly string index;

        public Hunk(string rawData, string index)
        {
            this.rawData = rawData;
        }

        public string Id { get => index; }

        public string GetElement()
        {
            return rawData;
        }
    }
}
