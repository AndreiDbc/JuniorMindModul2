using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Linq
{
    public class CustomComparerFacts
    {
        [Fact]
        public void CompareIsCalledShouldReturnMinusOne()
        {
            Assert.Equal(-1, new CustomComparer<string>(Comparer<string>.Default.Compare).Compare("as", "bdf"));
        }

        [Fact]
        public void CompareIsCalledShouldReturnZero()
        {
            Assert.Equal(0, new CustomComparer<string>(Comparer<string>.Default.Compare).Compare("as", "as"));
        }

        [Fact]
        public void CompareIsCalledShouldReturnOne()
        {
            Assert.Equal(1, new CustomComparer<string>(Comparer<string>.Default.Compare).Compare("zs", "bdf"));
        }
    }
}
