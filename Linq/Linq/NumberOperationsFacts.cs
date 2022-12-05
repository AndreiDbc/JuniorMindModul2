using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Linq
{
    public class NumberOperationsFacts
    {
        [Fact]
        public void GenerateSubarraysIsCalledShouldReturnSequenceOfNumbers()
        {
            Assert.Equal(new[] { new[] { 1 }, new[] { 2 }, new[] { 3 }, new[] { 1, 2 } }, new[] { 1, 2, 3, 4 }.GenerateSubarrays(3));
        }
    }
}
