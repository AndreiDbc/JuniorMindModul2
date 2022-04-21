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

        [Fact]
        public void AddResultAwayTeamWinsAwayTeamShouldHaveMorePoints()
        {
            Team homeTeam = new Team("FC SB", 16);
            Team awayTeam = new Team("U Cluj", 14);
            Match match = new Match(homeTeam, awayTeam, 1, 2);
            match.AddPoints(match);
            Assert.True(awayTeam.HasMorePoints(homeTeam));
        }

        [Fact]
        public void AddResult()
        {
            Team testTeam = new Team("Dinamo", 12);
            Team homeTeam = new Team("FC SB", 12);
            Team awayTeam = new Team("U Cluj", 14);
            Match match = new Match(homeTeam, awayTeam, 1, 1);
            match.AddPoints(match);
            Ranking ranking = new Ranking();
            ranking.Add(testTeam);
            ranking.Add(homeTeam);
            ranking.Add(awayTeam);
            ranking.Update(match);
            Assert.Equal(1, ranking.RankOf(awayTeam));
            Assert.Equal(2, ranking.RankOf(homeTeam));
            Assert.Equal(3, ranking.RankOf(testTeam));
        }
    }
}
