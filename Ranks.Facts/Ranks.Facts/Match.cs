namespace Ranks.Facts
{
    public class Match
    {
        private readonly Team homeTeam;
        private readonly Team awayTeam;
        private readonly int homeTeamGoals;
        private readonly int awayTeamGoals;

        public Match(Team homeTeam, Team awayTeam, int homeTeamGoals, int awayTeamGoals)
        {
            this.homeTeam = homeTeam;
            this.awayTeam = awayTeam;
            this.homeTeamGoals = homeTeamGoals;
            this.awayTeamGoals = awayTeamGoals;
        }

        public Team GetHomeTeam()
        {
            return homeTeam.Clone();
        }

        public Team GetAwayTeam()
        {
            return awayTeam.Clone();
        }

        public int GetHomeTeamGoals()
        {
            return homeTeamGoals;
        }

        public int GetAwayTeamGoals()
        {
            return awayTeamGoals;
        }
    }
}