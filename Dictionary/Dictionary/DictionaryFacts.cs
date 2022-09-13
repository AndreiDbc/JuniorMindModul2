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
    }
}