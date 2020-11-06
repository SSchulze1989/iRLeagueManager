using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iRLeagueDatabase.Filters
{
    public class SeasonStartIRatingFilterDescription : IResultsFilterDescription
    {
        public string FilterName => "Season start iRating";

        public string FilterDescription => "Filter based on iRating at first appearance in this season";

        public int NrOfFilterValues => 1;

        public IEnumerable<string> FilterValueDescriptions => new string[] { "Cutoff iRating value" };
    }
}
