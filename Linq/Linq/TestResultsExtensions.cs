using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Linq
{
    static class TestResultsExtensions
    {
        public static IEnumerable<TestResults> EliminateDuplicates(this IEnumerable<TestResults> testResults)
        {
            return testResults.GroupBy(
                a => a.FamilyId,
                b => b,
                (key, value) => new TestResults()
                {
                    FamilyId = key,
                    Id = value.ElementAt(0).Id,
                    Score = value.Max(a => a.Score)
                },
                EqualityComparer<string>.Default);
        }
    }
}
