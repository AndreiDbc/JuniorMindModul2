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
            var featuresToHashSet = features.ToHashSet();
            return merchandises.Where(x => featuresToHashSet.IsSubsetOf(x.Features));
        }

        public static IEnumerable<Merchandise> GetNoFeaturesMerchandises(
            this IEnumerable<Merchandise> merchandises, ICollection<Feature> features)
        {
            var featuresToHashSet = features.ToHashSet();
            return merchandises.Where(x => !featuresToHashSet.IsSupersetOf(x.Features) || x.Features.Count == 0);
        }

        public static IEnumerable<Merchandise> GetMerchandisesContainingFeatures(
            this IEnumerable<Merchandise> merchandises, ICollection<Feature> features)
        {
            var featuresToHashSet = features.ToHashSet();
            return merchandises.Where(x => featuresToHashSet.Intersect(x.Features).Any()
                     && !featuresToHashSet.IsSubsetOf(x.Features));
        }
    }
}
