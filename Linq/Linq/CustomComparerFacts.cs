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
        public void CompareIsCalledCompareKeysIsUsedShouldReturnMinusOne()
        {
            Assert.Equal(-1, new CustomComparer<string, int>(x => x.Length, Comparer<int>.Default)
                .Compare("as", "bdc"));
        }

        [Fact]
        public void CompareIsCalledCompareKeysIsUsedShouldReturnZero()
        {
            Assert.Equal(0, new CustomComparer<string, int>(x => x.Length, Comparer<int>.Default)
                .Compare("as", "as"));
        }

        [Fact]
        public void CompareIsCalledCompareKeysUsedShouldReturnOne()
        {
            Assert.Equal(1, new CustomComparer<string, int>(x => x.Length, Comparer<int>.Default)
                .Compare("casd", "bd"));
        }

        [Fact]
        public void CompareIsCalledShouldReturnReversedResult()
        {
            Assert.Equal(-1, new CustomComparer<int, int>(Comparer<int>.Default).Compare(3, 2));
        }

        [Fact]
        public void CompareIsCalledComparePrimaryAndSecondaryIsUsedShouldReturnPrimaryResult()
        {
            Assert.Equal(-1, new CustomComparer<string, bool>(Comparer<string>.Default, new CustomComparer<string, bool>(x => x.EndsWith("b"), Comparer<bool>.Default))
                .Compare("asd", "gb"));
        }

        [Fact]
        public void CompareIsCalledComparePrimaryAndSecondaryIsUsedShouldReturnSecondaryResult()
        {
            Assert.Equal(1, new CustomComparer<string, bool>(Comparer<string>.Default, new CustomComparer<string, bool>(x => x.EndsWith("b"), Comparer<bool>.Default))
                .Compare("zba", "zb"));
        }
    }
}
