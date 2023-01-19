using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Linq
{
    public class ReversePolishNotationFacts
    {
        [Fact]
        public void AnInstanceOfReversePolishNotationisCReatedCalculateIsCalledShouldReturnResult()
        {
            Assert.Equal(22, new ReversePolishNotation("10 6 9 3 + -11 * / * 17 + 5 +").GetResult());
        }
    }
}