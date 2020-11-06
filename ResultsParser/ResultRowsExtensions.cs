using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using iRLeagueManager.Models.Results;

namespace iRLeagueManager.ResultsParser
{
    public static class ResultRowsExtensions 
    {
        public static IEnumerable<ResultRowModel> MapToCollection(this IEnumerable<ResultRowModel> rows, IEnumerable<ResultRowModel> with)
        {
            List<ResultRowModel> keep = new List<ResultRowModel>();

            foreach(var withRow in with)
            {
                var keepRow = rows.SingleOrDefault(x => x.MemberId == withRow.MemberId);
                if (keepRow != null)
                {
                    keepRow.CopyFrom(withRow, nameof(ResultRowModel.ResultRowId));
                }
                else
                {
                    keepRow = withRow;
                }
                keep.Add(keepRow);
            }

            return keep;
        }
    }
}
