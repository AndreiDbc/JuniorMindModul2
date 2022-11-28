using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Linq
{
    public class ProduceFacts
    {
        [Fact]
        public void NewProduceIsCreatedShouldReturnNameAndQuantity()
        {
            var produce = new Produce();
            produce.Name = "Twix";
            produce.Quantity = 10;
            Assert.Equal("Twix", produce.Name);
            Assert.Equal(10, produce.Quantity);
        }
    }
}
