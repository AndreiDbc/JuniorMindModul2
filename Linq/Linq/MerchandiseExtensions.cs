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
            var dictionary = AddToDictionary(merchandises, features);
            return features.Aggregate(new List<Merchandise>(), (x, y) =>
            {
                if (dictionary.ContainsKey(y))
                {
                    x.AddRange(dictionary[y].Where(a => features.All(b => a.Features.Contains(b))));
                }

                return x;
            }).Distinct();
        }

        public static IEnumerable<Merchandise> GetNoFeaturesMerchandises(
            this IEnumerable<Merchandise> merchandises, ICollection<Feature> features)
        {
            var dictionary = AddToDictionary(merchandises, features);
            return features.Aggregate(new List<Merchandise>(), (x, y) =>
            {
                if (dictionary.ContainsKey(y))
                {
                    x.AddRange(dictionary[y].Where(a => features.All(b => !a.Features.Contains(b))));
                }

                return x;
            }).Distinct();
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
