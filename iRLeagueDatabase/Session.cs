using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace iRLeagueDatabase
{
    public class Session
    {
        [Key]
        public int SessionId { get; set; }

        public virtual Result Result { get; set; }
    }

    public class Result
    {
        [Key, ForeignKey(nameof(Session))]
        public int ResultId { get; set; }

        public virtual Session Session { get; set; }
    }
}
