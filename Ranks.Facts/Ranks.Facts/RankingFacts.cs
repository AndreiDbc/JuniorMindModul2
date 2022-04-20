using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Ranks.Facts
{
    public class RankingFacts
    {
        [Fact]
        public void AddTeamIsGivenFourTeamsToAddShouldReturnUpdatedArray()
        {
            Team team1 = new Team("FC SB", 12);
            Team team2 = new Team("U Cluj", 5);
            Team team3 = new Team("Dinamo", 4);
            Team team4 = new Team("Urziceni", 6);
            Ranking ranking = new Ranking();
            ranking.Add(team1);
            ranking.Add(team2);
            ranking.Add(team3);
            ranking.Add(team4);
            Assert.True(team1.DoNamesMatch(ranking.GetTeamAt(0)));
            Assert.True(team4.DoNamesMatch(ranking.GetTeamAt(1)));
            Assert.True(team2.DoNamesMatch(ranking.GetTeamAt(2)));
            Assert.True(team3.DoNamesMatch(ranking.GetTeamAt(3)));
        }

        [Fact]
        public void AddTeamArrayHasFourValuesShouldSortTeamsBasedOnPoints()
        {
            Team team1 = new Team("FC SB", 5);
            Team team2 = new Team("U Cluj", 7);
            Team team3 = new Team("Dinamo", 3);
            Team team4 = new Team("Urziceni", 12);
            Ranking ranking = new Ranking();
            ranking.Add(team1);
            ranking.Add(team2);
            ranking.Add(team3);
            ranking.Add(team4);
            Assert.Equal(3, ranking.RankOf(team1));
            Assert.Equal(2, ranking.RankOf(team2));
            Assert.Equal(4, ranking.RankOf(team3));
            Assert.Equal(1, ranking.RankOf(team4));
        }

        [Fact]
        public void GetTeamAtIsCalledShouldReturnTeam()
        {
            Team team1 = new Team("FC SB", 12);
            Team team2 = new Team("U Cluj", 8);
            Team team3 = new Team("Dinamo", 4);
            Ranking ranking = new Ranking();
            ranking.Add(team1);
            ranking.Add(team2);
            ranking.Add(team3);
            Assert.True(team1.DoNamesMatch(ranking.GetTeamAt(0)));
            Assert.True(team2.DoNamesMatch(ranking.GetTeamAt(1)));
            Assert.True(team3.DoNamesMatch(ranking.GetTeamAt(2)));
        }

        [Fact]
        public void RankOfIsGivenNameThatDoesntBelongToAnyTeamShouldReturn0()
        {
            Team team1 = new Team("FC SB", 12);
            Team team2 = new Team("U Cluj", 4);
            Team team3 = new Team("Dinamo", 8);
            Team team4 = new Team("Urziceni", 2);
            Ranking ranking = new Ranking();
            ranking.Add(team1);
            ranking.Add(team2);
            ranking.Add(team3);
            Assert.Equal(0, ranking.RankOf(team4));
        }

        [Fact]
        public void RankOfIsCalledShouldReturnPositions()
        {
            Team team1 = new Team("FC SB", 12);
            Team team2 = new Team("U Cluj", 4);
            Team team3 = new Team("Dinamo", 8);
            Ranking ranking = new Ranking();
            ranking.Add(team1);
            ranking.Add(team2);
            ranking.Add(team3);
            Assert.Equal(1, ranking.RankOf(team1));
            Assert.Equal(3, ranking.RankOf(team2));
            Assert.Equal(2, ranking.RankOf(team3));
        }

        [Fact]
        public void UpdateHomeTeamWinsShouldReturnNewPositions()
        {
            Team team1 = new Team("FC SB", 12);
            Team team2 = new Team("U Cluj", 4);
            Team team3 = new Team("Dinamo", 6);
            Ranking ranking = new Ranking();
            ranking.Add(team1);
            ranking.Add(team2);
            ranking.Add(team3);
            Match match = new Match(team2, team3, 2, 1);
            ranking.Update(match);
            Assert.Equal(1, ranking.RankOf(team1));
            Assert.Equal(2, ranking.RankOf(team2));
            Assert.Equal(3, ranking.RankOf(team3));
        }

        [Fact]
        public void UpdateAwayTeamWinsShouldReturnNewPositions()
        {
            Team team1 = new Team("FC SB", 12);
            Team team2 = new Team("U Cluj", 6);
            Team team3 = new Team("Dinamo", 4);
            Ranking ranking = new Ranking();
            ranking.Add(team1);
            ranking.Add(team2);
            ranking.Add(team3);
            Match match = new Match(team2, team3, 2, 1);
            ranking.Update(match);
            Assert.Equal(1, ranking.RankOf(team1));
            Assert.Equal(2, ranking.RankOf(team2));
            Assert.Equal(3, ranking.RankOf(team3));
        }

        [Fact]
        public void UpdateIsGivenADrawShouldReturnNewPositions()
        {
            Team team1 = new Team("FC SB", 12);
            Team team2 = new Team("U Cluj", 5);
            Team team3 = new Team("Dinamo", 5);
            Ranking ranking = new Ranking();
            ranking.Add(team1);
            ranking.Add(team2);
            ranking.Add(team3);
            Match match = new Match(team2, team3, 1, 1);
            ranking.Update(match);
            Assert.True(team2.HasEqualPoints(team3));
        }

        [Fact]
        public void UpdateIsGivenADrawHomeTeamLeadsShouldReturnNewPositions()
        {
            Team team1 = new Team("FC SB", 12);
            Team team2 = new Team("U Cluj", 5);
            Team team3 = new Team("Dinamo", 4);
            Ranking ranking = new Ranking();
            ranking.Add(team1);
            ranking.Add(team2);
            ranking.Add(team3);
            Match match = new Match(team2, team3, 1, 1);
            ranking.Update(match);
            Assert.True(team2.HasMorePoints(team3));
        }
    }
}