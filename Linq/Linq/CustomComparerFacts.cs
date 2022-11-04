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
        public void CompareIsCalledShouldReturnPrimaryResult()
        {
            Assert.Equal(-1, new CustomComparer<string>(Comparer<string>.Default, new ComparerConverter<string, bool>(x => x.EndsWith("b"), Comparer<bool>.Default))
                .Compare("asd", "gb"));
        }

        [Fact]
        public void CompareIsCalledShouldReturnSecondaryResult()
        {
            Assert.Equal(1, new CustomComparer<string>(Comparer<string>.Default, new ComparerConverter<string, bool>(x => x.EndsWith("b"), Comparer<bool>.Default))
                .Compare("zba", "zb"));
        }
    }
}
