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
        public void CloneIsCalledSouldReturnNewObjectWithSameData()
        {
            Team team = new Team("FC SB", 12);
            Assert.False(team.Clone().Equals(team));
            Assert.True(team.Clone().DoNamesMatch(team));
            Assert.True(team.Clone().HasEqualPoints(team));
        }

        [Fact]
        public void DoNamesMatchIsGivenOneObjectToCompareShouldReturnTrue()
        {
            Team team = new Team("FC SB", 12);
            Team testName = new Team("FC SB", 12);
            Assert.True(team.DoNamesMatch(testName));
        }

        [Fact]
        public void DoNamesMatchIsGivenOneObjectToCompareShouldReturnFalse()
        {
            Team team = new Team("FC SB", 12);
            Team testName = new Team("U Cluj", 12);
            Assert.False(team.DoNamesMatch(testName));
        }

        [Fact]
        public void HasMorePointsIsGivenTeamWithLessPointsShouldReturnTrue()
        {
            Team team = new Team("FC SB", 12);
            Team testTeam = new Team("U Cluj", 13);
            Assert.True(testTeam.HasMorePoints(team));
        }

        [Fact]
        public void HasMorePointsIsGivenTeamWithMorePointsShouldReturnFalse()
        {
            Team team = new Team("FC SB", 15);
            Team testTeam = new Team("U Cluj", 13);
            Assert.False(testTeam.HasMorePoints(team));
        }

        [Fact]
        public void AddWinIsCalledShouldAddThreeMorePoints()
        {
            Team team = new Team("FC SB", 12);
            Team testTeam = new Team("U Cluj", 14);
            team.AddWin();
            Assert.True(team.HasMorePoints(testTeam));
        }

        [Fact]
        public void AddWinIsGivenNegativeValueShouldReturnSameValue()
        {
            Team team = new Team("FC SB", -2);
            Team testTeam = new Team("U Cluj", -1);
            team.AddWin();
            Assert.False(team.HasMorePoints(testTeam));
        }

        [Fact]
        public void AddDrawIsCalledShouldAddOnePoint()
        {
            Team team = new Team("FC SB", 14);
            Team testTeam = new Team("U Cluj", 14);
            team.AddDraw();
            Assert.True(team.HasMorePoints(testTeam));
        }

        [Fact]
        public void AddDrawIsGivenNegativeValueShouldReturnSameValue()
        {
            Team team = new Team("FC SB", -14);
            Team testTeam = new Team("U Cluj", -14);
            team.AddDraw();
            Assert.True(team.HasEqualPoints(testTeam));
        }
    }
}
