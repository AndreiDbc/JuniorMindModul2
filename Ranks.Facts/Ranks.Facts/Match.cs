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

        public void AddPoints()
        {
            if (homeTeamGoals > awayTeamGoals)
            {
                homeTeam.AddWin();
            }
            else if (homeTeamGoals < awayTeamGoals)
            {
                awayTeam.AddWin();
            }
            else
            {
                awayTeam.AddDraw();
                homeTeam.AddDraw();
            }
        }
    }
}