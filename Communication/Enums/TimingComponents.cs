using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iRLeagueManager.Enums
{
    [Flags]
    public enum TimingComponents
    {
        Milliseconds    = 0b00000001,
        Seconds         = 0b00000010,
        Minutes         = 0b00000100,
        Hours           = 0b00001000,
        Days            = 0b00010000
    }
}
