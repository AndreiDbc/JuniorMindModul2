using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Xunit;

namespace Linq
{
    public class ProduceExtensionsFacts
    {
        [Fact]
        public void JoinListIsCalledShouldCombineLists()
        {
            var p1 = new Produce { Name = "Mars", Quantity = 1 };
            var p2 = new Produce { Name = "Twix", Quantity = 2 };
            var p3 = new Produce { Name = "Bounty", Quantity = 4 };
            var p4 = new Produce { Name = "Twix", Quantity = 4 };
            var l1 = new List<Produce>() { p1, p2 };
            var l2 = new List<Produce>() { p2, p3 };
            var result = l1.JoinLists(l2);
            Assert.Equal(p1.Name, result.ElementAt(0).Name);
            Assert.Equal(p1.Quantity, result.ElementAt(0).Quantity);
            Assert.Equal(p4.Quantity, result.ElementAt(1).Quantity);
            Assert.Equal(p4.Name, result.ElementAt(1).Name);
            Assert.Equal(p3.Quantity, result.ElementAt(2).Quantity);
            Assert.Equal(p3.Name, result.ElementAt(2).Name);
        }
    }
}
