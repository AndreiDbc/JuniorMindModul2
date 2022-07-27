using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Collections
{
    class List<T> : IEnumerable<T>
    {
        private T[] array;
        private int count;

        public List()
        {
            array = new T[4];
            count = 0;
        }

        public int Count { get => count; }

        public virtual T this[int index]
        {
            get => array[index];
            set => array[index] = value;
        }

        public virtual void Add(T element)
        {
            DoubleTheArraySize();

            array[count] = element;
            count++;
        }

        public int IndexOf(T element)
        {
            for (int i = 0; i < count; i++)
            {
                if (array[i].Equals(element))
                {
                    return i;
                }
            }

            return -1;
        }

        public bool Contains(T element)
        {
            return IndexOf(element) != -1;
        }

        public virtual void Insert(int index, T element)
        {
            DoubleTheArraySize();

            ShiftToRight(index);
            array[index] = element;
            count++;
        }

        public void Clear()
        {
            count = 0;
        }

        public virtual void Remove(T element)
        {
            int index = IndexOf(element);
            if (index == -1)
            {
                return;
            }

            ShiftToLeft(index);
            count--;
        }

        public virtual void RemoveAt(int index)
        {
            ShiftToLeft(index);
            count--;
        }

        public IEnumerator<T> GetEnumerator()
        {
            for (int index = 0; index < count; index++)
            {
                yield return array[index];
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private void ShiftToRight(int index)
        {
            for (int i = count; i > index; i--)
            {
                array[i] = array[i - 1];
            }
        }

        private void ShiftToLeft(int index)
        {
            for (int i = index; i < count - 1; i++)
            {
                array[i] = array[i + 1];
            }
        }

        private void DoubleTheArraySize()
        {
            if (count != array.Length)
            {
                return;
            }

            Array.Resize(ref array, array.Length * 2);
        }
    }
}
