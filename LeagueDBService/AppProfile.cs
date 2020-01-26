using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using iRLeagueDatabase;
using AutoMapper;
using AutoMapper.Collection;
using AutoMapper.EntityFramework;
using AutoMapper.EquivalencyExpression;
using iRLeagueDatabase.Entities;
using iRLeagueDatabase.Entities.Members;
using iRLeagueDatabase.Entities.Results;
using iRLeagueDatabase.Entities.Reviews;
using iRLeagueDatabase.Entities.Sessions;
using iRLeagueDatabase.DataTransfer;
using iRLeagueDatabase.DataTransfer.Members;
using iRLeagueDatabase.DataTransfer.Results;
using iRLeagueDatabase.DataTransfer.Reviews;
using iRLeagueDatabase.DataTransfer.Sessions;

namespace LeagueDBService
{
    public class AppProfile : Profile
    {
        public LeagueDbContext DbContext { get; set; }

        public AppProfile()
        {
            CreateMap<LeagueMemberEntity, LeagueMemberDataDTO>();
            CreateMap<LeagueMemberEntity, LeagueMemberInfoDTO>();
            //CreateMap<LeagueMemberDataDTO, LeagueMember>()
            //    .MapOnlyIfChanged();

            CreateMap<SeasonEntity, SeasonDataDTO>();
            CreateMap<SeasonEntity, SeasonInfoDTO>();
            //CreateMap<SeasonDataDTO, Season>()
            //    .MapOnlyIfChanged();

            CreateMap<ScheduleEntity, ScheduleDataDTO>();
            CreateMap<ScheduleEntity, ScheduleInfoDTO>();

            CreateMap<SessionBaseEntity, SessionDataDTO>()
                .Include<RaceSessionEntity, RaceSessionDataDTO>();
            CreateMap<SessionBaseEntity, SessionInfoDTO>();
            //CreateMap<SeasonDataDTO, SessionBase>()
            //    .MapOnlyIfChanged();

            CreateMap<RaceSessionEntity, RaceSessionDataDTO>();
            CreateMap<RaceSessionEntity, SessionInfoDTO>();
            //CreateMap<RaceSessionDataDTO, RaceSession>()
            //    .MapOnlyIfChanged();

            CreateMap<ResultEntity, ResultDataDTO>();
            CreateMap<ResultEntity, ResultInfoDTO>();
            //CreateMap<ResultDataDTO, Result>()
            //    .MapOnlyIfChanged();

            CreateMap<ResultRowEntity, ResultRowDataDTO>();

            CreateMap<IncidentReviewEntity, IncidentReviewDataDTO>();
            CreateMap<IncidentReviewEntity, IncidentReviewInfoDTO>();
            //CreateMap<IncidentReviewDataDTO, IncidentReview>()
            //    .MapOnlyIfChanged();
                //.IgnoreNestedRevisioEntites();

            CreateMap<CommentBaseEntity, CommentDataDTO>();
            CreateMap<CommentBaseEntity, CommentInfoDTO>();
            //CreateMap<CommentDataDTO, CommentBase>()
            //    .MapOnlyIfChanged();
                //.IgnoreNestedRevisioEntites();

            CreateMap<ReviewCommentEntity, ReviewCommentDataDTO>();
            CreateMap<ReviewCommentEntity, CommentInfoDTO>();
            //CreateMap<ReviewCommentDataDTO, ReviewComment>()
            //    .MapOnlyIfChanged();
            //.IgnoreNestedRevisioEntites();

            //CreateMap<IncidentReviewDataDTO, IncidentReview>()
            //    .MapOnlyIfChanged();
            //.ForMember(dest => dest.InvolvedMembers, opt => opt.Ignore());
            CreateMap<LeagueMemberDataDTO, LeagueMemberEntity>()
                .ConstructUsing(source => GetMapping(source, DbContext.Members))
                .EqualityComparison((src, dest) => src.MemberId == dest.MemberId)
                .MapOnlyIfChanged();
            CreateMap<SeasonDataDTO, SeasonEntity>()
                .ConstructUsing(source => GetMapping(source, DbContext.Seasons))
                .EqualityComparison((src, dest) => src.SeasonId == dest.SeasonId)
                .MapOnlyIfChanged();
            CreateMap<CommentDataDTO, CommentBaseEntity>()
                .ConstructUsing(source => GetMapping(source, DbContext.Set<CommentBaseEntity>()))
                .EqualityComparison((src, dest) => src.CommentId == dest.CommentId)
                .MapOnlyIfChanged();
            CreateMap<ResultDataDTO, ResultEntity>()
                .ConstructUsing(source => GetMapping(source, DbContext.Set<ResultEntity>()))
                .EqualityComparison((src, dest) => src.ResultId == dest.ResultId)
                .MapOnlyIfChanged()
                .AfterMap((src, dest) =>
                {
                    //if (dest.Session.SessionResult != dest)
                    //{
                    //    dest.Session.SessionResult = dest;
                    //}
                });
            CreateMap<ScoringDataDTO, ScoringEntity>()
                .ConstructUsing(source => GetMapping(source, DbContext.Set<ScoringEntity>()))
                .EqualityComparison((src, dest) => src.ScoringId == dest.ScoringId)
                .MapOnlyIfChanged();
            CreateMap<ResultRowDataDTO, ResultRowEntity>()
                //.ConstructUsing(source => GetMapping(source, DbContext.ResultRows))
                .EqualityComparison((src, dest) => src.ResultRowId == dest.ResultRowId)
                .MapOnlyIfChanged();
            CreateMap<IncidentReviewDataDTO, IncidentReviewEntity>()
                .EqualityComparison((src, dest) => src.ReviewId == dest.ReviewId)
                .ConstructUsing(source => GetMapping(source, DbContext.Set<IncidentReviewEntity>()))
                .MapOnlyIfChanged();
            CreateMap<ReviewCommentDataDTO, ReviewCommentEntity>()
                .ConstructUsing(source => GetMapping(source, DbContext.Set<ReviewCommentEntity>()))
                .EqualityComparison((src, dest) => src.CommentId == dest.CommentId)
                .MapOnlyIfChanged();
            CreateMap<RaceSessionDataDTO, RaceSessionEntity>()
                .ConstructUsing(source => GetMapping(source, DbContext.Set<RaceSessionEntity>()))
                .EqualityComparison((src, dest) => src.SessionId == dest.SessionId)
                .ForMember(dest => dest.Schedule, opt => opt.Ignore())
                //.ConstructUsing(source => GetMapping(source, DbContext.Schedules.Find(source.Schedule.ScheduleId).Sessions.Where(x => x is RaceSession).Cast<RaceSession>()))
                //.ForMember(x => x.SessionResult, map => map.Condition(src => (src.SessionResult != null && src.SessionResult?.ResultId != 0)))
                .MapOnlyIfChanged();
            CreateMap<SessionDataDTO, SessionBaseEntity>()
                .ConstructUsing(source => GetMapping(source, DbContext.Set<SessionBaseEntity>()))
                //.ConstructUsing(source => GetMapping(source, DbContext.Schedules.Find(source.Schedule.ScheduleId).Sessions))
                .EqualityComparison((src, dest) => src.SessionId == dest.SessionId)
                .MapOnlyIfChanged()
                .ForMember(dest => dest.Schedule, opt => opt.Ignore())
                //.Include<RaceSessionDataDTO, RaceSessionEntity>();
                .IncludeAllDerived();
            CreateMap<ScheduleDataDTO, ScheduleEntity>()
                .ConstructUsing(source => GetMapping(source, DbContext.Set<ScheduleEntity>()))
                .EqualityComparison((src, dest) => src.ScheduleId == dest.ScheduleId)
                .ForMember(dest => dest.Season, opt => opt.Ignore())
                .MapOnlyIfChanged();

            CreateMap<LeagueMemberInfoDTO, LeagueMemberEntity>()
                .IncludeAllDerived()
                .ConvertUsing((source, dest) => GetMapping(source, dest, DbContext.Members));
            CreateMap<IncidentReviewInfoDTO, IncidentReviewEntity>()
                .IncludeAllDerived()
                .ConvertUsing((source, dest) => GetMapping(source, dest, DbContext.Set<IncidentReviewEntity>()));
            CreateMap<SeasonInfoDTO, SeasonEntity>()
                .IncludeAllDerived()
                .ConvertUsing((source, dest) => GetMapping(source, dest, DbContext.Seasons));
            CreateMap<CommentInfoDTO, CommentBaseEntity>()
                .IncludeAllDerived()
                .ConvertUsing((source, dest) => GetMapping(source, dest, DbContext.Set<CommentBaseEntity>()));
            CreateMap<ResultInfoDTO, ResultEntity>()
                .IncludeAllDerived()
                .ConvertUsing((source, dest) => GetMapping(source, dest, DbContext.Set<ResultEntity>()));
            CreateMap<ScoringInfoDTO, ScoringEntity>()
                .IncludeAllDerived()
                .ConvertUsing((source, dest) => GetMapping(source, dest, DbContext.Set<ScoringEntity>()));
            CreateMap<SessionInfoDTO, SessionBaseEntity>()
                .IncludeAllDerived()
                .ConvertUsing((source, dest) => GetMapping(source, dest, DbContext.Set<SessionBaseEntity>())); //dest.Schedule.Sessions));
            CreateMap<SessionInfoDTO, RaceSessionEntity>()
                .IncludeAllDerived()
                .ConvertUsing((source, dest) => GetMapping(source, dest, DbContext.Set<RaceSessionEntity>())); //dest.Schedule.Sessions.Where(x => x is RaceSession).Cast<RaceSession>()));
            CreateMap<ScheduleInfoDTO, ScheduleEntity>()
                .IncludeAllDerived()
                .EqualityComparison((dest, src) => dest.ScheduleId == src.ScheduleId)
                .ConvertUsing((source, dest) => GetMapping(source, dest, DbContext.Set<ScheduleEntity>()));

            CreateMap<long, TimeSpan>()
                .ConvertUsing<TimeSpanConverter>();
            CreateMap<TimeSpan, long>()
                .ConvertUsing<TimeSpanConverter>();
        }

        private TDestination GetMapping<TSource, TDestination>(TSource source, System.Data.Entity.DbSet<TDestination> destDbSet) where TDestination : MappableEntity, new() where TSource : IMappableDTO
        //private static TDestination GetMapping<TSource, TDestination>(TSource source, IEnumerable<TDestination> destDbSet) where TDestination : MappableEntity, new() where TSource : IMappableDTO
        {
            //return GetMapping<TSource, TDestination, TDestination>(source, destDbSet);
            TDestination dest = null;

            if (source?.MappingId != null)
            {
                //dest = destDbSet.SingleOrDefault(x => x.MappingId == source.MappingId);
                dest = destDbSet.Find(source.MappingId);
            }

            if (dest == null)
            {
                dest = new TDestination();
                //DbContext.SaveChanges();
                //destDbSet.Add(dest);
            }

            return dest;
        }

        private TDestination GetMapping<TSource, TDestination>(TSource source, TDestination dest, System.Data.Entity.DbSet<TDestination> destDbSet) where TDestination : MappableEntity where TSource : IMappableDTO
            //private static TDestination GetMapping<TSource, TDestination>(TSource source, TDestination dest, IEnumerable<TDestination> destDbSet) where TDestination : MappableEntity where TSource : IMappableDTO
        {
            if (dest == null || source.MappingId != dest.MappingId)
            {
                if (source?.MappingId != null)
                {
                    //dest = destDbSet.SingleOrDefault(x => x.MappingId == source.MappingId);
                    dest = destDbSet.Find(source.MappingId);
                    return dest;
                }
                else
                    return null;
            }
            return dest;
        }
    }

    public class TimeSpanConverter : ITypeConverter<long, TimeSpan>, ITypeConverter<TimeSpan, long>
    {
        TimeSpan ITypeConverter<long, TimeSpan>.Convert(long source, TimeSpan destination, ResolutionContext context)
        {
            return TimeSpan.FromTicks(source);
        }

        long ITypeConverter<TimeSpan, long>.Convert(TimeSpan source, long destination, ResolutionContext context)
        {
            return source.Ticks;
        }
    }

    public static class MappingExpressions
    {
        public static IMappingExpression<TSource, TDestination> MapOnlyIfChanged<TSource, TDestination>(this IMappingExpression<TSource, TDestination> map)
        {
            map.ForAllMembers(source =>
            {
                source.Condition((sourceObject, destObject, sourceProperty, destProperty) =>
                {
                    if (destObject == null)
                        return true;

                    if (sourceProperty == null)
                        return !(destProperty == null);

                    if (!sourceProperty.Equals(destProperty) || sourceProperty is object)
                    {
                        if (sourceObject is VersionInfoDTO srcVersion && destObject is Revision destVersion)
                        {
                            if (destVersion.LastModifiedOn == null || srcVersion.LastModifiedOn > destVersion.LastModifiedOn)
                            {
                                return true;
                            }

                            if (sourceProperty is object)
                            {
                                if (sourceProperty is IMappableDTO srcMap && destProperty is MappableEntity destMap)
                                {
                                    return (destMap.MappingId == srcMap.MappingId) ? true : false;
                                }
                            }

                            return false;
                        }

                        return true;
                    }

                    return false;
                    //return !sourceProperty.Equals(destProperty);
                });
            });
            return map;
        }
    }
}
