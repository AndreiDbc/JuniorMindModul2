using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Dictionary
{
    public class ElementFacts
    {
        [Fact]
        public void ANodeIsCreatedShouldReturnNodeWithValues()
        {
            Element<int> node = new Element<int>(1, 1);
            Assert.Equal(1, node.Value);
            Assert.Equal(-1, node.Next);
        }
    }
}
