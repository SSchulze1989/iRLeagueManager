using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iRLeagueDatabase.Filters
{
    public class MemberListFilterDescription : IResultsFilterDescription
    {
        public string FilterName => "Member List";

        public string FilterDescription => "Include/Exclude members based on a static list";

        public int NrOfFilterValues => -1;

        public IEnumerable<string> FilterValueDescriptions => new string[] { "Select member" };
    }
}
