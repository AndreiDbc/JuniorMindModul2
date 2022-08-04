using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Collections
{
    class ReadOnlyList<T>
    {
        private readonly List<T> list;

        public ReadOnlyList(List<T> list)
        {
            this.list = list;
        }

        public int Count => list.Count;

        public T this[int index]
        {
            get => list[index];
            set
            {
                throw new NotSupportedException("List is Readonly.");
            }
        }

        public void CopyTo(T[] arrayToAdd, int arrayIndex)
        {
            list.CopyTo(arrayToAdd, arrayIndex);
        }

        public bool Contains(T element)
        {
            return list.Contains(element);
        }

        public int IndexOf(T element)
        {
            return list.IndexOf(element);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return list.GetEnumerator();
        }

        public void Add(T element)
        {
            throw new NotSupportedException("List is Readonly.");
        }

        public bool Remove(T element)
        {
            throw new NotSupportedException("List is Readonly.");
        }

        public void RemoveAt(int index)
        {
            throw new NotSupportedException("List is Readonly.");
        }

        public void Insert(int index, T element)
        {
            throw new NotSupportedException("List is Readonly.");
        }

        public void Clear()
        {
            throw new NotSupportedException("List is Readonly.");
        }
    }
}
