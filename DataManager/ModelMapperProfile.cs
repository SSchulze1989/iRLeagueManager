// MIT License

// Copyright (c) 2020 Simon Schulze

// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:

// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.

// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.Collection;
using AutoMapper.EquivalencyExpression;
using iRLeagueManager.Data;
using iRLeagueDatabase.DataTransfer;
using iRLeagueDatabase.DataTransfer.Members;
using iRLeagueDatabase.DataTransfer.Results;
using iRLeagueDatabase.DataTransfer.Reviews;
using iRLeagueDatabase.DataTransfer.Sessions;
using iRLeagueDatabase.DataTransfer.User;
//using iRLeagueManager.UserDBServiceRef;
using iRLeagueManager.Models;
using iRLeagueManager.Models.Sessions;
using iRLeagueManager.Models.Members;
using iRLeagueManager.Models.Results;
using iRLeagueManager.Models.Reviews;
using iRLeagueManager.Models.User;
using iRLeagueManager.Interfaces;
using iRLeagueManager.Enums;
using iRLeagueManager.Timing;
using iRLeagueManager.Converters;
using System.Security.Cryptography.X509Certificates;
using System.Runtime.CompilerServices;
using System.Globalization;
using System.Security.Cryptography;
using iRLeagueDatabase.DataTransfer.Filters;
using iRLeagueManager.Models.Filters;
using iRLeagueDatabase.Extensions;
using System.Web.Hosting;
using iRLeagueDatabase.DataTransfer.Statistics;
using iRLeagueManager.Models.Statistics;

namespace iRLeagueManager
{
    internal class ModelMapperProfile : Profile
    {
        public List<LeagueMember> MemberList => LeagueContext.MemberList.ToList();
        public LeagueContext LeagueContext { get; internal set; }
        private static IEnumerable<ScheduleModel> CurrentSchedules { get; set; }
        private static IEnumerable<SessionModel> CurrentSessions { get; set; }

        public ModelMapperProfile(IModelCache modelCache)
        {
            CreateMap<MappableDTO, MappableModel>()
                .BeforeMap((src, dest) => dest?.InitReset())
                .ForMember(dest => dest.LastUpdate, opt => opt.MapFrom((src, trg) => DateTime.Now))
                .ForMember(dest => dest.IsExpired, opt => opt.MapFrom((src, trg) => false))
                .AfterMap((src, dest) => dest?.ResetChangedState())
                .IncludeAllDerived();

            // Mapping Season data
            CreateMap<SeasonDataDTO, SeasonModel>()
                .EqualityComparison((src, dest) => src.SeasonId == dest.SeasonId)
                .ConstructUsing(source => modelCache.PutOrGetModel(new SeasonModel(source.SeasonId)))
                .ForMember(dest => dest.Schedules, opt => opt.UseDestinationValue())
                .ForMember(dest => dest.Scorings, opt => opt.UseDestinationValue())
                .ForMember(dest => dest.ScoringTables, opt => opt.UseDestinationValue())
                .ForMember(dest => dest.SeasonStatisticSets, opt => opt.MapFrom(src => src.SeasonStatisticSetIds.Select(x => new StatisticSetInfo() { Id = x })))
                .AfterMap((src, dest) =>
                {
                    CurrentSchedules = null;
                    dest.InitReset();
                })
                .ReverseMap();
            CreateMap<SeasonInfoDTO, SeasonModel>()
                .EqualityComparison((src, dest) => src.SeasonId == dest.SeasonId)
                .ConstructUsing(source => modelCache.PutOrGetModel(new SeasonModel(source.SeasonId)))
                .AfterMap((src, dest) =>
                {
                    dest.InitReset();
                })
                .ReverseMap();

            // Mapping League member data
            CreateMap<LeagueMemberDataDTO, LeagueMember>()
                .ConstructUsing(source => (source != null) ? modelCache.PutOrGetModel(new LeagueMember(source.MemberId.GetValueOrDefault())) : null)
                .ReverseMap();
            CreateMap<LeagueMemberInfoDTO, LeagueMember>()
                .ConvertUsing(source => (source != null) ? modelCache.PutOrGetModel(new LeagueMember(source.MemberId.GetValueOrDefault())) : null);
            //.ConstructUsing(source => new LeagueMember(source.MemberId));
            CreateMap<LeagueMember, LeagueMemberInfoDTO>();

            CreateMap<TeamDataDTO, TeamModel>()
                .ConstructUsing(source => (source != null) ? modelCache.PutOrGetModel(new TeamModel() { TeamId = source.TeamId }) : null)
                //.ForMember(dest => dest.Members, opt => opt.MapFrom((src, dest, members) =>
                //{
                //    return new ObservableCollection<LeagueMember>(src.MemberIds.Select(x => ModelCache.PutOrGetModel(new LeagueMember(x))));
                //}))
                .ForMember(dest => dest.Members, opt => opt.MapFrom(src => src.MemberIds))
                .ForMember(dest => dest.Members, opt => opt.UseDestinationValue())
                .ReverseMap()
                .ForMember(dest => dest.MemberIds, opt => opt.MapFrom((TeamModel src, TeamDataDTO dest, long[] members) =>
                {
                    return src.Members.Select(x => x.MemberId.GetValueOrDefault()).ToArray();
                }));

            CreateMap<long, LeagueMember>()
                .ConvertUsing((src, dest) => GetLeagueMember(src, modelCache));

            // Mapping incident data
            CreateMap<IncidentReviewDataDTO, IncidentReviewModel>()
                .ConstructUsing(source => modelCache.PutOrGetModel(new IncidentReviewModel(source.ReviewId)))
                .ForMember(dest => dest.InvolvedMembers, opt => opt.MapFrom(src => src.InvolvedMemberIds.Select(x => GetLeagueMember(x, modelCache))))
                .ForMember(dest => dest.InvolvedMembers, opt => opt.UseDestinationValue())
                .ForMember(dest => dest.Comments, opt => opt.UseDestinationValue())
                .ForMember(dest => dest.AcceptedReviewVotes, opt => opt.UseDestinationValue())
                .EqualityComparison((src, dest) => src.ReviewId == dest.ReviewId)
                .IncludeBase<IncidentReviewInfoDTO, IncidentReviewInfo>()
                .AfterMap((src, dest) =>
                {
                    dest.InitReset();
                })
                .ReverseMap()
                .ForMember(dest => dest.InvolvedMemberIds, opt => opt.MapFrom(src => src.InvolvedMembers.Select(x => x.MemberId)))
                .IncludeBase<IncidentReviewInfo, IncidentReviewInfoDTO>();
            CreateMap<IncidentReviewInfoDTO, IncidentReviewInfo>()
                //.ConstructUsing(source => new IncidentReviewInfo() { ReviewId = source.ReviewId })
                .ConstructUsing(source => Test(source))
                .EqualityComparison((src, dest) => src.ReviewId == dest.ReviewId)
                .ForMember(dest => dest.Author, opt => opt.MapFrom((src, dest, author) =>
                {
                    if (dest.Author != null && dest.Author.UserId == src.AuthorUserId)
                    {
                        return dest.Author;
                    }

                    return LeagueContext.UserManager.GetUserModel(src.AuthorUserId);
                }))
                .AfterMap((src, dest) =>
                {
                    dest.InitReset();
                })
                .ReverseMap()
                .ForMember(dest => dest.AuthorUserId, opt => opt.MapFrom(src => src.Author != null ? src.Author.UserId : null));

            // Mapping comment data
            CreateMap<ReviewCommentDataDTO, ReviewCommentModel>()
                .ConstructUsing(source => modelCache.PutOrGetModel(new ReviewCommentModel(source.CommentId.GetValueOrDefault(), source.AuthorName)))
                .EqualityComparison((src, dest) => src.CommentId == dest.CommentId)
                .ForMember(dest => dest.CommentReviewVotes, opt => opt.UseDestinationValue())
                .ForMember(dest => dest.Replies, opt => opt.UseDestinationValue())
                .AfterMap((src, dest) =>
                {
                    dest.InitReset();
                })
                .IncludeAllDerived()
                .ReverseMap();
            CreateMap<CommentDataDTO, CommentModel>()
                .ConstructUsing(source => modelCache.PutOrGetModel(new CommentModel(source.CommentId.GetValueOrDefault(), source.AuthorName)))
                .EqualityComparison((src, dest) => src.CommentId == dest.CommentId)
                .ForMember(dest => dest.Replies, opt => opt.UseDestinationValue())
                .AfterMap((src, dest) =>
                {
                    dest.InitReset();
                })
                .IncludeAllDerived()
                .ReverseMap();
            CreateMap<CommentInfoDTO, CommentInfo>()
                //.ConstructUsing(source => ModelCache.PutOrGetModel(new CommentBase(source.CommentId.GetValueOrDefault(), source.AuthorName)))
                .ConstructUsing(source => new CommentInfo(source.CommentId, source.AuthorName))
                .EqualityComparison((src, dest) => src.CommentId == dest.CommentId)
                .ForMember(dest => dest.Author, opt => opt.MapFrom((src, dest, author) =>
                {
                    if (dest.Author != null && dest.Author.UserId == src.AuthorUserId)
                    {
                        return dest.Author;
                    }

                    return LeagueContext.UserManager.GetUserModel(src.AuthorUserId);
                }))
                .AfterMap((src, dest) =>
                {
                    dest.InitReset();
                })
                .IncludeAllDerived()
                .ReverseMap()
                .ForMember(dest => dest.AuthorUserId, opt => opt.MapFrom(src => src.Author != null ? src.Author.UserId : null))
                .ForMember(dest => dest.AuthorName, opt => opt.MapFrom(src => src.Author != null ? src.Author.UserName : src.AuthorName));
            //.IncludeAllDerived();

            // Mapping schedule data
            CreateMap<ScheduleDataDTO, ScheduleModel>()
                .BeforeMap((src, dest) =>
                {
                    if (CurrentSchedules == null)
                    {
                        CurrentSchedules = new ScheduleModel[0];
                    }
                    //CurrentSessions = dest?.Sessions;
                })
                //.ForMember(dest => dest.Season, opt => opt.Ignore())
                .EqualityComparison((src, dest) => src.ScheduleId == dest.ScheduleId)
                .ForMember(dest => dest.Sessions, opt => opt.UseDestinationValue())
                .ConstructUsing(source => modelCache.PutOrGetModel(new ScheduleModel(source.ScheduleId)))
                //.ForMember(dest => dest.Sessions, opt => opt.MapFrom((src, dest, target, context) => context.Mapper.Map(src.Sessions, dest.Sessions)))
                .AfterMap((src, dest) =>
                {
                    dest.InitReset();
                    CurrentSessions = null;
                    SortObservableCollection(dest.Sessions, x => x.Date);
                    int i = 1;
                    foreach (var race in dest.Sessions.Where(x => x.SessionType == SessionType.Race).Cast<RaceSessionModel>())
                    {
                        race.RaceId = i;
                        i++;
                    }
                })
                .ReverseMap();

            CreateMap<ScheduleInfoDTO, ScheduleInfo>()
                .EqualityComparison((src, dest) => src.ScheduleId == dest.ScheduleId)
                .ReverseMap()
                .IncludeAllDerived();

            // Mapping session data
            CreateMap<SessionDataDTO, SessionModel>()
                .BeforeMap((src, dest) =>
                {
                    if (CurrentSessions == null)
                    {
                        CurrentSessions = new SessionModel[0];
                    }
                })
                .EqualityComparison((src, dest) => src.SessionId == dest.SessionId)
                //.ConstructUsing(source => (source.SessionType == SessionType.Race) ? new RaceSessionModel(source.SessionId) : new SessionModel(source.SessionId, source.SessionType))
                .ConstructUsing(source => modelCache.PutOrGetModel(new SessionModel(source.SessionId, source.SessionType)))
                .ForMember(dest => dest.SessionResult, opt => opt.MapFrom(src => src.SessionResultId != null ? new ResultInfo(src.SessionResultId) : null))
                .ForMember(dest => dest.Reviews, opt => opt.MapFrom(src => src.ReviewIds.Select(x => new IncidentReviewInfo() { ReviewId = x })))
                .ForMember(dest => dest.Reviews, opt => opt.UseDestinationValue())
                .AfterMap((src, dest) =>
                {
                    dest.InitReset();
                })
                .Include<RaceSessionDataDTO, RaceSessionModel>();
            CreateMap<RaceSessionDataDTO, RaceSessionModel>()
                .BeforeMap((src, dest) =>
                {
                    if (CurrentSessions == null)
                    {
                        CurrentSessions = new SessionModel[0];
                    }
                })
                .EqualityComparison((src, dest) => src.SessionId == dest.SessionId)
                .ForMember(dest => dest.Reviews, opt => opt.UseDestinationValue())
                .ConstructUsing(source => modelCache.PutOrGetModel(new RaceSessionModel(source.SessionId, source.RaceId)))
                //.ForMember(dest => dest.Reviews, opt => opt.UseDestinationValue())
                .AfterMap((src, dest) =>
                {
                    dest.InitReset();
                });
            CreateMap<SessionModel, SessionDataDTO>()
                //.ForMember(dest => dest.LocationId, opt => opt.MapFrom(source => source.Location))
                .ForMember(dest => dest.SessionResultId, opt => opt.MapFrom(src => src.SessionResult != null ? src.SessionResult.ResultId : null))
                .ForMember(dest => dest.ReviewIds, opt => opt.MapFrom(src => src.Reviews.Select(x => x.ReviewId.GetValueOrDefault())))
                .Include<RaceSessionModel, RaceSessionDataDTO>();
            CreateMap<RaceSessionModel, RaceSessionDataDTO>();
                //.ForMember(dest => dest.LocationId, opt => opt.MapFrom(source => source.Location));
            CreateMap<SessionInfoDTO, SessionInfo>()
                .ConstructUsing(source => new SessionInfo(source.SessionId, source.SessionType))
                .ReverseMap();

            //Mapping result data
            CreateMap<ResultDataDTO, ResultModel>()
                .ConstructUsing(source => modelCache.PutOrGetModel(new ResultModel(source.ResultId.GetValueOrDefault())))
                .EqualityComparison((src, dest) => src.ResultId == dest.ResultId)
                .ForMember(dest => dest.Session, opt => opt.MapFrom(src => new SessionInfo(src.SessionId, SessionType.Undefined)))
                .ForMember(dest => dest.RawResults, opt => opt.UseDestinationValue())
                .ForMember(dest => dest.Reviews, opt => opt.MapFrom(src => src.ReviewIds.Select(x => new IncidentReviewInfo() { ReviewId = x })))
                .ForMember(dest => dest.Reviews, opt => opt.UseDestinationValue())
                .ReverseMap()
                .ForMember(dest => dest.SessionId, opt => opt.MapFrom(src => src.Session.SessionId))
                .ForMember(dest => dest.ReviewIds, opt => opt.MapFrom(src => src.Reviews.Select(x => x.ReviewId.GetValueOrDefault())));

            CreateMap<SimSessionDetailsDTO, SimSessionDetails>()
                .ReverseMap();

            CreateMap<ResultInfoDTO, ResultInfo>()
                .ConstructUsing(source => new ResultInfo(source.ResultId.GetValueOrDefault()))
                .ReverseMap()
                .Include<ResultModel, ResultDataDTO>();

            CreateMap<ResultRowDataDTO, ResultRowModel>()
                .ConstructUsing(source => modelCache.PutOrGetModel(new ResultRowModel(source.ResultRowId)))
                .ForMember(dest => dest.Member, opt => opt.MapFrom(src => GetLeagueMember(src.MemberId, modelCache)))
                .ForMember(dest => dest.Team, opt => opt.MapFrom(src => src.TeamId != null ? modelCache.PutOrGetModel(new TeamModel() { TeamId = src.TeamId.GetValueOrDefault() }) : null))
                .ForMember(dest => dest.Location, opt => opt.MapFrom((src, trg) => LeagueContext.Locations.FirstOrDefault(x => x.LocationId == src.LocationId)))
                .EqualityComparison((src, dest) => src.ResultRowId == dest.ResultRowId)
                .ReverseMap()
                .ForMember(dest => dest.MemberId, opt => opt.MapFrom(src => src.Member.MemberId))
                .ForMember(dest => dest.LocationId, opt => opt.MapFrom(src => src.Location != null ? src.Location.LocationId : null))
                .ForMember(dest => dest.TeamId, opt => opt.MapFrom(src => src.Team != null ? src.Team.TeamId : (long?)null));

            CreateMap<ScoringDataDTO, ScoringModel>()
                .ConstructUsing(source => modelCache.PutOrGetModel(new ScoringModel(source.ScoringId)))
                .EqualityComparison((src, dest) => src.ScoringId == dest.ScoringId)
                .ForMember(dest => dest.ConnectedSchedule, opt => opt.MapFrom(src => src.ConnectedScheduleId != null ? new ScheduleInfo(src.ConnectedScheduleId) : null))
                .ForMember(dest => dest.Sessions, opt => opt.MapFrom(src => src.SessionIds.Select(x => new SessionInfo(x, SessionType.Undefined))))
                .ForMember(dest => dest.ExtScoringSource, opt => opt.MapFrom(src => src.ExtScoringSourceId != null ? new ScoringInfo(src.ExtScoringSourceId) : null))
                .ForMember(dest => dest.BasePoints, opt => opt.MapFrom((src, dest, result) =>
                {
                    ObservableCollection<ScoringModel.BasePointsValue> pairs = new ObservableCollection<ScoringModel.BasePointsValue>();
                    pairs.Clear();
                    if (src.BasePoints == null || src.BasePoints == "" || src.BasePoints == " ")
                    {
                        return pairs;
                    }
                    string[] pointString = src.BasePoints.Split(' ');
                    for (int i = 0; i < pointString.Count(); i++)
                    {
                        pairs.Add(new ScoringModel.BasePointsValue(i + 1, int.Parse(pointString[i])));
                    }
                    return pairs;
                }))
                .ForMember(dest => dest.BasePoints, opt => opt.UseDestinationValue())
                .ForMember(dest => dest.BonusPoints, opt => opt.MapFrom((src, dest, result) =>
                {
                    ObservableCollection<ScoringModel.BonusPointsValue> pairs = new ObservableCollection<ScoringModel.BonusPointsValue>();
                    pairs.Clear();
                    if (src.BonusPoints == null || src.BonusPoints == "" || src.BonusPoints == " ")
                        return pairs;
                    string[] pointString = src.BonusPoints.Split(' ');
                    for (int i = 0; i < pointString.Count(); i++)
                    {
                        var stringPair = pointString[i].Split(':');
                        pairs.Add(new ScoringModel.BonusPointsValue(stringPair.First(), int.Parse(stringPair.Last())));
                    }
                    return pairs;
                }))
                .ForMember(dest => dest.BonusPoints, opt => opt.UseDestinationValue())
                .ForMember(dest => dest.Sessions, opt => opt.UseDestinationValue())
                .ForMember(dest => dest.Standings, opt => opt.UseDestinationValue())
                .ForMember(dest => dest.ResultsFilterOptionIds, opt => opt.UseDestinationValue())
                .ForMember(dest => dest.IncPenaltyPoints, opt => opt.Ignore())
                .ReverseMap()
                .ForMember(dest => dest.ConnectedScheduleId, opt => opt.MapFrom(src => src.ConnectedSchedule != null ? src.ConnectedSchedule.ScheduleId : null))
                .ForMember(dest => dest.SessionIds, opt => opt.MapFrom(src => src.Sessions.Select(x => x.SessionId)))
                .ForMember(dest => dest.ExtScoringSourceId, opt => opt.MapFrom(src => src.ExtScoringSource != null ? src.ExtScoringSource.ScoringId : null))
                .ForMember(dest => dest.BasePoints, opt => opt.MapFrom(src => (src.BasePoints.Count > 0) ? src.BasePoints.Select(x => x.Value.ToString()).Aggregate((x, y) => x + " " + y) : ""))
                .ForMember(dest => dest.BonusPoints, opt => opt.MapFrom(src => (src.BonusPoints.Count > 0) ? src.BonusPoints.Select(x => x.Key + ":" + x.Value.ToString()).Aggregate((x, y) => x + " " + y) : ""))
                .ForMember(dest => dest.IncPenaltyPoints, opt => opt.Ignore());
            CreateMap<ScoringInfoDTO, ScoringModel>()
                .ConstructUsing(source => modelCache.PutOrGetModel(new ScoringModel(source.ScoringId)))
                .EqualityComparison((src, dest) => src.ScoringId == dest.ScoringId)
                .ForAllMembers(opt => opt.Ignore());
            CreateMap<ScoringModel, ScoringInfoDTO>();
            CreateMap<ScoringInfoDTO, ScoringInfo>()
                .ConstructUsing(source => new ScoringInfo(source.ScoringId))
                .EqualityComparison((src, dest) => src.ScoringId == dest.ScoringId)
                .ReverseMap();
            CreateMap<ScoringTableDataDTO, ScoringTableModel>()
                .ConstructUsing(source => modelCache.PutOrGetModel(new ScoringTableModel() { ScoringTableId = source.ScoringTableId}))
                .EqualityComparison((src, dest) => src.ScoringTableId == dest.ScoringTableId)
                .ForMember(dest => dest.Sessions, opt => opt.MapFrom(src => src.SessionIds.Select(x => new SessionInfo(x, SessionType.Undefined))))
                .ForMember(dest => dest.Scorings, opt => opt.MapFrom((src, dest, result, context) =>
                {
                    List<double> factors = new List<double>();
                    bool success = true;
                    if (src.ScoringFactors != null)
                    {
                        var factorStrings = src.ScoringFactors.Replace(',', '.').Split(';');
                        foreach (var factorString in factorStrings)
                        {
                            if (double.TryParse(factorString, System.Globalization.NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out double factor))
                            {
                                factors.Add(factor);
                            }
                            else
                            {
                                success = false;
                                break;
                            }
                        }
                    }

                    if (factors.Count() == 0 || success == false)
                    {
                        factors = src.ScoringIds.Select(x => (double)1).ToList();
                    }

                    var mapper = context.Mapper;
                    //var destMultiScorings = src.Scorings.Select((x, i) => new MyKeyValuePair<ScoringInfo, double>(mapper.Map<ScoringModel>(x), factors.ElementAt(i)));
                    var destMultiScorings = src.ScoringIds.Select((x, i) => new MyKeyValuePair<ScoringInfo, double>(mapper.Map<ScoringInfo>(modelCache.PutOrGetModel(new ScoringInfo(x))), factors.ElementAt(i)));
                    return new ObservableCollection<MyKeyValuePair<ScoringInfo, double>>(destMultiScorings);
                }))
                .ForMember(dest => dest.Scorings, opt => opt.UseDestinationValue())
                .ReverseMap()
                .ForMember(dest => dest.SessionIds, opt => opt.MapFrom(src => src.Sessions.Select(x => x.SessionId)))
                .ForMember(dest => dest.ScoringFactors, opt => opt.MapFrom((src, dest, factors) =>
                {
                    if (src.Scorings?.Count > 0)
                        return src.Scorings.Select(x => x.Value.ToString()).Aggregate((x, y) => x + ";" + y);
                    return null;
                }))
                .ForMember(dest => dest.ScoringIds, opt => opt.MapFrom((src, dest, scoringIds) =>
                {
                    if (src.Scorings?.Count > 0)
                        return src.Scorings.Select(x => x.Key.ScoringId.GetValueOrDefault()).Where(x => x != default).ToArray();
                    return new long[0];
                }));
            CreateMap<ScoredResultRowDataDTO, ScoredResultRowModel>()
                //.ConstructUsing(source => ModelCache.PutOrGetModel(new ScoredResultRowModel() { ScoredResultRowId = source.ScoredResultRowId}))
                .ConstructUsing(source => new ScoredResultRowModel() { ScoredResultRowId = source.ScoredResultRowId })
                .ForMember(dest => dest.Location, opt => opt.MapFrom((src, trg) => LeagueContext.Locations.FirstOrDefault(x => x.LocationId == src.LocationId)))
                .EqualityComparison((src, dest) => src.ScoredResultRowId == dest.ScoredResultRowId)
                .IncludeBase<ResultRowDataDTO, ResultRowModel>();
            //.EqualityComparison((src, dest) => src.ScoredResultRowId == dest.ScoredResultRowId)

            CreateMap<ScoredResultDataDTO, ScoredResultModel>()
                .ConstructUsing(source => modelCache.PutOrGetModel(new ScoredResultModel() { Scoring = new ScoringInfo(source.ScoringId), ResultId = source.ResultId }))
                .EqualityComparison((src, dest) => src.SessionId == dest.Session.SessionId && src.ScoringId == dest.Scoring.ScoringId)
                .ForMember(dest => dest.Scoring, opt => opt.MapFrom(src => new ScoringInfo(src.ScoringId)))
                .ForMember(dest => dest.FinalResults, opt => opt.MapFrom(src => src.FinalResults.OrderBy(x => x.FinalPosition)))
                .ForMember(dest => dest.FinalResults, opt => opt.UseDestinationValue())
                .ForMember(dest => dest.HardChargers, opt => opt.MapFrom(src => src.HardChargerMemberIds.Select(x => GetLeagueMember(x, modelCache))))
                .ForMember(dest => dest.CleanestDrivers, opt => opt.MapFrom(src => src.CleanesDriverMemberIds.Select(x => GetLeagueMember(x, modelCache))))
                .ForMember(dest => dest.HardChargers, opt => opt.UseDestinationValue())
                .ForMember(dest => dest.CleanestDrivers, opt => opt.UseDestinationValue())
                .Include<ScoredTeamResultDataDTO, ScoredTeamResultModel>();

            CreateMap<ScoredTeamResultDataDTO, ScoredTeamResultModel>()
                .ConstructUsing(source => modelCache.PutOrGetModel(new ScoredTeamResultModel() { Scoring = new ScoringInfo(source.ScoringId), ResultId = source.ResultId }))
                .EqualityComparison((src, dest) => src.SessionId == dest.Session.SessionId && src.ScoringId == dest.Scoring.ScoringId)
                .ForMember(dest => dest.FinalResults, opt => opt.MapFrom(src => src.FinalResults.OrderBy(x => x.FinalPosition)))
                .ForMember(dest => dest.FinalResults, opt => opt.UseDestinationValue())
                .ForMember(dest => dest.TeamResults, opt => opt.MapFrom((src, trg) => src.TeamResults.OrderBy(x => x.FinalPosition)))
                .ForMember(dest => dest.TeamResults, opt => opt.UseDestinationValue())
                .IncludeBase<ScoredResultDataDTO, ScoredResultModel>();

            CreateMap<ScoredTeamResultRowDataDTO, ScoredTeamResultRowModel>()
                .ConstructUsing(source => new ScoredTeamResultRowModel() { ScoredResultRowId = source.ScoredResultRowId })
                .EqualityComparison((src, dest) => src.ScoredResultRowId == dest.ScoredResultRowId)
                .ForMember(dest => dest.ScoredResultRows, opt => opt.UseDestinationValue())
                .ForMember(dest => dest.Team, opt => opt.MapFrom((src, dst) =>
                {
                    return modelCache.PutOrGetModel(new TeamModel() { TeamId = src.TeamId });
                }));

            CreateMap<StandingsDataDTO, StandingsModel>()
                .ConstructUsing(source => modelCache.PutOrGetModel(new StandingsModel() { ScoringTableId = source.ScoringTableId, SessionId = source.SessionId }))
                .EqualityComparison((src, dest) => src.ScoringTableId == dest.ScoringTableId)
                .ForMember(dest => dest.MostWinsDriver, opt => opt.MapFrom(src => GetLeagueMember(src.MostWinsDriverId, modelCache)))
                .ForMember(dest => dest.MostPolesDriver, opt => opt.MapFrom(src => GetLeagueMember(src.MostPolesDriverId, modelCache)))
                .ForMember(dest => dest.CleanestDriver, opt => opt.MapFrom(src => GetLeagueMember(src.CleanestDriverId, modelCache)))
                .ForMember(dest => dest.MostPenaltiesDriver, opt => opt.MapFrom(src => GetLeagueMember(src.MostPenaltiesDriverId, modelCache)))
                .ForMember(dest => dest.StandingsRows, opt => opt.UseDestinationValue())
                .Include<TeamStandingsDataDTO, TeamStandingsModel>();

            CreateMap<StandingsRowDataDTO, StandingsRowModel>()
                //.ConstructUsing(source => ModelCache.PutOrGetModel(new StandingsRowModel() { Scoring = new ScoringInfo(source.Scoring.ScoringId), Member = new LeagueMember(source.Member.MemberId) }))
                .ConstructUsing(source => new StandingsRowModel())
                .ForMember(dest => dest.Member, opt => opt.MapFrom(src => GetLeagueMember(src.MemberId, modelCache)))
                .ForMember(dest => dest.CountedResults, opt => opt.MapFrom(src => src.CountedResults.OrderBy(x => x.Date)))
                .ForMember(dest => dest.DroppedResults, opt => opt.MapFrom(src => src.DroppedResults.OrderBy(x => x.Date)))
                .ForMember(dest => dest.Team, opt => opt.MapFrom(src => src.TeamId != null ? modelCache.PutOrGetModel(new TeamModel() { TeamId = src.TeamId.Value }) : null))
                .EqualityComparison((src, dest) => src.MemberId == dest.Member.MemberId)
                .Include<TeamStandingsRowDataDTO, TeamStandingsRowModel>()
                .AfterMap((src, dest) => dest.DroppedResults.ForEach(x => x.IsDroppedResult = true));

            CreateMap<TeamStandingsDataDTO, TeamStandingsModel>()
                .ConstructUsing(source => modelCache.PutOrGetModel(new TeamStandingsModel() { ScoringTableId = source.ScoringTableId, SessionId = source.SessionId }))
                .EqualityComparison((src, dest) => src.ScoringTableId == dest.ScoringTableId)
                .ForMember(dest => dest.StandingsRows, opt => opt.UseDestinationValue());
            CreateMap<TeamStandingsRowDataDTO, TeamStandingsRowModel>()
                .ConstructUsing(source => new TeamStandingsRowModel())
                .ForMember(dest => dest.Team, opt => opt.MapFrom((src, dst) =>
                {
                    return modelCache.PutOrGetModel(new TeamModel() { TeamId = src.TeamId });
                }))
                .ForMember(dest => dest.DriverStandingsRows, opt => opt.MapFrom(src => src.DriverStandingsRows.OrderBy(x => -x.TotalPoints).ThenBy(x => -x.PenaltyPoints)))
                .ForMember(dest => dest.DriverStandingsRows, opt => opt.UseDestinationValue())
                .EqualityComparison((src, dest) => src.TeamId == dest.Team.TeamId);

            CreateMap<AddPenaltyDTO, AddPenaltyModel>()
                .ConstructUsing(source => modelCache.PutOrGetModel(new AddPenaltyModel(source.ScoredResultRowId)))
                .EqualityComparison((src, dest) => src.ScoredResultRowId == dest.ScoredResultRowId)
                .ReverseMap();

            //CreateMap<LeagueUserDTO, UserModel>()
            //    .ConstructUsing(source => ModelCache.PutOrGetModel(new UserModel(source.AdminId)))
            //    .EqualityComparison((src, dest) => src.AdminId == dest.UserId);

            CreateMap<UserDTO, UserModel>()
                .ConstructUsing(src => modelCache.PutOrGetModel(new UserModel(src.UserId)));
                //.ForMember(dest => dest.Admin, opt => opt.MapFrom(src => src));
            CreateMap<UserModel, UserDTO>();

            CreateMap<ReviewVoteDataDTO, ReviewVoteModel>()
                .ConstructUsing(src => new ReviewVoteModel() { ReviewVoteId = src.ReviewVoteId })
                .EqualityComparison((src, dest) => src.ReviewVoteId == dest.ReviewVoteId)
                .ForMember(dest => dest.MemberAtFault, opt => opt.MapFrom(src => GetLeagueMember(src.MemberAtFaultId, modelCache)))
                .ForMember(dest => dest.VoteCategory, opt => opt
                    .MapFrom(src => src.VoteCategoryId != null ? modelCache.PutOrGetModel(new VoteCategoryModel() { CatId = src.VoteCategoryId.Value }) : null))
                .ReverseMap()
                .ForMember(dest => dest.MemberAtFaultId, opt => opt.MapFrom(src => src.MemberAtFault != null ? src.MemberAtFault.MemberId : null))
                .ForMember(dest => dest.VoteCategoryId, opt => opt.MapFrom(src => src.VoteCategory != null ? src.VoteCategory.CatId : (long?)null));

            CreateMap<TimeSpan, LapTime>()
                .ConvertUsing<LapTimeConverter>();
            CreateMap<TimeSpan, LapInterval>()
                .ConvertUsing<LapIntervalConverter>();

            CreateMap<LapTime, TimeSpan>()
                .ConvertUsing<LapTimeConverter>();
            CreateMap<LapInterval, TimeSpan>()
                .ConvertUsing<LapIntervalConverter>();

            CreateMap<VoteCategoryDTO, VoteCategoryModel>()
                .ConstructUsing(src => modelCache.PutOrGetModel(new VoteCategoryModel() { CatId = src.CatId}))
                .EqualityComparison((src, dest) => src.CatId == dest.CatId)
                .ReverseMap();

            CreateMap<CustomIncidentDTO, CustomIncidentModel>()
                .ConstructUsing(src => modelCache.PutOrGetModel(new CustomIncidentModel() { IncidentId = src.IncidentId }))
                .EqualityComparison((src, dest) => src.IncidentId == dest.IncidentId)
                .ReverseMap();

            CreateMap<ReviewPenaltyDTO, ReviewPenaltyModel>()
                .ConstructUsing(src => modelCache.PutOrGetModel(new ReviewPenaltyModel() { ResultRowId = src.ResultRowId, ReviewId = src.ReviewId }))
                .EqualityComparison((src, dest) => src.ResultRowId == dest.ResultRowId && src.ReviewId == dest.ReviewId)
                .ReverseMap();

            CreateMap<ResultsFilterOptionDTO, ResultsFilterOptionModel>()
                .ConstructUsing(src => modelCache.PutOrGetModel(new ResultsFilterOptionModel(src.ResultsFilterId, src.ScoringId)))
                .EqualityComparison((src, dest) => src.ResultsFilterId == dest.ResultsFilterId)
                .ForMember(dest => dest.FilterValues, opt => opt.MapFrom((src, dest, destMember, context) =>
                {
                    var targetColumnProperty = typeof(ResultRowModel).GetNestedPropertyInfo(dest.ColumnPropertyName);
                    var sourceColumnProperty = typeof(ResultRowDataDTO).GetNestedPropertyInfo(src.ColumnPropertyName);
                    var targetPropertyType = targetColumnProperty.PropertyType;
                    var sourcePropertyType = sourceColumnProperty.PropertyType;
                    return new ObservableCollection<FilterValueModel>(src.FilterValues?
                        .Select(x => new FilterValueModel(targetPropertyType, targetPropertyType.Equals(sourcePropertyType) == false ? context.Mapper.Map(x, sourcePropertyType, targetPropertyType) : x))
                        ?? new FilterValueModel[0]);
                }))
                .ForMember(dest => dest.FilterValues, opt => opt.UseDestinationValue())
                .ReverseMap()
                .ForMember(dest => dest.FilterValues, opt => opt.MapFrom((src, dest, destMember, context) =>
                {
                    var targetColumnProperty = typeof(ResultRowDataDTO).GetNestedPropertyInfo(dest.ColumnPropertyName);
                    var sourceColumnProperty = typeof(ResultRowModel).GetNestedPropertyInfo(src.ColumnPropertyName);
                    var targetPropertyType = targetColumnProperty.PropertyType;
                    var sourcePropertyType = sourceColumnProperty.PropertyType;
                    return src.FilterValues?
                        .Select(x => targetPropertyType.Equals(sourcePropertyType) == false ? context.Mapper.Map(x.Value, sourcePropertyType, targetPropertyType) : x.Value)
                        .ToArray()
                        ?? new object[0];
                }));

            #region statistic mapping
            CreateMap<StatisticSetDTO, StatisticSetInfo>();

            CreateMap<StatisticSetDTO, StatisticSetModel>()
                .ConstructUsing(src => modelCache.PutOrGetModel(new StatisticSetModel() { Id = src.Id }))
                .EqualityComparison((src, dest) => src.Id == dest.Id)
                .Include<SeasonStatisticSetDTO, SeasonStatisticSetModel>()
                .Include<LeagueStatisticSetDTO, LeagueStatisticSetModel>()
                .Include<ImportedStatisticSetDTO, ImportedStatisticSetModel>()
                .ReverseMap()
                .IncludeAllDerived();

            CreateMap<SeasonStatisticSetDTO, SeasonStatisticSetModel>()
                .ConstructUsing(src => modelCache.PutOrGetModel(new SeasonStatisticSetModel() { Id = src.Id}))
                .ForMember(dest => dest.Season, opt => opt.MapFrom(src => modelCache.PutOrGetModel(new SeasonModel() { SeasonId = src.SeasonId })))
                .ForMember(dest => dest.ScoringTable, opt => opt.MapFrom(src => src.ScoringTableId.HasValue && src.ScoringTableId != 0 ? modelCache.PutOrGetModel(new ScoringTableModel() { ScoringTableId = src.ScoringTableId.Value }) : null))
                .EqualityComparison((src, dest) => src.Id == dest.Id)
                .ReverseMap()
                .ForMember(dest => dest.ScoringTableId, opt => opt.MapFrom(src => src != null ? src.ScoringTable.ScoringTableId : (long?)null))
                .ForMember(dest => dest.SeasonId, opt => opt.MapFrom(src => src.Season.SeasonId));

            CreateMap<LeagueStatisticSetDTO, LeagueStatisticSetModel>()
                .ConstructUsing(src => modelCache.PutOrGetModel(new LeagueStatisticSetModel() { Id = src.Id }))
                .ForMember(dest => dest.StatisticSets, opt => opt.MapFrom(src => src.SeasonStatisticSetIds.Select(x => modelCache.GetModel<StatisticSetModel>(x) ?? new StatisticSetModel() { Id = x })))
                .EqualityComparison((src, dest) => src.Id == dest.Id)
                .ReverseMap()
                .ForMember(dest => dest.SeasonStatisticSetIds, opt => opt.MapFrom(src => src.StatisticSets.Select(x => x.Id)));

            CreateMap<ImportedStatisticSetDTO, ImportedStatisticSetModel>()
                .ConstructUsing(src => modelCache.PutOrGetModel(new ImportedStatisticSetModel() { Id = src.Id }))
                .EqualityComparison((src, dest) => src.Id == dest.Id)
                .ReverseMap();

            CreateMap<DriverStatisticDTO, DriverStatisticModel>()
                .ConstructUsing(src => modelCache.PutOrGetModel(new DriverStatisticModel() { StatisticSetId = src.StatisticSetId }))
                .EqualityComparison((src, dest) => src.StatisticSetId == dest.StatisticSetId)
                .ReverseMap();

            CreateMap<DriverStatisticRowDTO, DriverStatisticRowModel>()
                .EqualityComparison((src, dest) => src.StatisticSetId == dest.StatisticSetId && src.MemberId == dest.MemberId)
                .ForMember(dest => dest.LeagueMember, opt => opt.MapFrom(src => GetLeagueMember(src.MemberId, modelCache)))
                .ReverseMap();
            #endregion
        }

        private void SortObservableCollection<T, TKey>(ObservableCollection<T> collection, Func<T, TKey> key)
        {
            if (collection == null)
                return;

            lock (collection)
            {
                var sortedList = collection.OrderBy(key);

                for (int i = 0; i < sortedList.Count(); i++)
                {
                    var item = sortedList.ElementAt(i);
                    if (!collection.ElementAt(i).Equals(item))
                    {
                        var index = collection.IndexOf(item);
                        collection.Move(index, i);
                    }
                }
            }
        }

        public IncidentReviewInfo Test(IncidentReviewInfoDTO dto)
        {
            var review = new IncidentReviewInfo() { ReviewId = dto.ReviewId };
            return review;
        }

        public LeagueMember GetLeagueMember(long? memberId, IModelCache modelCache)
        {
            return memberId != null ? modelCache.PutOrGetModel(new LeagueMember(memberId)) : null;
        }
    }

    public class LapTimeConverter : ITypeConverter<TimeSpan, LapTime>, ITypeConverter<LapTime, TimeSpan>
    {
        LapTime ITypeConverter<TimeSpan, LapTime>.Convert(TimeSpan source, LapTime destination, ResolutionContext context)
        {
            return new LapTime(source);
        }

        TimeSpan ITypeConverter<LapTime, TimeSpan>.Convert(LapTime source, TimeSpan destination, ResolutionContext context)
        {
            return source.Time;
        }
    }

    public class LapIntervalConverter : ITypeConverter<TimeSpan, LapInterval>, ITypeConverter<LapInterval, TimeSpan>
    {
        LapInterval ITypeConverter<TimeSpan, LapInterval>.Convert(TimeSpan source, LapInterval destination, ResolutionContext context)
        {
            return new LapInterval(source);
        }

        TimeSpan ITypeConverter<LapInterval, TimeSpan>.Convert(LapInterval source, TimeSpan destination, ResolutionContext context)
        {
            return source.Time;
        }
    }
}
