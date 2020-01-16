using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace iRLeagueDatabase.Entities.Sessions
{
    public class ScheduleEntity : Revision
    {
        [Key]
        public int ScheduleId { get; set; }

        public string Name { get; set; }

        public override object MappingId => ScheduleId;

        public SeasonEntity Season { get; set; }

        public DateTime? ScheduleStart => Sessions.Select(x => x.Date).Min();

        public DateTime? ScheduleEnd => Sessions.Select(x => x.Date).Max();

        //[InverseProperty(nameof(SessionBaseEntity.Schedule))]
        public virtual List<SessionBaseEntity> Sessions { get; set; } = new List<SessionBaseEntity>();

        public ScheduleEntity() { }

        public ScheduleEntity(int scheduleId) : this()
        {
            ScheduleId = scheduleId;
        }
    }
}
