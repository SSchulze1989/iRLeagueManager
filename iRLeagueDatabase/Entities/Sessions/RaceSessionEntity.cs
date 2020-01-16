using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Xml;
using System.Xml.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using iRLeagueDatabase.Entities.Results;
using iRLeagueManager.Enums;

namespace iRLeagueDatabase.Entities.Sessions
{
    /// <summary>
    /// Session including a race. Can have attached qualy or practice.
    /// </summary>
    [Serializable()]
    public class RaceSessionEntity : SessionBaseEntity
    {
        /// <summary>
        /// Unique race id for the league
        /// </summary>
        [Key]
        public int RaceId { get; set; }
        
        /// <summary>
        /// Number of laps for the race. Set to 0 for time based races.
        /// </summary>
        public int Laps { get; set; }

        /// <summary>
        /// Length of the free practice. Set to 0:00:00 for no practice or warmup.
        /// </summary>
        public TimeSpan PracticeLength { get; set; }

        /// <summary>
        /// Length of the attached qualifying. Set to 0:00:00 for no attached qualy.
        /// </summary>
        public TimeSpan QualyLength { get; set; }

        /// <summary>
        /// Length of the race. If length is not time limited - set to 0:00:00
        /// </summary>
        public TimeSpan RaceLength { get; set; }

        /// <summary>
        /// Session id from iracing.com service
        /// </summary>
        public string IrSessionId { get; set; }

        /// <summary>
        /// Link to the iracing.com results page.
        /// </summary>
        public string IrResultLink { get; set; }
        
        /// <summary>
        /// Check if session has attached qualifying
        /// </summary>
        public bool QualyAttached { get; set; }

        /// <summary>
        /// Check if session has attached free-practice or warmup
        /// </summary>
        public bool PracticeAttached { get; set; }

        /// <summary>
        /// Create a new RaceSession object.
        /// </summary>
        public RaceSessionEntity()
        {
            //SessionType = SessionType.Race;
        }
    }
}
