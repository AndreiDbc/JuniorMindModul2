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
        public void MathematicalExpresionIsGivenExpresionWithoutBracketsShouldReturnEmptyText()
        {
            MathematicalExpression expression = new MathematicalExpression();
            Assert.Equal("", expression.Match("2+5*4-7/23").RemainingText());
        }

        [Fact]
        public void MathematicalExpresionIsGivenExpresionWithBracketsShouldReturnEmptyText()
        {
            MathematicalExpression expression = new MathematicalExpression();
            Assert.Equal("", expression.Match("(2+5)*4-(7/23)").RemainingText());
        }
    }
}
