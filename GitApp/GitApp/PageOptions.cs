using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitApp
{
    public class PageOptions
    {
        private readonly IText text;

        public PageOptions(IText text)
        {
            this.text = text;
        }
    }
}
