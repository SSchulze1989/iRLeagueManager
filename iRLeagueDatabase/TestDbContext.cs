using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace iRLeagueDatabase
{
    public class TestDbContext : DbContext
    {
        public DbSet<Session> Sessions { get; set; }

        public TestDbContext() : base ("Data Source=" + Environment.MachineName + "\\IRLEAGUEDB;Initial Catalog=TestDb;Integrated Security=True;Pooling=False") { }
    }
}
