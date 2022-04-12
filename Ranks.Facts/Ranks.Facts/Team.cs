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

        public string GetName()
        {
            return this.name;
        }

        public int GetPoints()
        {
            return this.points;
        }

        public void AddPoints(int points)
        {
            this.points += points;
        }
    }
}
