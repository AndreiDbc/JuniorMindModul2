using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Linq
{
    public class ComparerConverterFacts
    {
        [Fact]
        public void CompareIsCalledShouldReturnMinusOne()
        {
            Assert.Equal(-1, new ComparerConverter<string, int>(x => x.Length, Comparer<int>.Default)
                .Compare("as", "bdc"));
        }

        [Fact]
        public void CompareIsCalledShouldReturnZero()
        {
            Assert.Equal(0, new ComparerConverter<string, int>(x => x.Length, Comparer<int>.Default)
                .Compare("as", "as"));
        }

        [Fact]
        public void CompareIsCalledShouldReturnOne()
        {
            Assert.Equal(1, new ComparerConverter<string, int>(x => x.Length, Comparer<int>.Default)
                .Compare("casd", "bd"));
        }
    }
}
