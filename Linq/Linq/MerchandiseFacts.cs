using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Linq
{
    public class MerchandiseFacts
    {
        [Fact]
        public void ANewInstanceOfMerchandiseIsCreatedShouldReturnMerchandise()
        {
            var f1 = new Feature();
            f1.Id = 1;
            var f2 = new Feature();
            f2.Id = 2;
            var f3 = new Feature();
            f3.Id = 3;
            var merchandise = new Merchandise();
            merchandise.Name = "a";
            merchandise.Features = new List<Feature>() { f1, f2, f3 };
            Assert.Equal("a", merchandise.Name);
            Assert.Equal(new[] { f1, f2, f3 }, merchandise.Features);
        }
    }
}
