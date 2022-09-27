using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Dictionary
{
    class Dictionary<TKey, TValue> : IDictionary<TKey, TValue>
    {
        private readonly int[] buckets;
        private Element<TKey, TValue>[] elements;
        private int freeIndex;
        private int count;

        public Dictionary(int length)
        {
            buckets = new int[length];
            SetBucketElementsToDefault();
            freeIndex = -1;
            count = 0;
            elements = new Element<TKey, TValue>[length];
        }

        public int Count { get => count; }

        public bool IsReadOnly { get; }

        public ICollection<TKey> Keys
        {
            get
            {
                TKey[] keys = new TKey[count];
                var enumerator = GetEnumerator();
                for (int i = 0; i < count && enumerator.MoveNext(); i++)
                {
                    keys[i] = enumerator.Current.Key;
                }

                return keys;
            }
        }

        public ICollection<TValue> Values
        {
            get
            {
                TValue[] values = new TValue[count];
                var enumerator = GetEnumerator();
                for (int i = 0; i < count && enumerator.MoveNext(); i++)
                {
                    values[i] = enumerator.Current.Value;
                }

                return values;
            }
        }

        public TValue this[TKey key]
        {
            get
            {
                int index = GetElementIndex(key);

                return index == -1 ? throw new KeyNotFoundException() : elements[index].Value;
            }

            set
            {
                int index = GetElementIndex(key);
                if (index == -1)
                {
                    Add(key, value);
                    return;
                }

                elements[index].Value = value;
            }
        }

        public void Add(TKey key, TValue value)
        {
            Add(new KeyValuePair<TKey, TValue>(key, value));
        }

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            if (ContainsKey(item.Key))
            {
                return;
            }

            var element = new Element<TKey, TValue>(item.Key, item.Value);
            int bucketLocation = GetBucketLocation(item.Key);
            Element<TKey, TValue> temp = new Element<TKey, TValue>(default, default);
            if (freeIndex != -1)
            {
                temp = elements[freeIndex];
            }

            element.Next = buckets[bucketLocation];
            elements[freeIndex == -1 ? count : freeIndex] = element;
            buckets[bucketLocation] = freeIndex == -1 ? count : freeIndex;
            freeIndex = temp.Next;
            count++;
        }

        public void Clear()
        {
            SetBucketElementsToDefault();
            count = 0;
            elements = new Element<TKey, TValue>[elements.Length];
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            int index = GetElementIndex(item.Key);
            return index != -1 && elements[index].Value.Equals(item.Value);
        }

        public bool ContainsKey(TKey key)
        {
            return GetElementIndex(key) != -1;
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            if (array == null)
            {
                throw new ArgumentNullException(nameof(array));
            }
            else if (arrayIndex < 0)
            {
                throw new ArgumentException("Index is outside of the bounds of the array.");
            }
            else if (Count > array.Length - arrayIndex)
            {
                throw new ArgumentException("The number of elements array is greater than the available space to the end of the destination array.");
            }

            var enumerator = GetEnumerator();
            for (int i = arrayIndex; i < array.Length && enumerator.MoveNext(); i++)
            {
                array[i] = enumerator.Current;
            }
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            int temp = 0;
            for (int i = 0; temp != count && i < elements.Length; i++)
            {
                if (!IsIndexFree(i))
                {
                    temp++;
                    yield return new KeyValuePair<TKey, TValue>(elements[i].Key, elements[i].Value);
                }
            }
        }

        public bool Remove(TKey key)
        {
            int index = GetElementIndex(key);
            if (index == -1)
            {
                return false;
            }

            var toRemove = elements[index];
            int location = GetBucketLocation(key);
            int temp = toRemove.Next;
            int previous = GetPreviousIndex(toRemove.Key);
            toRemove.Next = freeIndex;
            freeIndex = previous == -1 ? buckets[location] : index;
            toRemove.Value = default;
            toRemove.Key = default;
            buckets[location] = buckets[location].Equals(previous) ? temp : buckets[location];
            count--;

            return true;
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            return Remove(item.Key);
        }

        public bool TryGetValue(TKey key, [MaybeNullWhen(false)] out TValue value)
        {
            int index = GetElementIndex(key);
            if (index != -1)
            {
                value = elements[index].Value;
                return true;
            }

            value = default(TValue);
            return false;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private int GetBucketLocation(TKey key)
        {
            return Math.Abs(key.GetHashCode()) % elements.Length;
        }

        private void SetBucketElementsToDefault()
        {
            Array.Fill(buckets, -1);
        }

        private int GetElementIndex(TKey key)
        {
            int location = buckets[GetBucketLocation(key)];
            for (int i = location; i != -1; i = elements[i].Next)
            {
                if (elements[i].Key.Equals(key))
                {
                    return i;
                }
            }

            return -1;
        }

        private int GetPreviousIndex(TKey key)
        {
            int location = buckets[GetBucketLocation(key)];
            for (int i = location; i != -1 && !elements[location].Key.Equals(key); i = elements[i].Next)
            {
                if (elements[elements[i].Next].Key.Equals(key))
                {
                    return i;
                }
            }

            return -1;
        }

        private bool IsIndexFree(int index)
        {
            for (int i = freeIndex; i != -1; i = elements[i].Next)
            {
                if (i == index)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
