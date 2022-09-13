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
        private readonly Element<TKey, TValue>[] elements;
        private readonly int count;

        public Dictionary()
        {
            buckets = new int[5];
            count = 0;
            elements = new Element<TKey, TValue>[count];
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
                foreach (var element in elements)
                {
                    if (element.Key.Equals(key))
                    {
                        return element.Value;
                    }
                }

                return default(TValue);
            }

            set
            {
                foreach (var element in elements)
                {
                    if (element.Key.Equals(key))
                    {
                        element.Value = value;
                    }
                }
            }
        }

        public void Add(TKey key, TValue value)
        {
            throw new NotImplementedException();
        }

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            throw new NotImplementedException();
        }

        public bool ContainsKey(TKey key)
        {
            throw new NotImplementedException();
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public bool Remove(TKey key)
        {
            throw new NotImplementedException();
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            throw new NotImplementedException();
        }

        public bool TryGetValue(TKey key, [MaybeNullWhen(false)] out TValue value)
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}
