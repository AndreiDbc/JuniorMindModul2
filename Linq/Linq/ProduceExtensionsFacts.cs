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
            Assert.Equal(new List<Produce> { p1, p2, p3 }, l1.JoinLists(l2));
        }
    }
}
