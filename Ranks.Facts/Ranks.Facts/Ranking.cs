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

        public Ranking()
        {
            this.teams = new Team[0];
        }

        public void Add(Team newTeam)
        {
            Array.Resize(ref teams, teams.Length + 1);
            teams[^1] = newTeam;
            ShellSort();
        }

        public Team GetTeamAt(int index)
        {
            return teams[index];
        }

        public int RankOf(Team team)
        {
            for (int i = 0; i < teams.Length; i++)
            {
                if (teams[i].Equals(team))
                {
                    return i + 1;
                }
            }

            return 0;
        }

        public void Update(Match match)
        {
            match.AddPoints();
            ShellSort();
        }

        private void ShellSort()
        {
            const int two = 2;
            int i;
            int j;
            Team temp;
            int distance = 3;
            while (distance > 0)
            {
                for (i = 0; i < teams.Length; i++)
                {
                    j = i;
                    temp = teams[i];
                    Swap(temp, ref j, distance);
                    teams[j] = temp;
                }

                distance /= two;
            }
        }

        private void Swap(Team temp, ref int j, int distance)
        {
            while ((j >= distance) && temp.HasMorePoints(teams[j - distance]))
            {
                teams[j] = teams[j - distance];
                j -= distance;
            }
        }
    }
}
