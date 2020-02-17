using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using iRLeagueDatabase.Entities.Results;
using iRLeagueManager.Enums;
using iRLeagueDatabase.Entities.Reviews;

namespace iRLeagueDatabase.Entities.Sessions
{
    /// <summary>
    /// Base type for league sessions.
    /// </summary>
    public class SessionBaseEntity : Revision
    {
        //[Key]
        //[ForeignKey(nameof(Schedule))]
        //public int? ScheduleId { get; set; }
        //[Key, ForeignKey(nameof(Schedule)), Column(Order = 2)]
        //public int ScheduleId { get; set; }
        [Required]
        public virtual ScheduleEntity Schedule { get; set; }

        public override object MappingId => SessionId;

        /// <summary>
        /// Unique Session Id for League Session
        /// </summary>
        [Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SessionId { get; set; }

        public string SessionTitle { get; set; }
        
        /// <summary>
        /// Type of this session. (Practice, Qualifying or Race)
        /// </summary>
        //[XmlIgnore]
        public SessionType SessionType { get; set; }
        
        /// <summary>
        /// Date of the session.
        /// </summary>
        public DateTime? Date { get; set; }

        /// <summary>
        /// Id of the track and track-config for the session.
        /// </summary>
        public string LocationId { get; set; }
        
        /// <summary>
        /// Duration of the session. In case of a race with attached qualy, this also includes the times of free practice and qualifiying.
        /// </summary>
        public TimeSpan Duration { get; set; }

        /// <summary>
        /// Result of the session
        /// </summary>
        public virtual ResultEntity SessionResult { get; set; }

        //[InverseProperty(nameof(IncidentReview.Session))]
        //public virtual List<IncidentReview> Reviews { get; set; }

        //public XmlTimeSpan Duration { get; set; }C:\Users\simon\Documents\VisualStudio\DatabaseTest\DatabaseTest\Entities\Sessions\SessionBase.cs

        /// <summary>
        /// Create a new Session object
        /// </summary>
        public SessionBaseEntity()
        {
            SessionType = SessionType.Undefined;
            Date = DateTime.Now;
        }
    }
}
