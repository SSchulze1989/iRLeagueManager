using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iRLeagueDatabase.DataTransfer.Reviews
{
    public class VoteCategoryDTO : MappableDTO
    {
        public long CatId { get; set; }
        public string Text { get; set; }
        public int Index { get; set; }
        public int DefaultPenalty { get; set; }

        public override object MappingId => CatId;
        public override object[] Keys => new object[] { CatId };
    }
}
