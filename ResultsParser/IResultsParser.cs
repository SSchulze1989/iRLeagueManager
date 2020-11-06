using iRLeagueManager.Models.Members;
using iRLeagueManager.Models.Results;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iRLeagueManager.ResultsParser
{
    public interface IResultsParser
    {
        IEnumerable<LeagueMember> MemberList { get; set; }
        IEnumerable<TeamModel> TeamList { get; set; }
        Task ReadStreamAsync(StreamReader reader);
        IEnumerable<LeagueMember> GetNewMemberList();
        IEnumerable<ResultRowModel> GetResultRows();
    }
}
