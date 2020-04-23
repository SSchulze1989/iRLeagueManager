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

        public override int PositionChange => StartPosition - FinalPosition;

        public ScoredResultRowModel() : base() { }

        new public static ScoredResultRowModel GetTemplate()
        {
            var template = new ScoredResultRowModel
            {
                //template.CopyFrom(ResultRowModel.GetTemplate());
                RacePoints = 0,
                BonusPoints = 0,
                PenaltyPoints = 0,
                FinalPosition = 0,
                Member = Members.LeagueMember.GetTemplate()
            };

            return template;
        }
    }
}
