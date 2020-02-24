using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iRLeagueManager.Models.Results
{
    public class ScoredResultRowModel : ResultRowModel
    {
        private int racePoints;
        public int RacePoints { get => racePoints; set => SetValue(ref racePoints, value); }

        private int bonusPoints;
        public int BonusPoints { get => bonusPoints; set => SetValue(ref bonusPoints, value); }

        private int penaltyPoints;
        public int PenaltyPoints { get => penaltyPoints; set => SetValue(ref penaltyPoints, value); }

        private int finalPosition;
        public int FinalPosition { get => finalPosition; set => SetValue(ref finalPosition, value); }

        public int TotalPoints => RacePoints + BonusPoints - PenaltyPoints;

        public override int PositionChange => FinalPosition - StartPosition;
    }
}
