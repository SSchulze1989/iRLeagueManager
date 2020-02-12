using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iRLeagueDatabase.DataTransfer.Results
{
    public class StandingsRowDTO
    {
        public int MemberId { get; set; }

        public int Pos { get; set; }
        
        public string Name { get; set; }

        public int Change { get; set; }

        public string Team { get; set; }

        public int Points { get; set; }

        public int PenaltyPoints { get; set; }

        public int PointsChange { get; set; }

        public int RacesCounted { get; set; }

        public int RacesParticipated { get; set; }

        public int Wins { get; set; }

        public int Poles { get; set; }

        public int Top3 { get; set; }

        public int Top5 { get; set; }

        public int Top10 { get; set; }

        public int Top15 { get; set; }

        public int Top20 { get; set; }

        public int FastestLaps { get; set; }
    }
}
