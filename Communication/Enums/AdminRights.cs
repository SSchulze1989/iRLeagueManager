using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iRLeagueManager.Enums
{
    [Flags]
    public enum AdminRights
    {
        Member                  = 0b00000000,
        EditMemberData          = 0b00000001,
        EditLeagueData          = 0b00000010,
        DeleteMemberData        = 0b00000100,
        DeleteLeagueData        = 0b00001000,
        EditDeleteMemberData    = 0b00000101,
        EditDeleteLeagueData    = 0b00001010,
        RightManager            = 0b00010001,
        Owner                   = 0b11111111
    }
}
