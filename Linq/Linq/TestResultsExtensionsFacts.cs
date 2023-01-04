using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Linq
{
    public class TestResultsExtensionsFacts
    {
        [Fact]
        public void AnEnumerableOfTestResultsIsCreatedShouldEliminateDuplicates()
        {
            var t1 = new TestResults() { FamilyId = "a", Id = "as", Score = 1 };
            var t2 = new TestResults() { FamilyId = "b", Id = "bs", Score = 2 };
            var t3 = new TestResults() { FamilyId = "a", Id = "as", Score = 3 };
            var results = new[] { t1, t2, t3 }.EliminateDuplicates();
            Assert.Equal("a", results.ElementAt(0).FamilyId);
            Assert.Equal(3, results.ElementAt(0).Score);
            Assert.Equal("b", results.ElementAt(1).FamilyId);
            Assert.Equal(2, results.ElementAt(1).Score);
        }
    }
}
