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
        public static IEnumerable<Merchandise> GetAllFeaturesMerchandises(this IEnumerable<Merchandise> merchandises, ICollection<Feature> features)
        {
            return merchandises.Where(merchandise => features.All(merchandise.Features.Contains));
        }

        public static IEnumerable<Merchandise> GetNoFeaturesMerchandises(this IEnumerable<Merchandise> merchandises, ICollection<Feature> features)
        {
            return merchandises.Where(merchandise => !features.Any(merchandise.Features.Contains));
        }

        public static IEnumerable<Merchandise> GetMerchandisesContainingFeatures(this IEnumerable<Merchandise> merchandises, ICollection<Feature> features)
        {
            var dictionary = AddToDictionary(merchandises, features);

            return features.Aggregate(new List<Merchandise>(), (x, y) =>
            {
                x.AddRange(dictionary[y].Where(a => features.Any(b => !a.Features.Contains(b) && a.Features.Count != 0)));
                return x;
            }).Distinct();
        }

        private static Dictionary<Feature, List<Merchandise>> AddToDictionary(
            this IEnumerable<Merchandise> merchandises, ICollection<Feature> features)
        {
            var dictionary = new Dictionary<Feature, List<Merchandise>>();
            return merchandises.Aggregate(dictionary, (x, y) =>
            {
                features.Aggregate(x, (a, b) =>
                {
                    if (y.Features.Contains(b))
                    {
                        a[b].Add(y);
                    }
                    else
                    {
                        a[b] = new List<Merchandise>() { y };
                    }

                    return x;
                });

                return dictionary;
            });
        }
    }
}
