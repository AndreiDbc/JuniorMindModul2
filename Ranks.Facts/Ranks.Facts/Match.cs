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

        public void AddPoints(Match match)
        {
            if (match.GetHomeTeamGoals() > match.GetAwayTeamGoals() && homeTeam.DoNamesMatch(match.GetHomeTeam()))
            {
                homeTeam.AddWin();
            }
            else if (match.GetHomeTeamGoals() < match.GetAwayTeamGoals() && awayTeam.DoNamesMatch(match.GetAwayTeam()))
            {
                awayTeam.AddWin();
            }
            else if (match.GetHomeTeamGoals() == match.GetAwayTeamGoals() && homeTeam.DoNamesMatch(match.GetHomeTeam()) && awayTeam.DoNamesMatch(match.GetAwayTeam()))
            {
                awayTeam.AddDraw();
                homeTeam.AddDraw();
            }
        }

        public int GetHomeTeamGoals()
        {
            return homeTeamGoals;
        }

        public int GetAwayTeamGoals()
        {
            return awayTeamGoals;
        }

        private Team GetHomeTeam()
        {
            return this.homeTeam;
        }

        private Team GetAwayTeam()
        {
            return this.awayTeam;
        }
    }
}