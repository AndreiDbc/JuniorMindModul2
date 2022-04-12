using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Ranks.Facts
{
    public class TeamFacts
    {
        [Fact]
        public void GetNameMehtodIsGivenOneObjectShouldReturnObjectName()
        {
            Team team = new Team("FC SB", 12);
            const string testName = "FC SB";
            Assert.Equal(testName, team.GetName());
        }

        [Fact]
        public void GetPointsMehtodIsGivenOneObjectShouldReturnObjectPoints()
        {
            Team team = new Team("FC SB", 12);
            const int TestPoints = 12;
            Assert.Equal(TestPoints, team.GetPoints());
        }

        [Fact]
        public void AddPointsMehtodIsGivenOneObjectShouldAddThreeMorePoints()
        {
            Team team = new Team("FC SB", 12);
            Team testTeam = new Team("FC SB", 15);
            team.AddPoints(3);
            Assert.Equal(team.GetPoints(), testTeam.GetPoints());
        }
    }
}
