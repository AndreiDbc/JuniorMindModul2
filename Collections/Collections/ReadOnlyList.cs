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

        public override int Count => list.Count;

        public override T this[int index]
        {
            get => list[index];
            set
            {
                throw new NotSupportedException("List is Readonly.");
            }
        }

        public override void CopyTo(T[] arrayToAdd, int arrayIndex)
        {
            list.CopyTo(arrayToAdd, arrayIndex);
        }

        public override bool Contains(T element)
        {
            return list.Contains(element);
        }

        public override int IndexOf(T element)
        {
            return list.IndexOf(element);
        }

        public override IEnumerator<T> GetEnumerator()
        {
            return list.GetEnumerator();
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
