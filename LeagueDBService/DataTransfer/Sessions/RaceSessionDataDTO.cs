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
        public int RaceId { get; set; }

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
