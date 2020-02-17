using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using iRLeagueDatabase.Entities.Sessions;
using iRLeagueDatabase.Entities.Results;
using iRLeagueDatabase.Entities.Reviews;

namespace iRLeagueDatabase.Entities
{
    public class SeasonEntity : Revision
    {   
        [Key]
        public int SeasonId { get; set; }

        public override object MappingId => SeasonId;

        public string SeasonName { get; set; }
        
        public virtual List<ScheduleEntity> Schedules { get; set; }

        //public virtual List<ScoringEntity> Scorings { get; set; }

        //public virtual List<IncidentReviewEntity> Reviews { get; set; }

        [NotMapped]
        public virtual IEnumerable<ResultEntity> Results => null; /*Schedules.Select(x => x.Sessions.Select(y => y.SessionResult)).Aggregate((x, y) => x.Concat(y));*/

        //[ForeignKey(nameof(MainScoring))]
        //public int? MainScoringId { get; set; }
        public ScoringEntity MainScoring { get; set; }

        [NotMapped]
        public DateTime? SeasonStart => Schedules.Select(x => x.ScheduleStart).Min();
        [NotMapped]
        public DateTime? SeasonEnd => Schedules.Select(x => x.ScheduleEnd)?.Max();

        public SeasonEntity()
        {
            Schedules = new List<ScheduleEntity>();
            //Scorings = new List<ScoringEntity>();
            //Reviews = new List<IncidentReviewEntity>();
        }
    }
}
