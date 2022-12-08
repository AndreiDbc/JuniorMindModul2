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

        [Fact]
        public void GenerateSumIsCalledShouldReturnSequenceThatResultsToK()
        {
            Assert.Equal(new[] { "+1-2+3=2" }, 3.GenerateSum(2));
        }

        [Fact]
        public void GenerateTripletsIsCalledShouldReturnSequenceOfNumbers()
        {
            Assert.Equal(new[] { new[] { 3, 4, 5 }, new[] { 4, 3, 5 } }, new[] { 1, 2, 3, 4, 5}.GenerateTriplets());
        }
    }
}
