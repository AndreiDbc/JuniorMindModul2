using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Linq
{
    public class ReverseComparerFacts
    {
        [Fact]
        public void CompareIsCalledShouldReturnReversedResult()
        {
            Assert.Equal(-1, new ReverseComparer<int>(Comparer<int>.Default).Compare(3, 2));
        }
    }
}
