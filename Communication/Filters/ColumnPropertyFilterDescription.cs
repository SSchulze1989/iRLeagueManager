using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iRLeagueDatabase.Filters
{
    public class ColumnPropertyFilterDescription : IResultsFilterDescription
    {
        public string FilterName => "Column value Filter";

        public string FilterDescription => "Select results based on comparing a single column to a static value";

        public int NrOfFilterValues => 3;

        public IEnumerable<string> FilterValueDescriptions => new string[] { "Column Name", "Operator", "Value" };
    }
}
