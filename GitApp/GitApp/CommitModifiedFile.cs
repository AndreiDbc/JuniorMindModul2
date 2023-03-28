using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitApp
{
    public class CommitModifiedFile : IElement
    {
        private readonly string data;

        public CommitModifiedFile(string data)
        {
            this.data = data;
        }

        public string GetElement()
        {
            return data;
        }
    }
}
