using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace iRLeagueDatabase.Entities
{
    //[NotMapped]
    public abstract class MappableEntity
    {
        [NotMapped]
        public virtual object MappingId { get; } = null;
    }
}
