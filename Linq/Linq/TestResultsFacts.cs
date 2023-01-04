using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Linq
{
    public class TestResultsFacts
    {
        [Fact]
        public void AnInstanceOfTestResultsIsCreatedShouldReturnObject()
        {
            var test = new TestResults();
            test.FamilyId = "a";
            test.Id = "b";
            test.Score = 2;
            Assert.Equal("a", test.FamilyId);
            Assert.Equal("b", test.Id);
            Assert.Equal(2, test.Score);
        }
    }
}
