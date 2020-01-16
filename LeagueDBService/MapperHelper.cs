using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iRLeagueDatabase;
using iRLeagueDatabase.DataTransfer;
using iRLeagueDatabase.DataTransfer.Members;
using iRLeagueDatabase.DataTransfer.Sessions;
using iRLeagueDatabase.DataTransfer.Reviews;
using iRLeagueDatabase.DataTransfer.Results;
using iRLeagueDatabase.Entities;
using iRLeagueDatabase.Entities.Sessions;
using iRLeagueDatabase.Entities.Results;
using iRLeagueDatabase.Entities.Members;
using iRLeagueDatabase.Entities.Reviews;
using AutoMapper;

namespace LeagueDBService
{
    public static class MapperHelper
    {
        public static IMappingExpression<TSource, TDestination> MapOnlyIfChanged<TSource, TDestination>(this IMappingExpression<TSource, TDestination> map)
        {
            map.ForAllMembers(source =>
            {
                source.Condition((sourceObject, destObject, sourceProperty, destProperty) =>
                {
                    if (sourceProperty == null)
                        return !(destProperty == null);
                    return !sourceProperty.Equals(destProperty);
                });
            });
            return map;
        }

        public static IMappingExpression<TSource, TDestination> IgnoreNestedRevisioEntites<TSource, TDestination>(this IMappingExpression<TSource, TDestination> map)
        {
            map.ForAllMembers(source =>
            {
                source.Condition((sourceObject, destObject, sourceProperty, destProperty) =>
                {
                    if (sourceProperty is VersionDTO || destProperty is VersionDTO ||
                        sourceProperty is Revision || destProperty is Revision)
                        return false;
                    return true;
                });
            });
           return map;
        }

        public static IMapper GetEntityMapper(LeagueDbContext dbContext)
        {
            var config = new MapperConfiguration(cfg =>
            {
            cfg.CreateMap<IncidentReviewDataDTO, IncidentReview>()
                .MapOnlyIfChanged();
            //.ForMember(dest => dest.InvolvedMembers, opt => opt.Ignore());
            cfg.CreateMap<LeagueMemberDataDTO, LeagueMember>()
                .ConstructUsing(source => GetMapping(source, dbContext.Members))
                .MapOnlyIfChanged();
            cfg.CreateMap<SeasonDataDTO, Season>()
                .ConstructUsing(source => GetMapping(source, dbContext.Seasons))
                .MapOnlyIfChanged();
            cfg.CreateMap<CommentDataDTO, CommentBase>()
                .ConstructUsing(source => GetMapping(source, dbContext.Comments))
                .MapOnlyIfChanged();
            cfg.CreateMap<ResultDataDTO, Result>()
                .ConstructUsing(source => GetMapping(source, dbContext.Results))
                .MapOnlyIfChanged();
            cfg.CreateMap<ScoringDataDTO, Scoring>()
                .ConstructUsing(source => GetMapping(source, dbContext.Scorings))
                .MapOnlyIfChanged();
            cfg.CreateMap<ResultRowDataDTO, ResultRow>()
                .ConstructUsing(source => GetMapping(source, dbContext.ResultRows))
                .MapOnlyIfChanged();
            cfg.CreateMap<ReviewCommentDataDTO, ReviewComment>()
                .ConstructUsing(source => GetMapping(source, dbContext.ReviewComments))
                .MapOnlyIfChanged();
            cfg.CreateMap<SessionDataDTO, SessionBase>()
                .ConstructUsing(source => GetMapping(source, dbContext.Sessions))
                .MapOnlyIfChanged();
            cfg.CreateMap<SessionDataDTO, RaceSession>()
                .ConstructUsing(source => GetMapping(source, dbContext.RaceSessions))
                .MapOnlyIfChanged();
            cfg.CreateMap<ScheduleDataDTO, Schedule>()
                .ConstructUsing(source => GetMapping(source, dbContext.Schedules))
                .MapOnlyIfChanged();

            cfg.CreateMap<LeagueMemberInfoDTO, LeagueMember>().ConvertUsing((source, dest) => GetMapping(source, dest, dbContext.Members));
            cfg.CreateMap<IncidentReviewInfoDTO, IncidentReview>().ConvertUsing((source, dest) => GetMapping(source, dest, dbContext.Reviews));
            cfg.CreateMap<SeasonInfoDTO, Season>().ConvertUsing((source, dest) => GetMapping(source, dest, dbContext.Seasons));
            cfg.CreateMap<CommentInfoDTO, CommentBase>().ConvertUsing((source, dest) => GetMapping(source, dest, dbContext.Comments));
            cfg.CreateMap<ResultInfoDTO, Result>().ConvertUsing((source, dest) => GetMapping(source, dest, dbContext.Results));
            cfg.CreateMap<ScoringInfoDTO, Scoring>().ConvertUsing((source, dest) => GetMapping(source, dest, dbContext.Scorings));
            cfg.CreateMap<SessionInfoDTO, SessionBase>().ConvertUsing((source, dest) => GetMapping(source, dest, dbContext.Sessions));
            cfg.CreateMap<SessionInfoDTO, RaceSession>().ConvertUsing((source, dest) => GetMapping(source, dest, dbContext.RaceSessions));
            cfg.CreateMap<ScheduleInfoDTO, Schedule>().ConvertUsing((source, dest) => GetMapping(source, dest, dbContext.Schedules));
            });
            

            return config.CreateMapper();
        }

        private static TDestination GetMapping<TSource, TDestination>(TSource source, System.Data.Entity.DbSet<TDestination> destDbSet) where TDestination : MappableEntity, new() where TSource : IMappableDTO
        {
            //return GetMapping<TSource, TDestination, TDestination>(source, destDbSet);
            TDestination dest = null;

            if (source?.MappingId != null)
            {
                dest = destDbSet.Find(source.MappingId);
            }

            if (dest == null)
            {
                dest = new TDestination();
            }

            return dest;
        }

        //private static TDestination GetMapping<TSource, TDestination, TDbSet>(TSource source, System.Data.Entity.DbSet<TDbSet> destDbSet) where TDestination : MappableEntity, new() where TSource : MappableDTO where TDbSet : MappableEntity
        //{
        //    TDestination dest = null;

        //    if (source?.MappingId != null)
        //    {
        //        dest = destDbSet.Find(source.MappingId) as TDestination;
        //    }

        //    if (dest == null)
        //    {
        //        dest = new TDestination();
        //    }

        //    return dest;
        //}

        private static TDestination GetMapping<TSource, TDestination>(TSource source, TDestination dest, System.Data.Entity.DbSet<TDestination> destDbSet) where TDestination : MappableEntity where TSource : IMappableDTO
        {
            if (dest == null || source.MappingId != dest.MappingId)
            {
                if (source?.MappingId != null)
                {
                    dest = destDbSet.Find(source.MappingId);
                    return dest;
                }
                else
                    return null;
            }
            return dest;
        }

        //private static TDestination GetMapping<TSource, TDestination, TDbSet>(TSource source, TDestination dest, System.Data.Entity.DbSet<TDbSet> destDbSet) where TDestination : MappableEntity, new() where TSource : MappableDTO where TDbSet : MappableEntity
        //{
        //    if (dest == null || source.MappingId != dest.MappingId)
        //    {
        //        if (source?.MappingId != null)
        //        {
        //            dest = destDbSet.Find(source.MappingId) as TDestination;
        //            return dest;
        //        }
        //        else
        //            return null;
        //    }
        //    return dest;
        //}
    }
}
