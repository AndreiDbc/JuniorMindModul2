using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linq
{
    class Merchandise
    {
        public string Name { get; set; }

        public ICollection<Feature> Features { get; set; }
    }
}
