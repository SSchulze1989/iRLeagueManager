using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using iRLeagueDatabase.Entities.Members;

namespace iRLeagueDatabase.Entities
{
    public class Revision : MappableEntity
    {
        public DateTime? CreatedOn { get; set; } = null;
        public DateTime? LastModifiedOn { get; set; } = null;

        public int Version { get; set; }

        public LeagueMemberEntity CreatedBy { get; set; }
        public LeagueMemberEntity LastModifiedBy { get; set; }
    } 
}