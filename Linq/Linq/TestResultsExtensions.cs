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
                    Score = value.Aggregate(0, (x, y) =>
                    {
                        if (x < y.Score)
                        {
                            x = y.Score;
                        }

                        return x;
                    })
                },
                EqualityComparer<string>.Default);
        }
    }
}
