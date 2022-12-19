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
        public void GetAllFeaturesMerchandisesIsCalledShouldReturnMerchandise()
        {
            var f1 = new Feature { Id = 1 };
            var f2 = new Feature { Id = 2 };
            var f3 = new Feature { Id = 3 };
            var m1 = new Merchandise { Name = "a", Features = new List<Feature>() };
            var m2 = new Merchandise { Name = "b", Features = new List<Feature>() { f2, f3 } };
            var m3 = new Merchandise { Name = "c", Features = new List<Feature>() { f1, f2, f3 } };
            Assert.Equal(new List<Merchandise>() { m3 }, new List<Merchandise>() { m1, m2, m3 }.GetAllFeaturesMerchandises(new List<Feature>() { f1, f2, f3 }));
        }

        [Fact]
        public void GetNoFeaturesMerchandisesIsCalledShouldReturnMerchandise()
        {
            var f1 = new Feature { Id = 1 };
            var f2 = new Feature { Id = 2 };
            var f3 = new Feature { Id = 3 };
            var m1 = new Merchandise { Name = "a", Features = new List<Feature>() };
            var m2 = new Merchandise { Name = "b", Features = new List<Feature>() { f2, f3 } };
            var m3 = new Merchandise { Name = "c", Features = new List<Feature>() { f1, f2, f3 } };
            Assert.Equal(new List<Merchandise>() { m1 }, new List<Merchandise>() { m1, m2, m3 }.GetNoFeaturesMerchandises(new List<Feature>() { f1, f2, f3 }));
        }

        [Fact]
        public void GetMerchandisesContainingFeaturesIsCalledShouldReturnMerchandise()
        {
            var f1 = new Feature { Id = 1 };
            var f2 = new Feature { Id = 2 };
            var f3 = new Feature { Id = 3 };
            var m1 = new Merchandise { Name = "a", Features = new List<Feature>() };
            var m2 = new Merchandise { Name = "b", Features = new List<Feature>() { f2, f3 } };
            var m3 = new Merchandise { Name = "c", Features = new List<Feature>() { f1, f2, f3 } };
            Assert.Equal(new List<Merchandise>() { m2 }, new List<Merchandise>() { m1, m2, m3 }.GetMerchandisesContainingFeatures(new List<Feature>() { f1, f2, f3 }));
        }
    }
}
