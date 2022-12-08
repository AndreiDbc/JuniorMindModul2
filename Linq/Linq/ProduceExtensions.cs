using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Linq
{
    static class ProduceExtensions
    {
        public static IEnumerable<Produce> JoinLists(this IEnumerable<Produce> first, IEnumerable<Produce> second)
        {
            return first.Select(x => new Produce
            {
                Name = x.Name,
                Quantity = second.Contains(x)
            ? second.First(y => y.Name == x.Name).Quantity + x.Quantity
            : x.Quantity
            }).Union(second.Where(z => !first.Contains(z)), EqualityComparer<Produce>.Default);
        }
    }
}
