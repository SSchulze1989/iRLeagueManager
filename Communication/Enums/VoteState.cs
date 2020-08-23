﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iRLeagueManager.Enums
{
    public enum VoteState
    {
        NoVote,
        Open,
        VotesNeeded,
        Conflict,
        MajorityVote,
        Agreed,
        Closed
    }
}
