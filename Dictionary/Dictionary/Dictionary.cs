using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                int temp = 0;
                for (int i = 0; i < elements.Length && temp != count; i++)
                {
                    if (!elements[i].Key.Equals(default(TKey)))
                    {
                        keys[temp] = elements[i].Key;
                        temp++;
                    }
                }

                return keys;
            }
        }

        public ICollection<TValue> Values
        {
            get
            {
                TValue[] values = new TValue[count];
                int temp = 0;
                for (int i = 0; i < elements.Length && temp != count; i++)
                {
                    if (!elements[i].Key.Equals(default(TKey)))
                    {
                        values[temp] = elements[i].Value;
                        temp++;
                    }
                }

                return values;
            }
        }

        public TValue this[TKey key]
        {
            get
            {
                if (!ContainsKey(key))
                {
                    throw new KeyNotFoundException();
                }

                return GetElement(key).Value;
            }

            set
            {
                if (!ContainsKey(key))
                {
                    Add(key, value);
                    return;
                }

                GetElement(key).Value = value;
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
            freeIndex = freeIndex == -1 ? -1 : temp.Next;
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
            Element<TKey, TValue> element = GetElement(item.Key);
            return element?.Key.Equals(item.Key) == true && element.Value.Equals(item.Value);
        }

        public bool ContainsKey(TKey key)
        {
            return GetElement(key) != null;
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            for (int i = 0, j = count; i < elements.Length && j > 0; i++, j--)
            {
                array[arrayIndex] = new (elements[i].Key, elements[i].Value);
                arrayIndex++;
            }
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            foreach (var element in elements)
            {
                yield return new KeyValuePair<TKey, TValue>(element.Key, element.Value);
            }
        }

        public bool Remove(TKey key)
        {
            if (!ContainsKey(key))
            {
                return false;
            }

            var toRemove = GetElement(key);
            toRemove.Key = default;
            toRemove.Value = default;
            int temp = toRemove.Next;
            toRemove.Next = freeIndex;
            buckets[GetBucketLocation(key)] = temp;
            count--;

            return true;
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            return Remove(item.Key);
        }

        public bool TryGetValue(TKey key, [MaybeNullWhen(false)] out TValue value)
        {
            Element<TKey, TValue> element = GetElement(key);
            if (element != null)
            {
                value = element.Value;
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

        private Element<TKey, TValue> GetElement(TKey key)
        {
            int location = buckets[GetBucketLocation(key)];
            for (int i = location; i != -1; i = elements[i].Next)
            {
                if (elements[i].Key.Equals(key))
                {
                    return elements[i];
                }
            }

            return null;
        }
    }
}
