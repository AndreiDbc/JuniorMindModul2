using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linq
{
    static class MerchandiseExtensions
    {
        public static IEnumerable<Merchandise> GetAllFeaturesMerchandises(
            this IEnumerable<Merchandise> merchandises, ICollection<Feature> features)
        {
            var hashset = features.ToHashSet();
            return merchandises.Where(x => hashset.IsSubsetOf(x.Features));
        }

        public static IEnumerable<Merchandise> GetNoFeaturesMerchandises(
            this IEnumerable<Merchandise> merchandises, ICollection<Feature> features)
        {
            var hashset = features.ToHashSet();
            return merchandises.Where(x => !x.Features.ToHashSet().IsSubsetOf(hashset) || x.Features.Count == 0);
        }

        public static IEnumerable<Merchandise> GetMerchandisesContainingFeatures(
            this IEnumerable<Merchandise> merchandises, ICollection<Feature> features)
        {
            var dictionary = new Dictionary<Feature, List<Merchandise>>();

            return merchandises.Aggregate(dictionary, (x, y) =>
            {
                features.Aggregate(x, (a, b) =>
                {
                    if (!features.All(y.Features.Contains) && y.Features.Contains(b))
                    {
                        if (a.ContainsKey(b))
                        {
                            a[b].Add(y);
                        }
                        else
                        {
                            a.Add(b, new List<Merchandise> { y });
                        }
                    }

                    return x;
                });

                return dictionary;
            }).Values.SelectMany(a => a).Distinct();
        }
    }
}
