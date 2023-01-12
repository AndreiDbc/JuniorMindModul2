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
            var hashset = features.ToHashSet();
            return merchandises.Where(x => x.Features.Count != 0 && hashset.IsProperSupersetOf(x.Features));
        }
    }
}
