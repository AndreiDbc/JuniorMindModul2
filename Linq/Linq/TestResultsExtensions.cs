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
            var dictionary = new Dictionary<string, TestResults>();
            return testResults.Aggregate(dictionary, (x, y) =>
            {
                if (x.ContainsKey(y.FamilyId))
                {
                    x[y.FamilyId].Score = y.Score > x[y.FamilyId].Score ? y.Score : x[y.FamilyId].Score;
                }
                else
                {
                    x.Add(y.FamilyId, y);
                }

                return x;
            }).Values;
        }
    }
}
