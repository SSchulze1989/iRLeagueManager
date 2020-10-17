using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iRLeagueManager.Models.Reviews
{
    public class ReviewPenaltyModel : MappableModel
    {
        private long resultRowId;
        public long ResultRowId { get => resultRowId; set => SetValue(ref resultRowId, value); }
        
        private long reviewId;
        public long ReviewId { get => reviewId; set => SetValue(ref reviewId, value); }
        
        private int penaltyPoints;
        public int PenaltyPoints { get => penaltyPoints; set => SetValue(ref penaltyPoints, value); }

        private ReviewVoteModel reviewVote;
        public ReviewVoteModel ReviewVote { get => reviewVote; set => SetValue(ref reviewVote, value); }

        public override long[] ModelId => new long[] { ResultRowId, ReviewId };
    }
}
