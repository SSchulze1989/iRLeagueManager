// MIT License

// Copyright (c) 2020 Simon Schulze

// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:

// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.

// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iRLeagueManager.Timing;
using iRLeagueManager.Enums;
using iRLeagueManager.Attributes;

namespace iRLeagueManager.Interfaces
{
    public interface IResultRow
    {
        //int FinalPosition { get; set; }
        int StartPosition { get; set; }
        int FinishPosition { get; set; }
        [EqualityCheckProperty]
        //int MemberId { get; }
        ILeagueMember Member { get; }
        [EqualityCheckProperty]
        int CarNumber { get; set; }
        int ClassId { get; set; }
        string Car { get; set; }
        string CarClass { get; set; }
        int CompletedLaps { get; set; }
        int LeadLaps { get; set; }
        int FastLapNr { get; set; }
        int Incidents { get; set; }
        RaceStatusEnum Status { get; set; }
        //int RacePoints { get; set; }
        //int BonusPoints { get; set; }
        LapTime QualifyingTime { get; set; }
        LapInterval Interval { get; set; }
        LapTime AvgLapTime { get; set; }
        LapTime FastestLapTime { get; set; }
        //int PositionChange { get; set; }
    }
}
