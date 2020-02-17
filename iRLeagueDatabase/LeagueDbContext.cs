using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Core;
using System.Data.Entity.Core.Objects;

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

        private readonly OrphansToHandle OrphansToHandle;

        public LeagueDbContext() : this("Data Source=" + Environment.MachineName + "\\IRLEAGUEDB;Initial Catalog=LeagueDatabase;Integrated Security=True;Pooling=False")
        {
            //Database.SetInitializer(new MigrateDatabaseToLatestVersion<LeagueDbContext, iRLeagueDatabase.Migrations.Configuration>());
        }

        public LeagueDbContext(string nameOrConnectionString) : base(nameOrConnectionString)
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<LeagueDbContext, iRLeagueDatabase.Migrations.Configuration>());
            OrphansToHandle = new OrphansToHandle();
            OrphansToHandle.Add<SessionBaseEntity, ScheduleEntity>();
            OrphansToHandle.Add<RaceSessionEntity, ScheduleEntity>();
            OrphansToHandle.Add<ResultEntity, SessionBaseEntity>();
            OrphansToHandle.Add<ResultEntity, RaceSessionEntity>();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SeasonEntity>()
                .HasMany(r => r.Schedules)
                .WithRequired(r => r.Season);
            //modelBuilder.Entity<SeasonEntity>()
            //    .HasMany(r => r.Results)
            //    .WithRequired(r => r.Season);

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
                .WithRequired(r => r.Session);
            modelBuilder.Entity<SessionBaseEntity>()
                .HasRequired(r => r.Schedule)
                .WithMany(r => r.Sessions)
                .WillCascadeOnDelete();
        }

        public override int SaveChanges()
        {
            HandleOrphans();
            return base.SaveChanges();
        }

        private void HandleOrphans()
        {
            var objectContext = ((IObjectContextAdapter)this).ObjectContext;

            objectContext.DetectChanges();

            var deletedThings = objectContext.ObjectStateManager.GetObjectStateEntries(EntityState.Deleted).ToList();

            foreach (var deletedThing in deletedThings)
            {
                if (deletedThing.IsRelationship)
                {
                    var entityToDelete = IdentifyEntityToDelete(objectContext, deletedThing);

                    if (entityToDelete != null)
                    {
                        objectContext.DeleteObject(entityToDelete);
                    }
                }
            }
        }

        private object IdentifyEntityToDelete(ObjectContext objectContext, ObjectStateEntry deletedThing)
        {
            // The order is not guaranteed, we have to find which one has to be deleted
            var entityKeyOne = objectContext.GetObjectByKey((EntityKey)deletedThing.OriginalValues[0]);
            var entityKeyTwo = objectContext.GetObjectByKey((EntityKey)deletedThing.OriginalValues[1]);

            foreach (var item in OrphansToHandle.List)
            {
                if (IsInstanceOf(entityKeyOne, item.ChildToDelete) && IsInstanceOf(entityKeyTwo, item.Parent))
                {
                    return entityKeyOne;
                }
                if (IsInstanceOf(entityKeyOne, item.Parent) && IsInstanceOf(entityKeyTwo, item.ChildToDelete))
                {
                    return entityKeyTwo;
                }
            }

            return null;
        }

        private bool IsInstanceOf(object obj, Type type)
        {
            // Sometimes it's a plain class, sometimes it's a DynamicProxy, we check for both.
            return
                type == obj.GetType() ||
                (
                    obj.GetType().Namespace == "System.Data.Entity.DynamicProxies" &&
                    type == obj.GetType().BaseType
                );
        }
    }

    public class OrphansToHandle
    {
        public IList<EntityPairDto> List { get; private set; }

        public OrphansToHandle()
        {
            List = new List<EntityPairDto>();
        }

        public void Add<TChildObjectToDelete, TParentObject>()
        {
            List.Add(new EntityPairDto() { ChildToDelete = typeof(TChildObjectToDelete), Parent = typeof(TParentObject) });
        }
    }

    public class EntityPairDto
    {
        public Type ChildToDelete { get; set; }
        public Type Parent { get; set; }
    }
}
