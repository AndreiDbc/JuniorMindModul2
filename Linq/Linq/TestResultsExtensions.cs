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
                (_, value) => value.MaxBy(a => a.Score),
                EqualityComparer<string>.Default);
        }
    }
}
