using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using iRLeagueDatabase;
using iRLeagueDatabase.DataTransfer;
using iRLeagueDatabase.DataTransfer.Members;
using iRLeagueDatabase.DataTransfer.Sessions;
using iRLeagueDatabase.DataTransfer.Reviews;
using iRLeagueDatabase.DataTransfer.Results;
using iRLeagueDatabase.Entities;
using iRLeagueDatabase.Entities.Members;
using iRLeagueDatabase.Entities.Reviews;
using iRLeagueDatabase.Entities.Sessions;
using iRLeagueDatabase.Entities.Results;
using System.Data.Entity;
using AutoMapper;
using AutoMapper.Collection;
using AutoMapper.EquivalencyExpression;

namespace LeagueDBService
{
    // HINWEIS: Mit dem Befehl "Umbenennen" im Menü "Umgestalten" können Sie den Klassennamen "Service1" sowohl im Code als auch in der Konfigurationsdatei ändern.
    [ServiceBehavior(IncludeExceptionDetailInFaults = true)]
    public class LeagueDBService : ILeagueDBService
    {
        AppProfile MapperProfile { get; }

        MapperConfiguration MapperConfiguration { get; set; }

        public LeagueDBService()
        {
            MapperProfile = new AppProfile();
            MapperConfiguration = new MapperConfiguration(cfg =>
            {
                cfg.AddCollectionMappers();
                cfg.AddProfile(MapperProfile);
            });
        }

        public void CleanUpSessions()
        {
            //using (var leagueDb = new LeagueDbContext())
            //{
            //    var Sessions = leagueDb.Sessions.ToArray();
            //    foreach (var session in Sessions)
            //    {
            //        if (session.Schedule == null)
            //        {
            //            leagueDb.Sessions.Remove(session);
            //        }
            //    }
            //    leagueDb.SaveChanges();
            //}
        }

        public LeagueMemberDataDTO GetMember(int memberId)
        {
            LeagueMemberDataDTO leagueMember;
            using (var leagueDb = new LeagueDbContext())
            {
                var memberEntity = leagueDb.Members.SingleOrDefault(x => x.MemberId == memberId);
                if (memberEntity != null)
                {
                    var mapper = MapperConfiguration.CreateMapper();

                    leagueMember = mapper.Map<LeagueMemberDataDTO>(memberEntity);
                }
                else
                {
                    leagueMember = null;
                }
                return leagueMember;
            }
        }

        public List<SeasonDataDTO> GetSeasons(int[] seasonIds = null)
        {
            //IQueryable<Season> seasonEntities;
            IEnumerable<SeasonEntity> seasonEntities;
            List<SeasonDataDTO> seasonDTOs = new List<SeasonDataDTO>();

            var mapper = MapperConfiguration.CreateMapper();

            using (var leagueDb = new LeagueDbContext())
            {
                // Deprecated becaus Automapper ignores include statements with ProjectTo()
                //if (seasonIds == null || seasonIds == new int[0])
                //{
                //    seasonEntities = leagueDb.Seasons.AsQueryable();
                //}
                //else
                //{
                //    seasonEntities = leagueDb.Seasons.Where(x => seasonIds.Contains(x.SeasonId));
                //}

                //seasonDTOs = mapper.ProjectTo<SeasonDataDTO>(seasonEntities).ToList();

                if (seasonIds == null || seasonIds == new int[0])
                {
                    seasonEntities = leagueDb.Seasons.ToArray();
                }
                else
                {
                    seasonEntities = leagueDb.Seasons.Where(x => seasonIds.Contains(x.SeasonId));
                }

                foreach (var seasonEntity in seasonEntities)
                {
                    seasonDTOs.Add(mapper.Map<SeasonDataDTO>(seasonEntity));
                }
            }

            return seasonDTOs;
        }

        public SeasonDataDTO PutSeason(SeasonDataDTO seasonData)
        {
            if (seasonData == null)
                return null;

            SeasonDataDTO returnData;
            var mapper = MapperConfiguration.CreateMapper();

            using (LeagueDbContext leagueDb = new LeagueDbContext())
            {
                //var mapper = MapperHelper.GetEntityMapper(leagueDb);
                MapperProfile.DbContext = leagueDb;

                SeasonEntity seasonEntity;
                
                if (leagueDb.Seasons.Any(x => x.SeasonId == seasonData.SeasonId))
                {
                    seasonEntity = leagueDb.Seasons.Find(seasonData.SeasonId);
                    mapper.Map(seasonData, seasonEntity);
                }
                else
                {
                    seasonEntity = mapper.Map<SeasonEntity>(seasonData);
                    seasonEntity = leagueDb.Seasons.Add(seasonEntity);
                }
                leagueDb.SaveChanges();
                
                seasonEntity = leagueDb.Seasons.Find(seasonEntity.SeasonId);
                returnData = mapper.Map<SeasonDataDTO>(seasonEntity);
            }

            return returnData;
        }

        public List<LeagueMemberDataDTO> GetMembers(int[] memberIds = null)
        {
            IQueryable<LeagueMemberEntity> memberEntities;
            List<LeagueMemberDataDTO> memberDTOs;

            var mapper = MapperConfiguration.CreateMapper();

            using (var leagueDb = new LeagueDbContext())
            {
                if (memberIds == null || memberIds == new int[0])
                {
                    memberEntities = leagueDb.Members.AsQueryable();
                }
                else
                {
                    memberEntities = leagueDb.Members.Where(x => memberIds.Contains(x.MemberId));
                }

                memberDTOs = mapper.ProjectTo<LeagueMemberDataDTO>(memberEntities).ToList();
            }

            return memberDTOs;
        }

        public LeagueMemberDataDTO[] UpdateMemberList(LeagueMemberDataDTO[] members)
        {
            if (members == null || members.Count() == 0)
                return null;

            LeagueMemberDataDTO[] returnData;
            var mapper = MapperConfiguration.CreateMapper();

            using (LeagueDbContext leagueDb = new LeagueDbContext())
            {
                //var mapper = MapperHelper.GetEntityMapper(leagueDb);
                foreach (var memberData in members)
                {
                    MapperProfile.DbContext = leagueDb;

                    LeagueMemberEntity memberEntity;

                    //Put review to Db
                    if (leagueDb.Members.Any(x => x.MemberId == memberData.MemberId) && memberData.MemberId != 0)
                    {
                        memberEntity = leagueDb.Members.Find(memberData.MemberId);
                        mapper.Map(memberData, memberEntity);
                    }
                    else
                    {
                        memberEntity = mapper.Map<LeagueMemberEntity>(memberData);
                        leagueDb.Members.Add(memberEntity);
                    }
                }
                leagueDb.SaveChanges();

                //Get review object to return

                returnData = GetMembers().ToArray();
            }

            return returnData;
        }

        public LeagueMemberDataDTO GetLastMember()
        {
            int lastMemberId = 0;

            using (var leagueDb = new LeagueDbContext())
            {
                var member = leagueDb.Members.ToList().Last();
                lastMemberId = member.MemberId;
            }

            return GetMember(lastMemberId);
        }

        public LeagueMemberDataDTO PutMember(LeagueMemberDataDTO memberData)
        {
            if (memberData == null)
                return null;

            LeagueMemberDataDTO returnData;
            var mapper = MapperConfiguration.CreateMapper();

            using (LeagueDbContext leagueDb = new LeagueDbContext())
            {
                //var mapper = MapperHelper.GetEntityMapper(leagueDb);
                MapperProfile.DbContext = leagueDb;

                LeagueMemberEntity memberEntity;

                //Put review to Db
                if (leagueDb.Members.Any(x => x.MemberId == memberData.MemberId))
                {
                    memberEntity = leagueDb.Members.Find(memberData.MemberId);
                    mapper.Map(memberData, memberEntity);
                }
                else
                {
                    memberEntity = mapper.Map<LeagueMemberEntity>(memberData);
                    leagueDb.Members.Add(memberEntity);
                }
                leagueDb.SaveChanges();

                //Get review object to return
                memberEntity = leagueDb.Members.Find(memberData.MemberId);
                returnData = mapper.Map<LeagueMemberDataDTO>(memberEntity);
            }

            return returnData;
        }

        public SeasonDataDTO GetSeason(int seasonId)
        {
            var mapper = MapperConfiguration.CreateMapper();
            SeasonDataDTO season = null;
            using (var leagueDb = new LeagueDbContext())
            {
                var seasonEntity = leagueDb.Seasons.SingleOrDefault(x => x.SeasonId == seasonId);
                season = mapper.Map<SeasonDataDTO>(seasonEntity);
            }

            return season;
        }

        public IncidentReviewDataDTO GetReview(int reviewId)
        {
            var mapper = MapperConfiguration.CreateMapper();
            IncidentReviewDataDTO review = null;
            using (var leagueDb = new LeagueDbContext())
            {
                var reviewEntitiy = leagueDb.Set<IncidentReviewEntity>().Find(reviewId);
                review = mapper.Map<IncidentReviewDataDTO>(reviewEntitiy);
            }
            return review;
        }
        
        public IncidentReviewDataDTO PutReview(IncidentReviewDataDTO review)
        {
            if (review == null)
                return null;

            IncidentReviewDataDTO returnReview;
            var mapper = MapperConfiguration.CreateMapper();

            using (var leagueDb = new LeagueDbContext())
            {
                //var mapper = MapperHelper.GetEntityMapper(leagueDb);
                MapperProfile.DbContext = leagueDb;

                IncidentReviewEntity reviewEntity;
                var reviewSet = leagueDb.Set<IncidentReviewEntity>();

                //Put review to Db
                if (reviewSet.Any(x => x.ReviewId == review.ReviewId))
                {
                    reviewEntity = reviewSet.Find(review.ReviewId);
                    //var config = new MapperConfiguration(cfg => {
                    //    cfg.CreateMap<IncidentReviewDTO, IncidentReview>()
                    //        .ForMember(dest => dest.MemberAtFault, map => map.MapFrom((source, dest) => leagueDb.Members.Where(x => x.MemberId == source.MemberAtFaultId).FirstOrDefault()))
                    //        .MapOnlyIfChanged();
                    //});
                    mapper.Map(review, reviewEntity);
                }
                else
                {
                    reviewEntity = mapper.Map<IncidentReviewEntity>(review);
                    reviewSet.Add(reviewEntity);
                }
                leagueDb.SaveChanges();

                //Get review object to return
                reviewEntity = reviewSet.Find(review.ReviewId);
                returnReview = mapper.Map<IncidentReviewDataDTO>(reviewEntity);
            }

            return returnReview;
        }

        public CommentDataDTO GetComment(int commentId)
        {
            var mapper = MapperConfiguration.CreateMapper();
            CommentDataDTO comment = null;

            using (var leagueDb = new LeagueDbContext())
            {
                var commentSet = leagueDb.Set<CommentBaseEntity>();
                var commentEntity = commentSet.Where(x => x.CommentId == commentId).FirstOrDefault();

                if (commentEntity is ReviewCommentEntity)
                {
                    comment = mapper.Map<ReviewCommentDataDTO>(commentEntity);
                }
                else
                {
                    comment = mapper.Map<CommentDataDTO>(commentEntity);
                }
            }
            return comment;
        }

        public CommentDataDTO PutComment(ReviewCommentDataDTO comment)
        {
            if (comment == null)
                return null;

            CommentDataDTO returnComment;
            var mapper = MapperConfiguration.CreateMapper();

            using (var leagueDb = new LeagueDbContext())
            {
                //var mapper = MapperHelper.GetEntityMapper(leagueDb);
                MapperProfile.DbContext = leagueDb;

                CommentBaseEntity commentEntity;
                var commentSet = leagueDb.Set<CommentBaseEntity>();

                //Put review to Db
                if (commentSet.Any(x => x.CommentId == comment.CommentId))
                {
                    commentEntity = commentSet.Find(comment.CommentId);
                    mapper.Map(comment, commentEntity);
                }
                else
                {
                    commentEntity = mapper.Map<CommentBaseEntity>(comment);
                    commentSet.Add(commentEntity);
                }
                leagueDb.SaveChanges();

                //Get review object to return
                commentEntity = commentSet.Find(comment.CommentId);
                returnComment = mapper.Map<CommentDataDTO>(commentEntity);
            }

            return returnComment;
        }

        public SessionDataDTO GetSession(int sessionId)
        {
            SessionDataDTO sessionData;
            using (var leagueDb = new LeagueDbContext())
            {
                var memberEntity = leagueDb.Set<SessionBaseEntity>().Find(sessionId);
                if (memberEntity != null)
                {
                    var mapper = MapperConfiguration.CreateMapper();

                    sessionData = mapper.Map<SessionDataDTO>(memberEntity);
                }
                else
                {
                    sessionData = null;
                }
                return sessionData;
            }
        }

        public SessionDataDTO PutSession(SessionDataDTO sessionData)
        {
            SessionDataDTO returnData = null;
            var mapper = MapperConfiguration.CreateMapper();

            using (LeagueDbContext leagueDb = new LeagueDbContext())
            {
                //var mapper = MapperHelper.GetEntityMapper(leagueDb);
                MapperProfile.DbContext = leagueDb;

                SessionBaseEntity sessionEntity;
                var sessionSet = leagueDb.Set<SessionBaseEntity>();

                //Put review to Db
                if (sessionSet.Any(x => x.SessionId == sessionData.SessionId))
                {
                    sessionEntity = sessionSet.SingleOrDefault(x => x.SessionId == sessionData.SessionId);
                    mapper.Map(sessionData, sessionEntity);
                }
                else
                {
                    sessionEntity = mapper.Map<SessionBaseEntity>(sessionData);
                    sessionSet.Add(sessionEntity);
                }
                leagueDb.SaveChanges();

                //Get review object to return
                returnData = mapper.Map<SessionDataDTO>(sessionEntity);
            }

            return returnData;
        }

        public ScheduleDataDTO GetSchedule(int scheduleId)
        {
            ScheduleDataDTO scheduleData;
            using (var leagueDb = new LeagueDbContext())
            {
                var memberEntity = leagueDb.Set<ScheduleEntity>().Find(scheduleId);
                if (memberEntity != null)
                {
                    var mapper = MapperConfiguration.CreateMapper();

                    scheduleData = mapper.Map<ScheduleDataDTO>(memberEntity);
                }
                else
                {
                    scheduleData = null;
                }
                return scheduleData;
            }
        }

        public ScheduleDataDTO PutSchedule(ScheduleDataDTO scheduleData)
        {
            ScheduleDataDTO returnData = null;
            var mapper = MapperConfiguration.CreateMapper();

            using (LeagueDbContext leagueDb = new LeagueDbContext())
            {
                //var mapper = MapperHelper.GetEntityMapper(leagueDb);
                MapperProfile.DbContext = leagueDb;

                ScheduleEntity scheduleEntity;
                var scheduleSet = leagueDb.Set<ScheduleEntity>();

                //Put review to Db
                if (scheduleSet.Any(x => x.ScheduleId == scheduleData.ScheduleId))
                {
                    scheduleEntity = scheduleSet.Find(scheduleData.ScheduleId);
                    mapper.Map(scheduleData, scheduleEntity);
                }
                else
                {
                    scheduleEntity = mapper.Map<ScheduleEntity>(scheduleData);
                    scheduleSet.Add(scheduleEntity);
                }
                leagueDb.SaveChanges();

                //Get review object to return
                scheduleEntity = scheduleSet.Find(scheduleData.ScheduleId);
                returnData = mapper.Map<ScheduleDataDTO>(scheduleEntity);
            }

            return returnData;
        }

        public List<ScheduleDataDTO> GetSchedules(int[] scheduleIds = null)
        {
            IQueryable<ScheduleEntity> scheduleEntities;
            List<ScheduleDataDTO> scheduleDTOs;

            var mapper = MapperConfiguration.CreateMapper();

            using (var leagueDb = new LeagueDbContext())
            {
                var scheduleSet = leagueDb.Set<ScheduleEntity>();

                if (scheduleIds == null || scheduleIds == new int[0])
                {
                    scheduleEntities = scheduleSet.AsQueryable();
                }
                else
                {
                    scheduleEntities = scheduleSet.Where(x => scheduleIds.Contains(x.ScheduleId));
                }

                scheduleDTOs = mapper.ProjectTo<ScheduleDataDTO>(scheduleEntities).ToList();
            }

            return scheduleDTOs;
        }

        public ResultDataDTO GetResult(int resultId)
        {
            ResultDataDTO resultData;
            using (var leagueDb = new LeagueDbContext())
            {
                var memberEntity = leagueDb.Set<ResultEntity>().Find(resultId);
                if (memberEntity != null)
                {
                    var mapper = MapperConfiguration.CreateMapper();

                    resultData = mapper.Map<ResultDataDTO>(memberEntity);
                }
                else
                {
                    resultData = null;
                }
                return resultData;
            }
        }

        public ResultDataDTO PutResult(ResultDataDTO resultData)
        {
            ResultDataDTO returnData = null;
            var mapper = MapperConfiguration.CreateMapper();

            using (LeagueDbContext leagueDb = new LeagueDbContext())
            {
                //var mapper = MapperHelper.GetEntityMapper(leagueDb);
                MapperProfile.DbContext = leagueDb;

                ResultEntity resultEntity;
                var resultSet = leagueDb.Set<ResultEntity>();

                //Put review to Db
                if (resultSet.Any(x => x.ResultId == resultData.ResultId))
                {
                    resultEntity = resultSet.Find(resultData.ResultId);
                    mapper.Map(resultData, resultEntity);
                }
                else
                {
                    resultEntity = mapper.Map<ResultEntity>(resultData);
                    resultSet.Add(resultEntity);
                }
                leagueDb.SaveChanges();

                //Get review object to return
                resultEntity = resultSet.Find(resultData.ResultId);
                returnData = mapper.Map<ResultDataDTO>(resultEntity);
            }

            return returnData;
        }
    }
}
