using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.ComponentModel;
using System.Xml.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using iRLeagueDatabase.Entities.Sessions;
using iRLeagueDatabase.Entities.Reviews;


namespace iRLeagueDatabase.Entities.Results
{
    public class ResultEntity : Revision
    {
        //[Key, ForeignKey(nameof(Session)), Column(Order = 1)]
        //[Key, Column(Order = 1)]
        //public int ResultId { get; set; }

        //[ForeignKey(nameof(Session))]
        //public int SessionId { get; set; }

        //public virtual SeasonEntity Season { get; set; }

        [Key, ForeignKey(nameof(Session))]
        public virtual int ResultId { get; set; }
        [Required]
        public virtual SessionBaseEntity Session { get; set; }

        //public int Test { get; set; }

        public override object MappingId => ResultId;

        /// <summary>
        /// Data rows of results table
        /// </summary>
        [InverseProperty(nameof(ResultRowEntity.Result))]
        public virtual List<ResultRowEntity> RawResults { get; set; }

        public virtual List<IncidentReviewEntity> Reviews { get; set; } = new List<IncidentReviewEntity>();
 
        //public List<ResultRow> FinalResults { get; set; }

        public ResultEntity()
        {
            RawResults = new List<ResultRowEntity>();
            //FinalResults = new List<ResultRow>();
        }
    }
}
