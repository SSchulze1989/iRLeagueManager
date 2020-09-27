using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iRLeagueManager.Models.Reviews
{
    public class VoteCategoryModel : MappableModel
    {
        private long catId;
        public long CatId { get => catId; set => SetValue(ref catId, value); }

        private string text;
        public string Text { get => text; set => SetValue(ref text, value); }

        private int index;
        public int Index { get => index; set => SetValue(ref index, value); }

        private int defaultPenalty;
        public int DefaultPenalty { get => defaultPenalty; set => SetValue(ref defaultPenalty, value); }

        public override long[] ModelId => new long[] { CatId };
    }
}
