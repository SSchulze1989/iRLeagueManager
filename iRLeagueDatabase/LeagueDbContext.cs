using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

using iRLeagueDatabase.Entities;
using iRLeagueDatabase.Entities.Members;
using iRLeagueDatabase.Entities.Results;
using iRLeagueDatabase.Entities.Reviews;
using iRLeagueDatabase.Entities.Sessions;

namespace iRLeagueDatabase
{
    public class LeagueDbContext : DbContext
    {
        public virtual DbSet<SeasonEntity> Seasons { get; set; }
        public virtual DbSet<LeagueMemberEntity> Members { get; set; }

        public LeagueDbContext() : base("Data Source=" + Environment.MachineName 
            + "\\IRLEAGUEDB;Initial Catalog=LeagueDatabase;Integrated Security=True;Pooling=False")
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<LeagueDbContext, iRLeagueDatabase.Migrations.Configuration>());
        }

        public LeagueDbContext(string nameOrConnectionString) : base(nameOrConnectionString) { }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SeasonEntity>()
                .HasMany(r => r.Schedules)
                .WithRequired(r => r.Season);
            modelBuilder.Entity<SeasonEntity>()
                .HasMany(r => r.Results)
                .WithRequired(r => r.Season);

            modelBuilder.Entity<IncidentReviewEntity>()
                .HasMany(r => r.InvolvedMembers)
                .WithMany()
                .Map(rm =>
                {
                    rm.MapLeftKey("ReviewRefId");
                    rm.MapRightKey("MemberRefId");
                    rm.ToTable("IncidentReview_LeagueMember");
                });
            modelBuilder.Entity<IncidentReviewEntity>()
                .HasOptional(r => r.MemberAtFault);
                //.WithMany(r => r.Reviews);
            modelBuilder.Entity<IncidentReviewEntity>()
                .HasRequired(r => r.Result)
                .WithMany(r => r.Reviews);

            modelBuilder.Entity<SessionBaseEntity>()
                .HasOptional(r => r.SessionResult)
                .WithOptionalDependent(r => r.Session);
            modelBuilder.Entity<SessionBaseEntity>()
                .HasRequired(r => r.Schedule)
                .WithMany(r => r.Sessions)
                .WillCascadeOnDelete();
        }
    }
}
