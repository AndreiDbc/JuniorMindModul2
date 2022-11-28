using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Linq
{
    public class ProduceExtensionsFacts
    {
        [Fact]
        public void JoinListIsCalledShouldCombineLists()
        {
            var p1 = new Produce();
            p1.Name = "Mars";
            p1.Quantity = 1;
            var p2 = new Produce();
            p2.Name = "Twix";
            p2.Quantity = 2;
            var p3 = new Produce();
            p3.Name = "Bounty";
            p3.Quantity = 4;
            var p4 = new Produce();
            p4.Name = "Twix";
            p4.Quantity = 4;
            var l1 = new List<Produce>() { p1, p2 };
            var l2 = new List<Produce>() { p2, p3 };
            Assert.Equal(new List<Produce>() { p1, p4, p3 }, l1.JoinLists(l2));
        }
    }
}
