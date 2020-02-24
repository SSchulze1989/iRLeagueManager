using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using iRLeagueManager.Data;
using iRLeagueManager.Models;
using iRLeagueManager.Models.Results;

namespace TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var LeagueContext = new LeagueContext();

            var season = LeagueContext.GetModelAsync<SeasonModel>(1).Result;
            var scoring = new ScoringModel()
            {
                Name = "TestScoring",
                AverageRaceNr = -1,
                BasePoints = "20 19 18 17 16 15 14 13 12 11 10 09 08 07 06 05 04 03 02 01",
                BonusPoints = "p1:5 p2:3 p3:1",
                DropWeeks = 3,
                Season = season
            };
            season.Scorings.Add(scoring);
            season.MainScoring = scoring;
            LeagueContext.UpdateModelAsync(season).Wait();
        }
    }
}
