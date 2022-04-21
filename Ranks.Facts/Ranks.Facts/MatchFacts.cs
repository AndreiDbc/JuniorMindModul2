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
        public void AddResultAwayTeamWinsAwayTeamShouldHaveMorePoints()
        {
            Team homeTeam = new Team("FC SB", 16);
            Team awayTeam = new Team("U Cluj", 14);
            Match match = new Match(homeTeam, awayTeam, 1, 2);
            match.AddPoints();
            Assert.True(awayTeam.HasMorePoints(homeTeam));
        }

        [Fact]
        public void AddResultIsGivenADrawSouldUpdatePositionsAccordingly()
        {
            Team testTeam = new Team("Dinamo", 12);
            Team homeTeam = new Team("FC SB", 12);
            Team awayTeam = new Team("U Cluj", 14);
            Match match = new Match(homeTeam, awayTeam, 1, 1);
            match.AddPoints();
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
