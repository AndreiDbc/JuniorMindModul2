using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Ranks.Facts
{
    public class RankingFacts
    {
        [Fact]
        public void GetTeamAtRankFunctionIsGivenMiddleTeamScore()
        {
            Team team1 = new Team("FC SB", 12);
            Team team2 = new Team("U Cluj", 8);
            Team team3 = new Team("Dinamo", 4);
            Team[] testRank = { team1, team2, team3 };
            Ranking testRanking = new Ranking(testRank);
            Assert.Equal(team2.GetName(), testRanking.GetTeamAtRank(1).GetName());
        }

        [Fact]
        public void GetTeamAtRankArrayHasFourValuesShouldSortTeamsBasedOnPoints()
        {
            Team team1 = new Team("FC SB", 1);
            Team team2 = new Team("U Cluj", 12);
            Team team3 = new Team("Dinamo", 8);
            Team team4 = new Team("Urziceni", 5);
            Team[] testRank = { team1, team2, team3, team4 };
            Ranking testRanking = new Ranking(testRank);
            Assert.Equal(team2.GetName(), testRanking.GetTeamAtRank(0).GetName());
            Assert.Equal(team3.GetName(), testRanking.GetTeamAtRank(1).GetName());
            Assert.Equal(team4.GetName(), testRanking.GetTeamAtRank(2).GetName());
            Assert.Equal(team1.GetName(), testRanking.GetTeamAtRank(3).GetName());
        }

        [Fact]
        public void AddTeamToRanksArrayIsGivenOneTeamToAddShouldReturnUpdatedArray()
        {
            Team team1 = new Team("FC SB", 12);
            Team team2 = new Team("U Cluj", 4);
            Team team3 = new Team("Dinamo", 2);
            Team[] ranks = { team1, team2 };
            Ranking ranking = new Ranking(ranks);
            ranking.AddTeamToRanks(team3);
            Assert.Equal(team1, ranking.GetTeamAtRank(0));
            Assert.Equal(team2, ranking.GetTeamAtRank(1));
            Assert.Equal(team3, ranking.GetTeamAtRank(2));
        }

        [Fact]
        public void GetTeamScoreFunctionIsGivenNameThatDoesntBelongToAnyTeamShouldReturn0()
        {
            Team team1 = new Team("FC SB", 12);
            Team team2 = new Team("U Cluj", 4);
            Team team3 = new Team("Dinamo", 8);
            Team[] testRank = { team1, team2, team3 };
            Ranking testRanking = new Ranking(testRank);
            Assert.Equal(0, testRanking.GetTeamScore("Urziceni"));
        }

        [Fact]
        public void GetTeamScoreFunctionIsGivenLastNameFromArrayShouldReturnPoints()
        {
            Team team1 = new Team("FC SB", 12);
            Team team2 = new Team("U Cluj", 4);
            Team team3 = new Team("Dinamo", 8);
            Team[] testRank = { team1, team2, team3 };
            Ranking testRanking = new Ranking(testRank);
            Assert.Equal(8, testRanking.GetTeamScore("Dinamo"));
        }

        [Fact]
        public void UpdateRanksScoresFunctionIsGivenUpdateToATeamShouldUpdatedTeamScore()
        {
            Team team1 = new Team("FC SB", 12);
            Team team2 = new Team("U Cluj", 4);
            Team team3 = new Team("Dinamo", 8);
            Team[] testRank = { team1, team2, team3 };
            Ranking testRanking = new Ranking(testRank);
            testRanking.UpdateRankScores(team3, 5);
            Assert.Equal(13, team3.GetPoints());
        }
    }
}