using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Linq
{
    public class LinqFacts
    {
        [Fact]
        public void AllIsCalledShouldReturnTrue()
        {
            IEnumerable<int> list = new List<int>() { 1, 2, 3 };
            Assert.True(list.All(val => val < 5));
        }

        [Fact]
        public void AllIsCalledShouldReturnFalse()
        {
            IEnumerable<int> list = new List<int>() { 1, 6, 3 };
            Assert.False(list.All(val => val < 5));
        }

        [Fact]
        public void AnyIsCalledShouldReturnTrue()
        {
            IEnumerable<int> list = new List<int>() { 1, 2, 3 };
            Assert.True(list.Any(val => val == 2));
        }

        [Fact]
        public void AnyIsCalledShouldReturnFalse()
        {
            IEnumerable<int> list = new List<int>() { 1, 2, 3 };
            Assert.False(list.Any(val => val == 6));
        }

        [Fact]
        public void FirstIsCalledShouldReturnItem()
        {
            IEnumerable<int> list = new List<int>() { 1, 2, 3 };
            Assert.Equal(2, list.First(val => val > 1));
        }

        [Fact]
        public void FirstIsCalledShouldThrowException()
        {
            IEnumerable<int> list = new List<int>() { 1, 2, 3 };
            Assert.Throws<InvalidOperationException>(() => list.First(val => val > 3));
        }
    }
}