using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ranks.Facts
{
    public class Ranking
    {
        private Team[] teams;

        public Ranking(Team[] teams)
        {
            this.teams = teams;
        }

        public void AddTeamToRanks(Team newTeam)
        {
            Array.Resize(ref teams, teams.Length + 1);
            teams[^1] = newTeam;
        }

        public Team GetTeamAtRank(int index)
        {
            BubbleSort();
            return teams[index];
        }

        public int GetTeamScore(string teamName)
        {
            for (int i = 0; i < teams.Length; i++)
            {
                if (teams[i].GetName() == teamName)
                {
                    return teams[i].GetPoints();
                }
            }

            return 0;
        }

        public void UpdateRankScores(Team team, int points)
        {
            team.AddPoints(points);

            BubbleSort();
        }

        private void BubbleSort()
        {
            for (int j = 0; j < teams.Length - 1; j++)
            {
                for (int i = 0; i < teams.Length - 1; i++)
                {
                    Swap(i, i + 1);
                }
            }
        }

        private void Swap(int firstValue, int secondValue)
        {
            if (teams[firstValue].GetPoints() >= teams[secondValue].GetPoints())
            {
                return;
            }

            Team temp = teams[firstValue];
            teams[firstValue] = teams[secondValue];
            teams[secondValue] = temp;
        }
    }
}
