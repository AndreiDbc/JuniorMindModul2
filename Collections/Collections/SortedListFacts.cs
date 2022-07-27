using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Collections
{
    public class SortedListFacts
    {
        [Fact]
        public void AnInstanceOfIntSortedListIsCreatedValuesAreSetUsingIndeexerShouldReturnSortedValues()
        {
            SortedList<int> test = new SortedList<int>();
            test.Add(1);
            test.Add(6);
            test.Add(8);
            test[0] = 0;
            test[1] = 5;
            test[2] = 7;
            test[3] = 2;
            Assert.Equal(0, test[0]);
            Assert.Equal(5, test[1]);
            Assert.Equal(7, test[2]);
            Assert.Equal(0, test[3]);
        }

        [Fact]
        public void AddIsCalledShoudReturnSortedElements()
        {
            SortedList<string> test = new SortedList<string>();
            test.Add("av");
            test.Add("bc");
            test.Add("ad");
            Assert.Equal("ad", test[0]);
            Assert.Equal("av", test[1]);
            Assert.Equal("bc", test[2]);
        }

        [Fact]
        public void InsertisCalledShouldAddElementToList()
        {
            SortedList<string> test = new SortedList<string>();
            test.Add("av");
            test.Add("bc");
            test.Add("ad");
            test.Insert(1, "af");
            Assert.Equal("ad", test[0]);
            Assert.Equal("af", test[1]);
            Assert.Equal("av", test[2]);
            Assert.Equal("bc", test[3]);
        }
    }
}
