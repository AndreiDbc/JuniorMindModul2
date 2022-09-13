using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dictionary
{
    class Element<TKey, TValue>
    {
        public Element(TKey key, TValue data)
        {
            this.Value = data;
            this.Key = key;
            Next = -1;
        }

        public int Next { get; set; }

        public TValue Value { get; set; }

        public TKey Key { get; set; }
    }
}
