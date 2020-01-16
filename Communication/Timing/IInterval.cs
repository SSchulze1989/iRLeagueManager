using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iRLeagueManager.Timing
{
    public interface IInterval : ILapTime
    {
        int Laps { get; }
    }
}
