using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ranks.Facts
{
    public class Team
    {
        private readonly string name;
        private int points;

        public Team(string name, int points)
        {
            this.name = name;
            this.points = points;
        }

        public Team Clone()
        {
            return new Team(name, points);
        }

        public bool DoNamesMatch(Team toMatch)
        {
            return this.name == toMatch.name;
        }

        public bool HasMorePoints(Team secondTeam)
        {
            return this.points > secondTeam.points;
        }

        public bool HasEqualPoints(Team secondTeam)
        {
            return this.points == secondTeam.points;
        }

        public void AddWin()
        {
            if (points < 0)
            {
                return;
            }

            points += 3;
        }

        public void AddDraw()
        {
            if (points < 0)
            {
                return;
            }

            points++;
        }
    }
}
