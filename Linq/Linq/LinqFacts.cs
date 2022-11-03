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
        public void AllIsCalledPredicateIsNullShouldThorwException()
        {
            Assert.Throws<ArgumentNullException>(() => new List<int>() { 1, 2, 3 }.All(null));
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
        public void AnyIsCalledPredicateIsNullShouldThorwException()
        {
            Assert.Throws<ArgumentNullException>(() => new List<int>() { 1, 2, 3 }.All(null));
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
        public void FirstIsCalledPredicateIsNullShouldThorwException()
        {
            Assert.Throws<ArgumentNullException>(() => new List<int>() { 1, 2, 3 }
            .First(null));
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
        public void WhereIsCalledPredicateIsNullShouldThrowException()
        {
            Assert.Throws<ArgumentNullException>(() => new[] { 1, 2, 3, 4 }.Where(null));
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

        [Fact]
        public void AggregateIsCalledShouldReturnMultiplication()
        {
            Assert.Equal(3000, new List<int> { 10, 20, 15 }.Aggregate((x, y) => x * y));
        }

        [Fact]
        public void AggregateIsCalledFuncIsNullShouldThrowException()
        {
            Assert.Throws<ArgumentNullException>(() => new List<int> { 10, 20, 15 }.Aggregate(null));
        }

        [Fact]
        public void JoinIsCalledShouldReturnResult()
        {
            Assert.Equal(new[] { "a d", "b e", "c f" }, new Dictionary<int, string>() { { 0, "a" }, { 1, "b" }, { 2, "c" } }.Join(
                new Dictionary<int, string>() { { 2, "d" }, { 4, "e" }, { 6, "f" } },
                outerKeySelector => outerKeySelector.Key + 1,
                innerKeySelector => innerKeySelector.Key / 2,
                (outer, inner) => outer.Value + " " + inner.Value));
        }

        [Fact]
        public void JoinIsCalledOuterKeySelectorIsNullShouldThrowException()
        {
            Assert.Throws<ArgumentNullException>(() => new Dictionary<int, string>() { { 0, "a" }, { 1, "b" }, { 2, "c" } }.Join(
                new Dictionary<int, string>() { { 2, "d" }, { 4, "e" }, { 6, "f" } },
                null,
                innerKeySelector => innerKeySelector.Key / 2,
                (outer, inner) => outer.Value + " " + inner.Value));
        }

        [Fact]
        public void JoinIsCalledInnerKeySelectorIsNullShouldThrowException()
        {
            Assert.Throws<ArgumentNullException>(() => new Dictionary<int, string>() { { 0, "a" }, { 1, "b" }, { 2, "c" } }.Join(
                new Dictionary<int, string>() { { 2, "d" }, { 4, "e" }, { 6, "f" } },
                outerKeySelector => outerKeySelector.Key + 1,
                null,
                (outer, inner) => outer.Value + " " + inner.Value));
        }

        [Fact]
        public void DistinctIsCalledShouldReturnDistinctElements()
        {
            Assert.Equal(new[] { 1, 2, 3 }, new List<int>() { 1, 1, 2, 3, 3 }.Distinct(EqualityComparer<int>.Default));
        }

        [Fact]
        public void DistinctIsCalledIEqualityComparerIsNullShouldTHorwException()
        {
            Assert.Throws<ArgumentNullException>(() => new List<int>() { 1, 1, 2, 3, 3 }.Distinct(null));
        }

        [Fact]
        public void UnionIsCalledShouldReturnElements()
        {
            Assert.Equal(new[] { 1, 2, 3 }, new List<int>() { 1, 2 }
            .Union(new List<int>() { 2, 3 }, EqualityComparer<int>.Default));
        }

        [Fact]
        public void UnionIsCalledSecondIsNullShouldThrowException()
        {
            Assert.Throws<ArgumentNullException>(() => new List<int>() { 1, 2 }
            .Union(null, EqualityComparer<int>.Default));
        }

        [Fact]
        public void UnionIsCalledIequalityComparerIsNullShouldThrowException()
        {
            Assert.Throws<ArgumentNullException>(() => new List<int>() { 1, 2 }
            .Union(new List<int>() { 2, 3 }, null));
        }

        [Fact]
        public void IntersectIsCalledShouldReturnElements()
        {
            Assert.Equal(new[] { 1, 2 }, new List<int>() { 1, 2, 3, 4 }
            .Intersect(new List<int>() { 1, 2, 8, 9 }, EqualityComparer<int>.Default));
        }

        [Fact]
        public void IntersectIsCalledSecondIsNullSHouldThrowException()
        {
            Assert.Throws<ArgumentNullException>(() => new List<int>() { 1, 2, 3, 4 }
            .Intersect(null, EqualityComparer<int>.Default));
        }

        [Fact]
        public void IntersectIsCalledEqualityComparerIsNullShouldThrowException()
        {
            Assert.Throws<ArgumentNullException>(() => new List<int>() { 1, 2, 3, 4 }
            .Intersect(new List<int>() { 1, 2, 8, 9 }, null));
        }

        [Fact]
        public void ExceptIsCalledShouldReturnElements()
        {
            Assert.Equal(new[] { 1, 5, 7 }, new List<int>() { 1, 2, 3, 4, 5, 6, 7 }
            .Except(new List<int>() { 2, 3, 4, 6 }, EqualityComparer<int>.Default));
        }

        [Fact]
        public void ExceptIsCalledSecondIsNullShouldThrowException()
        {
            Assert.Throws<ArgumentNullException>(() => new List<int>() { 1, 2, 3, 4, 5, 6, 7 }
            .Except(null, EqualityComparer<int>.Default));
        }

        [Fact]
        public void ExceptIsCalledEqualityComparerIsNullShouldThrowException()
        {
            Assert.Throws<ArgumentNullException>(() => new List<int>() { 1, 2, 3, 4, 5, 6, 7 }
            .Except(new List<int>() { 2, 3, 4, 6 }, null));
        }

        [Fact]
        public void GroupByIsCalledShouldSortElements()
        {
            Assert.Equal(
                new List<string> { "3:ads-cvb", "2:ba" }, new[] { "ads", "ba", "cvb" }.GroupBy(
                x => x.Length,
                y => y,
                (key, values) => key + ":" + string.Join("-", values),
                EqualityComparer<int>.Default));
        }

        [Fact]
        public void GroupByIsCalledKeySelectorIsNullShouldThrowException()
        {
            Assert.Throws<ArgumentNullException>(() => new[] { "ads", "ba", "cvb" }.GroupBy(
                null,
                y => y,
                (key, values) => key + ":" + string.Join("-", values),
                EqualityComparer<int>.Default));
        }

        [Fact]
        public void GroupByIsCalledResultIEqualityComparerNullShouldThrowException()
        {
            Assert.Throws<ArgumentNullException>(() => new[] { "ads", "ba", "cvb" }.GroupBy(
                x => x.Length,
                y => y,
                (key, values) => key + ":" + string.Join("-", values),
                null));
        }

        [Fact]
        public void OrderByIsCalledShouldSortElements()
        {
            Assert.Equal(new[] { "z", "as", "bc", "zh", "bbc" }, new[] { "bbc", "as", "bc", "z", "zh" }
            .OrderBy(x => x.Length).Select(x => x));
        }
    }
}