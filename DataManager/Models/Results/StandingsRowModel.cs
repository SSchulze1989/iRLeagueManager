using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iRLeagueManager.Models.Results
{
    public class StandingsRowModel : ModelBase
    {
        public long? StandingsRowId { get; internal set; }

        public override long[] ModelId => new long[] { StandingsRowId.GetValueOrDefault() };

        private int pos;
        public int Pos { get => pos; internal set => SetValue(ref pos, value); }

        private string name;
        public string Name { get => name; set => SetValue(ref name, value); }

        private int change;
        public int Change { get => change; internal set => SetValue(ref change, value); }

        private string team;
        public string Team { get => team; set => SetValue(ref team, value); }
        
        private int points;
        public int Points { get => points; set => SetValue(ref points, value); }

        private int penaltyPoints;
        public int PenaltyPoints { get => penaltyPoints; set => SetValue(ref penaltyPoints, value); }

        private int pointsChange;
        public int PointsChange { get => pointsChange; set => SetValue(ref pointsChange, value); }

        private int racesCounted;
        public int RacesCounted { get => racesCounted; set => SetValue(ref racesCounted, value); }

        private int racesParticipated;
        public int RacesParticipated { get => racesParticipated; set => SetValue(ref racesParticipated, value); }

        private int wins;
        public int Wins { get => wins; set => SetValue(ref wins, value); }

        private int poles;
        public int Poles { get => poles; set => SetValue(ref poles, value); }

        private int top3;
        public int Top3 { get => top3; set => SetValue(ref top3, value); }

        private int top5;
        public int Top5 { get => top5; set => SetValue(ref top5, value); }

        private int top10;
        public int Top10 { get => top10; set => SetValue(ref top10, value); }

        private int top15;
        public int Top15 { get => top15; set => SetValue(ref top15, value); }

        private int top20;
        public int Top20 { get => top20; set => SetValue(ref top20, value); }

        private int fastestLaps;
        public int FastestLaps { get => fastestLaps; set => SetValue(ref fastestLaps, value); }
    }
}
