using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
            var p4 = new Produce { Name = "Twix", Quantity = 3 };
            var p5 = new Produce { Name = "Mars", Quantity = 1 };
            var l1 = new List<Produce>() { p1, p2 };
            var l2 = new List<Produce>() { p4, p3, p5 };
            Assert.Equal(new[] { "Mars: 2", "Twix: 5", "Bounty: 4" }, l1.JoinLists(l2));
        }
    }
}
