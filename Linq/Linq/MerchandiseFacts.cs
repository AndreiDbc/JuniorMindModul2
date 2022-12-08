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
            var f1 = new Feature { Id = 1 };
            var f2 = new Feature { Id = 2 };
            var f3 = new Feature { Id = 3 };
            var merchandise = new Merchandise { Name = "a", Features = new List<Feature>() { f1, f2, f3 } };
            Assert.Equal("a", merchandise.Name);
            Assert.Equal(new[] { f1, f2, f3 }, merchandise.Features);
        }
    }
}
