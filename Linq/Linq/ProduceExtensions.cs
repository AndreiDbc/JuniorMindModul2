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
            var dictionary = first.ToDictionary(x => x.Name);

            return second.Aggregate(dictionary, (x, y) =>
            {
                if (x.ContainsKey(y.Name))
                {
                    x[y.Name].Quantity += y.Quantity;
                }
                else
                {
                    x.Add(y.Name, y);
                }

                return x;
            }).Values;
        }
    }
}
