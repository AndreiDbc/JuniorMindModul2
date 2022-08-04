using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Collections
{
    class ReadOnlyList<T> : List<T>
    {
        private readonly List<T> list;

        public ReadOnlyList(List<T> list)
        {
            this.list = list;
        }

        public override T this[int index]
        {
            get => list[index];
            set
            {
                throw new NotSupportedException("List is Readonly.");
            }
        }

        public override void Add(T element)
        {
            throw new NotSupportedException("List is Readonly.");
        }

        public override bool Remove(T element)
        {
            throw new NotSupportedException("List is Readonly.");
        }

        public override void RemoveAt(int index)
        {
            throw new NotSupportedException("List is Readonly.");
        }

        public override void Insert(int index, T element)
        {
            throw new NotSupportedException("List is Readonly.");
        }

        public override void Clear()
        {
            throw new NotSupportedException("List is Readonly.");
        }
    }
}
