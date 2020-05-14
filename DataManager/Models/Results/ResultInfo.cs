using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using iRLeagueManager.Models;
using iRLeagueManager.Models.Sessions;

namespace iRLeagueManager.Models.Results
{
    public class ResultInfo : ModelBase
    {
        public long? ResultId { get; internal set; }

        public override long[] ModelId => new long[] { ResultId.GetValueOrDefault() };

        private SessionInfo session;
        public SessionInfo Session { get => session; set => SetValue(ref session, value); }

        public ResultInfo(long? resultId)
        {
            ResultId = resultId;
        }
    }
}
