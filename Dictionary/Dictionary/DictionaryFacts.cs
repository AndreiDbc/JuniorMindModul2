using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Dictionary
{
    public class DictionaryFacts
    {
        [Fact]
        public void AddIsCalledShouldAddOneElementToDictionary()
        {
            Dictionary<int, string> dictionary = new (1);
            dictionary.Add(1, "a");
            ICollection<int> keys = dictionary.Keys;
            ICollection<string> values = dictionary.Values;
            Assert.Equal(1, dictionary.Count);
            Assert.Equal("a", values.First());
            Assert.Equal(1, keys.First());
        }

        [Fact]
        public void AddIsCalledMultipleTimesShouldUpdateDictionary()
        {
            Dictionary<int, string> dictionary = new (3);
            dictionary.Add(1, "a");
            dictionary.Add(2, "b");
            dictionary.Add(3, "c");
            ICollection<int> keys = dictionary.Keys;
            ICollection<string> values = dictionary.Values;
            Assert.Equal(3, dictionary.Count);
            Assert.Equal("a", values.First());
            Assert.Equal(1, keys.First());
            Assert.Equal("b", values.ElementAt(1));
            Assert.Equal(2, keys.ElementAt(1));
            Assert.Equal("c", values.ElementAt(2));
            Assert.Equal(3, keys.ElementAt(2));
        }

        [Fact]
        public void AddIsCalledMultipleTimesUsingKeyValuePairShouldUpdateDictionary()
        {
            Dictionary<int, string> dictionary = new (3);
            KeyValuePair<int, string> kvp1 = new KeyValuePair<int, string>(3, "a");
            KeyValuePair<int, string> kvp2 = new KeyValuePair<int, string>(6, "b");
            KeyValuePair<int, string> kvp3 = new KeyValuePair<int, string>(9, "c");
            dictionary.Add(kvp1);
            dictionary.Add(kvp2);
            dictionary.Add(kvp3);
            ICollection<int> keys = dictionary.Keys;
            ICollection<string> values = dictionary.Values;
            Assert.Equal("a", values.First());
            Assert.Equal(3, keys.First());
            Assert.Equal("b", values.ElementAt(1));
            Assert.Equal(6, keys.ElementAt(1));
            Assert.Equal("c", values.ElementAt(2));
            Assert.Equal(9, keys.ElementAt(2));
        }

        [Fact]
        public void ClearIsCalledShouldReturnDictionaryToDefaultState()
        {
            Dictionary<int, string> dictionary = new (3);
            dictionary.Add(1, "a");
            dictionary.Add(2, "b");
            dictionary.Add(3, "c");
            dictionary.Clear();
            ICollection<int> keys = dictionary.Keys;
            ICollection<string> values = dictionary.Values;
            Assert.Equal(0, dictionary.Count);
            Assert.Equal(0, keys.Count);
            Assert.Equal(0, values.Count);
        }

        [Fact]
        public void ContainsIsCalledShouldReturnFalse()
        {
            Dictionary<int, string> dictionary = new (3);
            dictionary.Add(1, "a");
            dictionary.Add(2, "b");
            dictionary.Add(3, "c");
            KeyValuePair<int, string> test = new KeyValuePair<int, string>(5, "z");
            Assert.False(dictionary.Contains(test));
        }

        [Fact]
        public void ContainsIsCalledShouldReturnTrue()
        {
            Dictionary<int, string> dictionary = new (3);
            dictionary.Add(1, "a");
            dictionary.Add(2, "b");
            dictionary.Add(3, "c");
            KeyValuePair<int, string> test = new KeyValuePair<int, string>(2, "b");
            Assert.True(dictionary.Contains(test));
        }

        [Fact]
        public void ContainsKeyIsCalledShouldReturnTrue()
        {
            Dictionary<int, string> dictionary = new (3);
            dictionary.Add(1, "a");
            dictionary.Add(2, "b");
            dictionary.Add(3, "c");
            Assert.True(dictionary.ContainsKey(3));
        }

        [Fact]
        public void ContainsKeyIsCalledShouldReturnFalse()
        {
            Dictionary<int, string> dictionary = new (3);
            dictionary.Add(1, "a");
            dictionary.Add(2, "b");
            dictionary.Add(3, "c");
            Assert.False(dictionary.ContainsKey(9));
        }

        [Fact]
        public void CopyToIsCalledShouldCopyElementsToArray()
        {
            Dictionary<int, string> dictionary = new (3);
            dictionary.Add(1, "a");
            dictionary.Add(2, "b");
            dictionary.Add(3, "c");
            KeyValuePair<int, string>[] test = new KeyValuePair<int, string>[3];
            KeyValuePair<int, string>[] toCompare = new[]
            {
                new KeyValuePair<int, string>(1, "a"), new KeyValuePair<int, string>(2, "b"), new KeyValuePair<int, string>(3, "c")
            };
            dictionary.CopyTo(test, 0);
            Assert.Equal(test, toCompare);
        }

        [Fact]
        public void AnEnumeratorIsCreatedMoveNextIsCalledShouldReturnFalse()
        {
            Dictionary<int, string> dictionary = new (3);
            dictionary.Add(1, "a");
            dictionary.Add(2, "b");
            dictionary.Add(3, "c");
            IEnumerator enumerator = dictionary.GetEnumerator();
            enumerator.MoveNext();
            enumerator.MoveNext();
            enumerator.MoveNext();
            Assert.Equal(new KeyValuePair<int, string>(3, "c"), enumerator.Current);
            Assert.False(enumerator.MoveNext());
        }

        [Fact]
        public void TryGetValueIsCalledShouldReturnFalse()
        {
            Dictionary<int, string> dictionary = new (3);
            dictionary.Add(1, "a");
            dictionary.Add(2, "b");
            dictionary.Add(3, "c");
            var value = "";
            dictionary.TryGetValue(4, out value);
            Assert.Equal(null, value);
            Assert.False(dictionary.TryGetValue(4, out value));
        }

        [Fact]
        public void TryGetValueIsCalledShouldReturnTrue()
        {
            Dictionary<int, string> dictionary = new (3);
            dictionary.Add(1, "a");
            dictionary.Add(2, "b");
            dictionary.Add(3, "c");
            var value = "";
            dictionary.TryGetValue(3, out value);
            Assert.Equal("c", value);
            Assert.True(dictionary.TryGetValue(3, out value));
        }
    }
}