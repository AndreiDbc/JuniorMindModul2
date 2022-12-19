using Microsoft.VisualBasic;
using System;
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
            return merchandises.Where(merchandise => features.Any(merchandise.Features.Contains) && !features.All(merchandise.Features.Contains));
        }
    }
}
