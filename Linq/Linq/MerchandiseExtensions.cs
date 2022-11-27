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
            return merchandises.Where(merchandise => features.All(feature => merchandise.Features.Contains(feature)));
        }
    }
}
