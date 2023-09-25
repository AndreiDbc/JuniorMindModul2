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
        private readonly string fileData;
        private readonly string index;

        public Hunk(string rawData, string fileData, string index)
        {
            this.rawData = rawData;
            this.fileData = fileData;
            this.index = index;
        }

        public string Id { get => index; }

        public string GetElement()
        {
            return fileData + "\n\x1b\n" + rawData;
        }
    }
}
