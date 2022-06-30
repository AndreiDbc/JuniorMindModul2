using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace JsonSecondVersion
{
    public class MathematicalExpresionFacts
    {
        [Fact]
        public void MathematicalExpresionIsGivenExpresionWithoutBracketsShouldReturnRemainingText()
        {
            MathematicalExpression expression = new MathematicalExpression();
            Assert.Equal("+3", expression.Match("1+2+3").RemainingText());
        }

        [Fact]
        public void MathematicalExpresionIsGivenExpresionWithBracketsShouldReturnEmptyText()
        {
            MathematicalExpression expression = new MathematicalExpression();
            Assert.Equal("", expression.Match("(2+5)*4").RemainingText());
        }
    }
}
