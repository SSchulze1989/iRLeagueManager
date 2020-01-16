using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iRLeagueManager.Attributes;

namespace iRLeagueManager.Interfaces
{
    public interface IRaceInfo
    {
        [EqualityCheckProperty]
        uint RaceId { get; }
        int Laps { get; }
        string IrResultLink { get; }
        string IrSessionId { get; }
        TimeSpan PracticeLength { get; }
        TimeSpan QualyLength { get; }
        TimeSpan RaceLength { get; }
        bool QualyAttached { get; }
        bool PracticeAttached { get; }
    }
}
