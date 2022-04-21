using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Ranks.Facts
{
    public class MatchFacts
    {
        [Fact]
        public void GetHomeTeamIsCalledShouldReturnNewObjectWithSameData()
        {
            Team homeTeam = new Team("FC SB", 12);
            Team awayTeam = new Team("U Cluj", 23);
            Match match = new Match(homeTeam, awayTeam, 1, 2);
            Assert.False(match.GetHomeTeam().Equals(homeTeam));
            Assert.True(match.GetHomeTeam().DoNamesMatch(homeTeam));
        }

        [Fact]
        public void GetAwayTeamIsCalledShouldReturnNewObjectWithSameData()
        {
            Team homeTeam = new Team("FC SB", 12);
            Team awayTeam = new Team("U Cluj", 23);
            Match match = new Match(homeTeam, awayTeam, 1, 2);
            Assert.False(match.GetAwayTeam().Equals(awayTeam));
            Assert.True(match.GetAwayTeam().DoNamesMatch(awayTeam));
        }

        [Fact]
        public void GetHomeTeamGoalsIsCalledShouldReturnGoals()
        {
            Team homeTeam = new Team("FC SB", 12);
            Team awayTeam = new Team("U Cluj", 23);
            Match match = new Match(homeTeam, awayTeam, 1, 2);
            Assert.Equal(1, match.GetHomeTeamGoals());
        }

        [Fact]
        public void GetAwayTeamGoalsIsCalledShouldReturnGoals()
        {
            Team homeTeam = new Team("FC SB", 12);
            Team awayTeam = new Team("U Cluj", 23);
            Match match = new Match(homeTeam, awayTeam, 1, 2);
            Assert.Equal(2, match.GetAwayTeamGoals());
        }
    }
}
