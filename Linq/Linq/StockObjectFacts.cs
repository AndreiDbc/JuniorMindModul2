using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Linq
{
    public class StockObjectFacts
    {
        [Fact]
        public void StockObjectIsCreatedShouldReturnContainingValues()
        {
            var obj = new StockObject("bike", 3);
            Assert.Equal("bike", obj.Name);
            Assert.Equal(3, obj.Quantity);
        }

        [Fact]
        public void StockObjectIsCreatedQuantitySetterIsCalledSHouldUpdateValue()
        {
            var obj = new StockObject("bike", 3);
            obj.Quantity += 3;
            Assert.Equal(6, obj.Quantity);
        }
    }
}
