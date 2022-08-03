using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Collections
{
    class ReadOnlyList<T> : List<T>
    {
        public override T this[int index]
        {
            get => base[index];
            set
            {
                throw new NotSupportedException("List is Readonly.");
            }
        }

        public override void Add(T element)
        {
            throw new NotSupportedException("List is Readonly.");
        }
    }
}
