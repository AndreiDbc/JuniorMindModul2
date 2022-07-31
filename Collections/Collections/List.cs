using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Collections
{
    class List<T> : ICollection<T>
    {
        private T[] array;
        private int count;

        public List()
        {
            array = new T[4];
            count = 0;
        }

        public int Count { get => count; }

        public bool IsReadOnly { get; }

        public virtual T this[int index]
        {
            get => array[index];
            set => array[index] = value;
        }

        public void CopyTo(T[] arrayToAdd, int arrayIndex)
        {
            try
            {
                Array.Copy(array, 0, arrayToAdd, arrayIndex, count);
            }
            catch (ArgumentNullException)
            {
                throw new ArgumentException("Array is null.");
            }
            catch (IndexOutOfRangeException)
            {
                throw new ArgumentException("Index is less than 0.");
            }
            catch (ArgumentException)
            {
                throw new ArgumentException("The number of elements array is greater than the available space to the end of the destination array.");
            }

            Array.Copy(array, 0, arrayToAdd, arrayIndex, count);
        }

        public virtual void Add(T element)
        {
            try
            {
                array[count] = element;
            }
            catch (NotSupportedException)
            {
                throw new NotSupportedException("Array is Read-Only.");
            }

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
            try
            {
                count = 0;
            }
            catch (NotSupportedException)
            {
                throw new NotSupportedException("Array is Read-Only.");
            }

            count = 0;
        }

        public virtual bool Remove(T element)
        {
            int index = IndexOf(element);
            try
            {
                ShiftToLeft(index);
            }
            catch (NotSupportedException)
            {
                throw new NotSupportedException("Array is Read-Only.");
            }

            if (index == -1)
            {
                return false;
            }

            ShiftToLeft(index);
            count--;

            return true;
        }

        public void RemoveAt(int index)
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
