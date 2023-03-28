using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitApp
{
    public interface IText
    {
        public int Index { get; set; }

        public int Current { get; set; }

        public List<IElement> GetText();
    }
}
