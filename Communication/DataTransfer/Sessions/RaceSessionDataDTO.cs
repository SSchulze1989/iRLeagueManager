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
using System.Runtime.Serialization;

namespace iRLeagueDatabase.DataTransfer.Sessions
{
    [DataContract]
    [KnownType(typeof(SessionDataDTO))]
    public class RaceSessionDataDTO : SessionDataDTO, IMappableDTO
    {
        [DataMember]
        /// <summary>
        /// Unique race id for the league
        /// </summary>
        public long RaceId { get; set; }

        [DataMember]
        /// <summary>
        /// Number of laps for the race. Set to 0 for time based races.
        /// </summary>
        public int Laps { get; set; }

        [DataMember]
        /// <summary>
        /// Length of the free practice. Set to 0:00:00 for no practice or warmup.
        /// </summary>
        public TimeSpan PracticeLength { get; set; }

        [DataMember]
        /// <summary>
        /// Length of the attached qualifying. Set to 0:00:00 for no attached qualy.
        /// </summary>
        public TimeSpan QualyLength { get; set; }

        [DataMember]
        /// <summary>
        /// Length of the race. If length is not time limited - set to 0:00:00
        /// </summary>
        public TimeSpan RaceLength { get; set; }

        [DataMember]
        /// <summary>
        /// Session id from iracing.com service
        /// </summary>
        public string IrSessionId { get; set; }

        [DataMember]
        /// <summary>
        /// Link to the iracing.com results page.
        /// </summary>
        public string IrResultLink { get; set; }

        [DataMember]
        /// <summary>
        /// Check if session has attached qualifying
        /// </summary>
        public bool QualyAttached { get; set; }

        [DataMember]
        /// <summary>
        /// Check if session has attached free-practice or warmup
        /// </summary>
        public bool PracticeAttached { get; set; }
    }
}
