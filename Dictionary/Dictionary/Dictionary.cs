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
        private int count;

        public Dictionary(int length)
        {
            buckets = new int[length];
            SetBucketElementsToDefault();
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
                for (int i = 0; i < count; i++)
                {
                    keys[i] = elements[i].Key;
                }

                return keys;
            }
        }

        public ICollection<TValue> Values
        {
            get
            {
                TValue[] values = new TValue[count];
                for (int i = 0; i < count; i++)
                {
                    values[i] = elements[i].Value;
                }

                return values;
            }
        }

        public TValue this[TKey key]
        {
            get
            {
                int location = buckets[GetBucketLocation(key)];
                for (int i = location; i != -1; i = elements[location].Next)
                {
                    if (elements[i].Key.Equals(key))
                    {
                        return elements[i].Value;
                    }
                }

                return default;
            }

            set
            {
                int location = buckets[GetBucketLocation(key)];
                for (int i = location; i != -1; i = elements[location].Next)
                {
                    if (elements[i].Key.Equals(key))
                    {
                        elements[i].Value = value;
                    }
                }
            }
        }

        public void Add(TKey key, TValue value)
        {
            Add(new KeyValuePair<TKey, TValue>(key, value));
        }

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            var element = new Element<TKey, TValue>(item.Key, item.Value);
            int bucketLocation = GetBucketLocation(item.Key);
            element.Next = buckets[bucketLocation];
            elements[count] = element;
            buckets[bucketLocation] = count;
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
            int location = buckets[GetBucketLocation(item.Key)];
            for (int i = location; i != -1; i = elements[location].Next)
            {
                if (elements[i].Key.Equals(item.Key) && elements[i].Value.Equals(item.Value))
                {
                    return true;
                }
            }

            return false;
        }

        public bool ContainsKey(TKey key)
        {
            int location = buckets[GetBucketLocation(key)];
            for (int i = location; i != -1; i = elements[location].Next)
            {
                if (elements[i].Key.Equals(key))
                {
                    return true;
                }
            }

            return false;
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            for (int i = 0; i < elements.Length; i++)
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
            int location = buckets[GetBucketLocation(key)];
            for (int i = location; i != -1; i = elements[location].Next)
            {
                if (elements[i].Key.Equals(key))
                {
                    buckets[i] = elements[i].Next;
                    elements[i] = null;
                    return true;
                }
            }

            return false;
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            return Remove(item.Key);
        }

        public bool TryGetValue(TKey key, [MaybeNullWhen(false)] out TValue value)
        {
            int location = buckets[GetBucketLocation(key)];
            for (int i = location; i != -1; i = elements[location].Next)
            {
                if (elements[i].Key.Equals(key))
                {
                    value = elements[i].Value;
                    return true;
                }
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
    }
}
