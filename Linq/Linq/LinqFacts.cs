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
            Assert.True(new List<int>() { 1, 2, 3 }.All(val => val < 5));
        }

        [Fact]
        public void AllIsCalledShouldReturnFalse()
        {
            Assert.False(new List<int>() { 1, 6, 3 }.All(val => val < 5));
        }

        [Fact]
        public void AnyIsCalledShouldReturnTrue()
        {
            Assert.True(new List<int>() { 1, 2, 3 }.Any(val => val == 2));
        }

        [Fact]
        public void AnyIsCalledShouldReturnFalse()
        {
            Assert.False(new List<int>() { 1, 2, 3 }.Any(val => val == 6));
        }

        [Fact]
        public void FirstIsCalledShouldReturnItem()
        {
            Assert.Equal(2, new List<int>() { 1, 2, 3 }.First(val => val > 1));
        }

        [Fact]
        public void FirstIsCalledShouldThrowException()
        {
            Assert.Throws<InvalidOperationException>(() => new List<int>() { 1, 2, 3 }
            .First(val => val > 3));
        }

        [Fact]
        public void SelectIsCalledShouldReturnStringToLower()
        {
            Assert.Equal(new[] { "asd", "dds", "sdsd" }, new List<string>() { "ASd", "dDS", "sdsD" }
            .Select(element => element.ToLower()));
        }

        [Fact]
        public void SelectManyIsCalledShouldReturnCharacters()
        {
            Assert.Equal(new[] { 'A', 'S', 'd', 'd', 'D', 'S' }, new List<string>() { "ASd", "dDS" }
            .SelectMany(x => x));
        }

        [Fact]
        public void WhereIsCalledShouldReturnNumbers()
        {
            Assert.Equal(new[] { 1, 2, 3 }, new[] { 1, 2, 3, 4 }.Where(number => number < 4));
        }

        [Fact]
        public void ToDictionaryIsCalledShouldConvertListToDictionary()
        {
            Assert.Equal(new Dictionary<char, char>().GetType(), new List<char>() { 'a', 'b', 'c' }
            .ToDictionary(x => x).GetType());
        }

        [Fact]
        public void ToDictionaryIsCalledShouldReturnValues()
        {
            Assert.Equal(new Dictionary<int, int> { { 2, 1 }, { 3, 2 }, { 4, 3 } }, new List<int>() { 1, 2, 3 }
            .ToDictionary(x => x + 1));
        }

        [Fact]
        public void ZipIsCalledShouldReturnNewEnumerable()
        {
            Assert.Equal(new[] { "1a", "2b", "3c" }, new List<int>() { 1, 2, 3 }
            .Zip(new List<string>() { "a", "b", "c", "d" }, (leters, numbers) => leters + numbers));
        }
    }
}