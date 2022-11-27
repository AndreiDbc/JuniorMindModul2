using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Linq
{
    public class MerchandiseExtensionsFacts
    {
        [Fact]
        public void GetAllFeaturesMerchendisesIsCalledShouldReturnMerchandise()
        {
            var f1 = new Feature();
            f1.Id = 1;
            var f2 = new Feature();
            f2.Id = 2;
            var f3 = new Feature();
            f3.Id = 3;
            var m1 = new Merchandise();
            m1.Name = "a";
            m1.Features = new List<Feature>();
            var m2 = new Merchandise();
            m2.Name = "b";
            m2.Features = new List<Feature>() { f2, f3 };
            var m3 = new Merchandise();
            m3.Name = "c";
            m3.Features = new List<Feature>() { f1, f2, f3 };
            Assert.Equal(new List<Merchandise>() { m3 }, new List<Merchandise>() { m1, m2, m3 }.GetAllFeaturesMerchandises(new List<Feature>() { f1, f2, f3 }));
        }
    }
}
