using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iRLeagueManager.Attributes;

namespace iRLeagueManager.Interfaces
{
    public interface IRaceSession : ISession, IRaceSessionInfo
    {
        int Laps { get; set; }
        string IrResultLink { get; set; }
        string IrSessionId { get; set; }
        TimeSpan PracticeLength { get; set; }
        TimeSpan QualyLength { get; set; }
        TimeSpan RaceLength { get; set; }
        bool QualyAttached { get; set; }
        bool PracticeAttached { get; set; }
    }
}
