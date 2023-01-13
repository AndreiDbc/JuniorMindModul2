using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Linq
{
    static class ProduceExtensions
    {
        public static IEnumerable<Produce> JoinLists(this IEnumerable<Produce> first, IEnumerable<Produce> second)
        {
            return first.Concat(second).GroupBy(
                a => a.Name,
                b => b.Quantity,
                (key, values) => new Produce() { Name = key, Quantity = values.Sum() },
                EqualityComparer<string>.Default);
        }
    }
}
