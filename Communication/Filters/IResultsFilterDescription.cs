using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iRLeagueDatabase.Filters
{
    public interface IResultsFilterDescription
    {
        string FilterName { get; }
        string FilterDescription { get; }
        int NrOfFilterValues { get; }
        IEnumerable<string> FilterValueDescriptions { get; }
    }
}
