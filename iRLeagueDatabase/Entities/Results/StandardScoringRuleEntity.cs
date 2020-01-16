using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iRLeagueDatabase.Entities.Results
{
    public class StandardScoringRuleEntity : ScoringRuleBaseEntity
    {
        /// <summary>
        /// Maximum Points Rewarded for the first place finisher
        /// </summary>
        public int MaxPoints { get; set; }
        /// <summary>
        /// Drop-off of points per place
        /// Eg.: PlaceDropOff = 1 --> 1st = 10pts, 2nd = 9 pts, ..., 10th = 1, 11th = 0,... 
        /// </summary>
        public int PlaceDropOff { get; set; }

        /// <summary>
        /// Calculate Championship points for a single position
        /// </summary>
        /// <param name="place">Finish position</param>
        /// <returns>Championship points</returns>
        public override int GetSingleChampPoint(int place)
        {
            return Math.Max(MaxPoints - (place - 1) * PlaceDropOff, 0);
        }

        public override Dictionary<int, int> GetChampPoints(ResultEntity result)
        {
            return result.RawResults.ToDictionary(k => k.Member.MemberId, v => GetSingleChampPoint(v.FinalPosition));
        }
    }
}
