using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Linq
{
    static class Linq
    {
        public static bool All<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            CheckForNull(source);
            CheckForNull(predicate);
            foreach (TSource item in source)
            {
                if (!predicate(item))
                {
                    return false;
                }
            }

            return true;
        }

        public static bool Any<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            CheckForNull(source);
            CheckForNull(predicate);
            foreach (var item in source)
            {
                if (predicate(item))
                {
                    return true;
                }
            }

            return false;
        }

        public static TSource First<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            CheckForNull(source);
            CheckForNull(predicate);
            foreach (var item in source)
            {
                if (predicate(item))
                {
                    return item;
                }
            }

            throw new InvalidOperationException();
        }

        public static IEnumerable<TResult> Select<TSource, TResult>(
            this IEnumerable<TSource> source, Func<TSource, TResult> selector)
        {
            CheckForNull(source);
            CheckForNull(selector);
            foreach (var item in source)
            {
                yield return selector(item);
            }
        }

        public static IEnumerable<TResult> SelectMany<TSource, TResult>(
            this IEnumerable<TSource> source, Func<TSource, IEnumerable<TResult>> selector)
        {
            CheckForNull(source);
            CheckForNull(selector);
            foreach (var item in source)
            {
                foreach (var selected in selector(item))
                {
                    yield return selected;
                }
            }
        }

        public static IEnumerable<TSource> Where<TSource>(
            this IEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            CheckForNull(source);
            CheckForNull(predicate);

            var result = new List<TSource>();
            foreach (var item in source)
            {
                if (predicate(item))
                {
                    result.Add(item);
                }
            }

            return result;
        }

        public static Dictionary<TKey, TElement> ToDictionary<TSource, TKey, TElement>(
            this IEnumerable<TSource> source,
            Func<TSource, TKey> keySelector,
            Func<TSource, TElement> elementSelector)
        {
            CheckForNull(source);
            CheckForNull(keySelector);
            CheckForNull(elementSelector);
            var dictionary = new Dictionary<TKey, TElement>();
            foreach (var item in source)
            {
                dictionary.Add(keySelector(item), elementSelector(item));
            }

            return dictionary;
        }

        public static IEnumerable<TResult> Zip<TFirst, TSecond, TResult>(
            this IEnumerable<TFirst> first,
            IEnumerable<TSecond> second,
            Func<TFirst, TSecond, TResult> resultSelector)
        {
            CheckForNull(first);
            CheckForNull(second);
            var enumerator = second.GetEnumerator();
            foreach (var item in first)
            {
                if (enumerator.MoveNext())
                {
                    yield return resultSelector(item, enumerator.Current);
                }
            }
        }

        public static TAccumulate Aggregate<TSource, TAccumulate>(this IEnumerable<TSource> source, TAccumulate seed, Func<TAccumulate, TSource, TAccumulate> func)
        {
            CheckForNull(source);
            CheckForNull(func);
            if (seed == null)
            {
                throw new ArgumentNullException(nameof(seed));
            }

            TAccumulate result = default;
            foreach (var item in source)
            {
                result = func(seed, item);
            }

            return result;
        }

        public static IEnumerable<TResult> Join<TOuter, TInner, TKey, TResult>(
        this IEnumerable<TOuter> outer,
        IEnumerable<TInner> inner,
        Func<TOuter, TKey> outerKeySelector,
        Func<TInner, TKey> innerKeySelector,
        Func<TOuter, TInner, TResult> resultSelector)
        {
            CheckForNull(outer);
            CheckForNull(inner);
            CheckForNull(outerKeySelector);
            CheckForNull(innerKeySelector);
            CheckForNull(resultSelector);

            var result = new List<TResult>();
            foreach (var outerItem in outer)
            {
                foreach (var innerItem in inner)
                {
                    if (outerKeySelector(outerItem).Equals(innerKeySelector(innerItem)))
                    {
                        result.Add(resultSelector(outerItem, innerItem));
                    }
                }
            }

            return result;
        }

        public static IEnumerable<TSource> Distinct<TSource>(this IEnumerable<TSource> source, IEqualityComparer<TSource> comparer)
        {
            CheckForNull(source);
            CheckForNull(comparer);
            var result = new List<TSource>();
            foreach (var item in source)
            {
                if (!result.Contains(item))
                {
                    result.Add(source.First(val => comparer.Equals(val, item)));
                }
            }

            return result;
        }

        public static IEnumerable<TSource> Union<TSource>(
        this IEnumerable<TSource> first,
        IEnumerable<TSource> second,
        IEqualityComparer<TSource> comparer)
        {
            CheckForNull(first);
            CheckForNull(second);
            CheckForNull(comparer);
            var result = new List<TSource>();
            result.AddRange(first);
            foreach (var firstItem in first)
            {
                foreach (var secondItem in second)
                {
                    if (!comparer.Equals(firstItem, secondItem) && !result.Contains(secondItem))
                    {
                        result.Add(secondItem);
                    }
                }
            }

            return result;
        }

        public static IEnumerable<TSource> Intersect<TSource>(
        this IEnumerable<TSource> first,
        IEnumerable<TSource> second,
        IEqualityComparer<TSource> comparer)
        {
            CheckForNull(first);
            CheckForNull(second);
            CheckForNull(comparer);
            var result = new List<TSource>();
            foreach (var firstItem in first)
            {
                foreach (var secondItem in second)
                {
                    if (comparer.Equals(firstItem, secondItem))
                    {
                        result.Add(firstItem);
                    }
                }
            }

            return result;
        }

        public static IEnumerable<TSource> Except<TSource>(
        this IEnumerable<TSource> first,
        IEnumerable<TSource> second,
        IEqualityComparer<TSource> comparer)
        {
            CheckForNull(first);
            CheckForNull(second);
            CheckForNull(comparer);

            var result = new List<TSource>();

            foreach (var item in first)
            {
                if (!first.Intersect(second, comparer).Contains(item))
                {
                    result.Add(item);
                }
            }

            return result;
        }

        public static IEnumerable<TResult> GroupBy<TSource, TKey, TElement, TResult>(
        this IEnumerable<TSource> source,
        Func<TSource, TKey> keySelector,
        Func<TSource, TElement> elementSelector,
        Func<TKey, IEnumerable<TElement>, TResult> resultSelector,
        IEqualityComparer<TKey> comparer)
        {
            CheckForNull(source);
            CheckForNull(keySelector);
            CheckForNull(elementSelector);
            CheckForNull(resultSelector);
            CheckForNull(comparer);

            var temp = new Dictionary<TKey, List<TElement>>(comparer);
            foreach (var element in source)
            {
                var key = keySelector(element);
                if (temp.ContainsKey(key))
                {
                    temp[key].Add(elementSelector(element));
                }
                else
                {
                    temp.Add(key, new List<TElement>() { elementSelector(element) });
                }
            }

            var result = new List<TResult>();

            foreach (var item in temp)
            {
                result.Add(resultSelector(item.Key, item.Value));
            }

            return result;
        }

        public static IOrderedEnumerable<TSource> OrderBy<TSource, TKey>(
        this IEnumerable<TSource> source,
        Func<TSource, TKey> keySelector,
        IComparer<TKey> comparer)
        {
            CheckForNull(source);
            CheckForNull(keySelector);
            CheckForNull(comparer);
            return new OrderedEnumerable<TKey, TSource>(source, comparer, keySelector);
        }

        public static IOrderedEnumerable<TSource> ThenBy<TSource, TKey>(
        this IOrderedEnumerable<TSource> source,
        Func<TSource, TKey> keySelector,
        IComparer<TKey> comparer)
        {
            CheckForNull(source);
            CheckForNull(keySelector);
            CheckForNull(comparer);
            return source.CreateOrderedEnumerable(keySelector, comparer, false);
        }

        private static void CheckForNull<T>(IEnumerable<T> source)
        {
            if (source != null)
            {
                return;
            }

            throw new ArgumentNullException(nameof(source));
        }

        private static void CheckForNull<TSource, TElement>(Func<TSource, TElement> predicate)
        {
            if (predicate != null)
            {
                return;
            }

            throw new ArgumentNullException(nameof(predicate));
        }

        private static void CheckForNull<TSource, TFirst, TSecond>(Func<TSource, TFirst, TSecond> predicate)
        {
            if (predicate != null)
            {
                return;
            }

            throw new ArgumentNullException(nameof(predicate));
        }

        private static void CheckForNull<TSource>(IEqualityComparer<TSource> predicate)
        {
            if (predicate != null)
            {
                return;
            }

            throw new ArgumentNullException(nameof(predicate));
        }

        private static void CheckForNull<TSource>(IComparer<TSource> predicate)
        {
            if (predicate != null)
            {
                return;
            }

            throw new ArgumentNullException(nameof(predicate));
        }
    }
}
