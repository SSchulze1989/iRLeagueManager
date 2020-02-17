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

        public string Test(string name)
        {
            return "Hallo " + name + "!";
        }

        public string TestDB()
        {
            string result;
            using (var leagueDb = new LeagueDbContext())
            {
                result = leagueDb.Seasons.First().SeasonName;
            }
            return result;
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

        public StandingsRowDTO[] GetSeasonStandings(int seasonId, int? lastSessionId = null)
        {
            SeasonEntity seasonEntity;
            List<StandingsRowDTO> standings = new List<StandingsRowDTO>();

            using (var leagueDb = new LeagueDbContext())
            {
                seasonEntity = leagueDb.Seasons.Find(seasonId);

                if (seasonEntity == null)
                    return null;

                IEnumerable<ResultEntity> results = leagueDb.Set<ResultEntity>().Where(x => x.Session.Schedule.Season.SeasonId == seasonId)
                    .OrderBy(x => x.Session.Date);

                if (lastSessionId != null)
                {
                    var lastSession = results.Select(x => x.Session).SingleOrDefault(x => x.SessionId == lastSessionId);
                    results = results.Where(x => x.Session.Date <= lastSession.Date).OrderBy(x => x.Session.Date);
                }

                Func<ResultRowEntity, int> getPoints = new Func<ResultRowEntity, int>(x => x.TotalPoints);
                int racesCounted = 8;

                //Calculate standings
                // Get different drivers in season
                var drivers = results.Select(x => x.RawResults.Select(y => y.Member)).Aggregate((x, y) => x.Concat(y)).Distinct();
                var lastRace = results.OrderBy(x => x.Session.Date).LastOrDefault();

                Dictionary<StandingsRowDTO, IEnumerable<ResultRowEntity>> standingsList = new Dictionary<StandingsRowDTO, IEnumerable<ResultRowEntity>>();
                Dictionary<StandingsRowDTO, ResultRowEntity> lastRaceList = new Dictionary<StandingsRowDTO, ResultRowEntity>();

                foreach (var driver in drivers)
                {
                    var driverResults = results.Where(x => x.RawResults.Exists(y => y.Member.MemberId == driver.MemberId)).OrderBy(x => x.Session.Date).ToList();
                    var driverResultRows = driverResults.Select(x => x.RawResults?.SingleOrDefault(y => y.Member.MemberId == driver.MemberId)).ToList();
                    var driverResultsCounted = driverResultRows.OrderBy(x => x.TotalPoints).Where(x => x.ResultId != lastRace.ResultId).Take(racesCounted);
                    var driverLastRaceResult = driverResultRows.SingleOrDefault(x => x.Result.Session.Date == lastRace.Session.Date);

                    var driverStandingsRow = new StandingsRowDTO()
                    {
                        MemberId = driver.MemberId,
                        Name = driver.Fullname,
                        Wins = driverResultRows.Select(x => x.FinalPosition).Where(x => x == 1).Count(),
                        Top3 = driverResultRows.Select(x => x.FinalPosition).Where(x => x <= 3).Count(),
                        Top5 = driverResultRows.Select(x => x.FinalPosition).Where(x => x <= 5).Count(),
                        Top10 = driverResultRows.Select(x => x.FinalPosition).Where(x => x <= 10).Count(),
                        Top15 = driverResultRows.Select(x => x.FinalPosition).Where(x => x <= 15).Count(),
                        Top20 = driverResultRows.Select(x => x.FinalPosition).Where(x => x <= 20).Count(),
                        Poles = driverResultRows.Select(x => x.StartPosition).Where(x => x == 1).Count(),
                        PenaltyPoints = driverResultRows.Select(x => x.PenaltyPoints).Aggregate((x, y) => x + y),
                        FastestLaps = driverResults.Select(x => x.RawResults.OrderBy(y => y.FastestLapTime).First()).Where(x => x.Member.MemberId == driver.MemberId).Count(),
                        RacesParticipated = driverResults.Count(),
                        RacesCounted = driverResultsCounted.Count(),
                        Change = 0,
                        PointsChange = 0
                    };

                    KeyValuePair<StandingsRowDTO, IEnumerable<ResultRowEntity>> standingsPair = new KeyValuePair<StandingsRowDTO, IEnumerable<ResultRowEntity>>(driverStandingsRow, driverResultsCounted);
                    KeyValuePair<StandingsRowDTO, ResultRowEntity> lastRacePair = new KeyValuePair<StandingsRowDTO, ResultRowEntity>(driverStandingsRow, driverLastRaceResult);
                    standingsList.Add(standingsPair.Key, standingsPair.Value);
                    lastRaceList.Add(lastRacePair.Key, lastRacePair.Value);
                }

                standings = CalcPoints(standingsList, getPoints).ToList();
                standings = CalcPositions(standings).ToList();

                foreach (var key in standingsList.Keys)
                {
                    if (lastRaceList[key] != null)
                    {
                        standingsList[key] = standingsList[key].Take(racesCounted - 1).Concat(new ResultRowEntity[] { lastRaceList[key] });
                    }
                }

                standings = CalcPoints(standingsList, getPoints).ToList();
                standings = CalcPositions(standings).OrderBy(x => x.Pos).ToList();
            }

            return standings.ToArray();
        }

        private IEnumerable<StandingsRowDTO> CalcPoints(IEnumerable<KeyValuePair<StandingsRowDTO, IEnumerable<ResultRowEntity>>> results, Func<ResultRowEntity, int> getPoints)
        {
            foreach (var entry in results)
            {
                var standingsRow = entry.Key;
                var resultRows = entry.Value;

                standingsRow.PointsChange = 0;

                foreach (var resultRow in resultRows)
                {
                    var racePoints = getPoints(resultRow);
                    standingsRow.Points += racePoints;
                    standingsRow.PointsChange = racePoints;
                }
            }
            return results.Select(x => x.Key);
        }

        private IEnumerable<StandingsRowDTO> CalcPositions(IEnumerable<StandingsRowDTO> standingsRows)
        {
            standingsRows = standingsRows.OrderBy(x => x.PenaltyPoints).OrderByDescending(x => x.Top3).OrderByDescending(x => x.Wins).OrderByDescending(x => x.Points);

            for (int i = 0; i < standingsRows.Count(); i++)
            {
                var row = standingsRows.ElementAt(i);
                row.Change = row.Pos - (i + 1);
                row.Pos = i + 1;
            }

            return standingsRows;
        }

        public StandingsRowDTO[] GetTeamStandings(int seasonId, int? lastSessionId)
        {
            return null;
        }
    }
}
