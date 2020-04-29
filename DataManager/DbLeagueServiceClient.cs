using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

using iRLeagueManager.LeagueDBServiceRef;
using iRLeagueManager.Models.Database;
using iRLeagueManager.Enums;
using iRLeagueManager.Interfaces;

namespace iRLeagueManager.Data
{
    public class DbLeagueServiceClient : DbServiceClientBase, ILeagueDBService
    {
        private string EndpointConfigurationName { get; } = "";

        private LeagueDBServiceClient DbClient
        {
            get
            {
                if (ConnectionStatus == ConnectionStatusEnum.Disconnected)
                    //Status.ConnectionStatus = Enums.ConnectionStatusEnum.Conecting;
                    SetConnectionStatus(Token, ConnectionStatusEnum.Connecting);

                var retVal = (EndpointConfigurationName == "") ? new LeagueDBServiceClient() : new LeagueDBServiceClient(EndpointConfigurationName);

                if (ConnectionStatus == ConnectionStatusEnum.Connecting)
                    //Status.ConnectionStatus = Enums.ConnectionStatusEnum.Connected;
                    SetConnectionStatus(Token, ConnectionStatusEnum.Connected);

                return retVal;
            }
        }

        //public DatabaseStatusModel Status { get; }

        public DbLeagueServiceClient() : base() { }

        public DbLeagueServiceClient(IDatabaseStatus status) : base(status)
        {
            //Status = new DatabaseStatusModel();
        }

        public DbLeagueServiceClient(IDatabaseStatus status, string endpointConfigurationName) : this(status)
        {
            EndpointConfigurationName = endpointConfigurationName;
        }

        protected override void SetDatabaseStatus(IToken token, DatabaseStatusEnum status, string endpointAddress = "")
        {
            base.SetDatabaseStatus(token, status, DbClient.Endpoint.Address.Uri.AbsoluteUri);
        }

        public async Task ClientCallAsync(Func<Task> func, UpdateKind updateKind, [CallerMemberName] string callName = "")
        {
            //await ClientGetAsync<object, object>(null, x => { func(); return null; }, updateKind, callName: callName);
            await ClientCallAsync<object>(null, x => func(), updateKind, callName);
        }

        public async Task ClientCallAsync<TKey>(TKey key, Func<TKey, Task> func, UpdateKind updateKind, [CallerMemberName] string callName = "")
        {
            int timeOutMilliseconds = 10000;
            try
            {
                if (await StartUpdateWhenReady(updateKind, timeOutMilliseconds, callName))
                    await func(key);
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                if (IsUpdateRunning(callName))
                    EndUpdate(callName);
            }
        }

        public async Task<TResult> ClientGetAsync<TResult>(Func<Task<TResult>> getFunc, UpdateKind updateKind, TResult defaultValue = null, [CallerMemberName] string callName = "") where TResult : class
        {
            return await ClientGetAsync<object, TResult>(null, x => getFunc(), UpdateKind.Loading, defaultValue, callName);
        }

        public async Task<TResult> ClientGetAsync<TKey, TResult>(TKey key, Func<TKey, Task<TResult>> getFunc, UpdateKind updateKind, TResult defaultValue = null, [CallerMemberName] string callName = "") where TResult : class
        {
            int timeOutMilliseconds = 10000;
            TResult retVar = defaultValue;
            try
            {
                if (await StartUpdateWhenReady(updateKind, timeOutMilliseconds, callName))
                    retVar = await getFunc(key);
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                if (IsUpdateRunning(callName))
                    EndUpdate(callName);
            }
            return retVar;
        }

        public void CleanUpSessions()
        {
            ((ILeagueDBService)DbClient).CleanUpSessions();
        }

        public async Task CleanUpSessionsAsync()
        {
            await ClientCallAsync(() => ((ILeagueDBService)DbClient).CleanUpSessionsAsync(), UpdateKind.Saving);
        }

        public CommentDataDTO GetComment(long commentId)
        {
            return ((ILeagueDBService)DbClient).GetComment(commentId);
        }

        public async Task<CommentDataDTO> GetCommentAsync(long commentId)
        {
            return await ClientGetAsync(commentId, x => ((ILeagueDBService)DbClient).GetCommentAsync(x), UpdateKind.Loading);
        }

        public LeagueMemberDataDTO GetLastMember()
        {
            return ((ILeagueDBService)DbClient).GetLastMember();
        }

        public async Task<LeagueMemberDataDTO> GetLastMemberAsync()
        {
            return await ClientGetAsync(() => ((ILeagueDBService)DbClient).GetLastMemberAsync(), UpdateKind.Loading);
        }

        public LeagueMemberDataDTO GetMember(long memberId)
        {
            return ((ILeagueDBService)DbClient).GetMember(memberId);
        }

        public async Task<LeagueMemberDataDTO> GetMemberAsync(long memberId)
        {
            return await ClientGetAsync(memberId, x => ((ILeagueDBService)DbClient).GetMemberAsync(x), UpdateKind.Loading);
        }

        public LeagueMemberDataDTO[] GetMembers(long[] memberId)
        {
            return ((ILeagueDBService)DbClient).GetMembers(memberId);
        }

        public async Task<LeagueMemberDataDTO[]> GetMembersAsync(long[] memberId = null)
        {
            return await ClientGetAsync(memberId, x => ((ILeagueDBService)DbClient).GetMembersAsync(x), UpdateKind.Loading);
        }

        public ResultDataDTO GetResult(long resultId)
        {
            return ((ILeagueDBService)DbClient).GetResult(resultId);
        }

        public async Task<ResultDataDTO> GetResultAsync(long resultId)
        {
            return await ClientGetAsync(resultId, x => ((ILeagueDBService)DbClient).GetResultAsync(x), UpdateKind.Loading);
        }

        public IncidentReviewDataDTO GetReview(long reviewId)
        {
            return ((ILeagueDBService)DbClient).GetReview(reviewId);
        }

        public async Task<IncidentReviewDataDTO> GetReviewAsync(long reviewId)
        {
            return await ClientGetAsync(reviewId, x => ((ILeagueDBService)DbClient).GetReviewAsync(x), UpdateKind.Loading);
        }

        public ScheduleDataDTO GetSchedule(long scheduleId)
        {
            return ((ILeagueDBService)DbClient).GetSchedule(scheduleId);
        }

        public async Task<ScheduleDataDTO> GetScheduleAsync(long scheduleId)
        {
            return await ClientGetAsync(scheduleId, x => ((ILeagueDBService)DbClient).GetScheduleAsync(x), UpdateKind.Loading);
        }

        public ScheduleDataDTO[] GetSchedules(long[] scheduleIds)
        {
            return ((ILeagueDBService)DbClient).GetSchedules(scheduleIds);
        }

        public async Task<ScheduleDataDTO[]> GetSchedulesAsync(long[] scheduleIds)
        {
            return await ClientGetAsync(scheduleIds, x => ((ILeagueDBService)DbClient).GetSchedulesAsync(x), UpdateKind.Loading);
        }

        public SeasonDataDTO GetSeason(long seasonId)
        {
            return ((ILeagueDBService)DbClient).GetSeason(seasonId);
        }

        public async Task<SeasonDataDTO> GetSeasonAsync(long seasonId)
        {
            
            return await ClientGetAsync(seasonId, x => ((ILeagueDBService)DbClient).GetSeasonAsync(x), UpdateKind.Loading);
        }

        public SeasonDataDTO[] GetSeasons(long[] seasonIds)
        {
            return ((ILeagueDBService)DbClient).GetSeasons(seasonIds);
        }

        public async Task<SeasonDataDTO[]> GetSeasonsAsync(long[] seasonIds)
        {
            return await ClientGetAsync(seasonIds, x => ((ILeagueDBService)DbClient).GetSeasonsAsync(x), UpdateKind.Loading);
        }

        public SessionDataDTO GetSession(long sessionId)
        {
            return ((ILeagueDBService)DbClient).GetSession(sessionId);
        }

        public async Task<SessionDataDTO> GetSessionAsync(long sessionId)
        {
            return await ClientGetAsync(sessionId, x => ((ILeagueDBService)DbClient).GetSessionAsync(x), UpdateKind.Loading);
        }

        public CommentDataDTO PutComment(ReviewCommentDataDTO comment)
        {
            return ((ILeagueDBService)DbClient).PutComment(comment);
        }

        public async Task<CommentDataDTO> PutCommentAsync(ReviewCommentDataDTO comment)
        {
            return await ClientGetAsync(comment, x => ((ILeagueDBService)DbClient).PutCommentAsync(x), UpdateKind.Updating, comment);
        }

        public LeagueMemberDataDTO PutMember(LeagueMemberDataDTO member)
        {
            return ((ILeagueDBService)DbClient).PutMember(member);
        }

        public async Task<LeagueMemberDataDTO> PutMemberAsync(LeagueMemberDataDTO member)
        {
            return await ClientGetAsync(member, x => ((ILeagueDBService)DbClient).PutMemberAsync(x), UpdateKind.Updating, member);
        }

        public ResultDataDTO PutResult(ResultDataDTO result)
        {
            return ((ILeagueDBService)DbClient).PutResult(result);
        }

        public async Task<ResultDataDTO> PutResultAsync(ResultDataDTO result)
        {
            return await ClientGetAsync(result, x => ((ILeagueDBService)DbClient).PutResultAsync(x), UpdateKind.Updating, result);
        }

        public IncidentReviewDataDTO PutReview(IncidentReviewDataDTO review)
        {
            return ((ILeagueDBService)DbClient).PutReview(review);
        }

        public async Task<IncidentReviewDataDTO> PutReviewAsync(IncidentReviewDataDTO review)
        {
            return await ClientGetAsync(review, x => ((ILeagueDBService)DbClient).PutReviewAsync(x), UpdateKind.Updating, review);
        }

        public ScheduleDataDTO PutSchedule(ScheduleDataDTO schedule)
        {
            return ((ILeagueDBService)DbClient).PutSchedule(schedule);
        }

        public async Task<ScheduleDataDTO> PutScheduleAsync(ScheduleDataDTO schedule)
        {
            return await ClientGetAsync(schedule, x => ((ILeagueDBService)DbClient).PutScheduleAsync(x), UpdateKind.Updating, schedule);
        }

        public SeasonDataDTO PutSeason(SeasonDataDTO season)
        {
            return ((ILeagueDBService)DbClient).PutSeason(season);
        }

        public async Task<SeasonDataDTO> PutSeasonAsync(SeasonDataDTO season)
        {
            return await ClientGetAsync(season, x => ((ILeagueDBService)DbClient).PutSeasonAsync(x), UpdateKind.Updating, season);
        }

        public SessionDataDTO PutSession(SessionDataDTO session)
        {
            return ((ILeagueDBService)DbClient).PutSession(session);
        }

        public async Task<SessionDataDTO> PutSessionAsync(SessionDataDTO session)
        {
            return await ClientGetAsync(session, x => ((ILeagueDBService)DbClient).PutSessionAsync(x), UpdateKind.Updating, session);
        }

        public LeagueMemberDataDTO[] UpdateMemberList(LeagueMemberDataDTO[] members)
        {
            return ((ILeagueDBService)DbClient).UpdateMemberList(members);
        }

        public async Task<LeagueMemberDataDTO[]> UpdateMemberListAsync(LeagueMemberDataDTO[] members)
        {
            return await ClientGetAsync(members, x => ((ILeagueDBService)DbClient).UpdateMemberListAsync(x), UpdateKind.Updating, members);
        }

        public string TestDB()
        {
            return ((ILeagueDBService)DbClient).TestDB();
        }

        public async Task<string> TestDBAsync()
        {
            return await ClientGetAsync(() => ((ILeagueDBService)DbClient).TestDBAsync(), UpdateKind.Loading);
        }

        public string Test(string name)
        {
            return ((ILeagueDBService)DbClient).Test(name);
        }

        public async Task<string> TestAsync(string name)
        {
            return await ClientGetAsync(name, x => ((ILeagueDBService)DbClient).TestAsync(x), UpdateKind.Loading);
        }

        public StandingsRowDTO[] GetSeasonStandings(long seasonId, long? lastSessionId)
        {
            return ((ILeagueDBService)DbClient).GetSeasonStandings(seasonId, lastSessionId);
        }

        public async Task<StandingsRowDTO[]> GetSeasonStandingsAsync(long seasonId, long? lastSessionId)
        {
            return await ClientGetAsync(new { season = seasonId, lastSession = lastSessionId }, x => ((ILeagueDBService)DbClient).GetSeasonStandingsAsync(x.season, x.lastSession), UpdateKind.Loading);
        }

        public StandingsRowDTO[] GetTeamStandings(long seasonId, long? lastSessionId)
        {
            return ((ILeagueDBService)DbClient).GetTeamStandings(seasonId, lastSessionId);
        }

        public async Task<StandingsRowDTO[]> GetTeamStandingsAsync(long seasonId, long? lastSessionId)
        {
            return await ClientGetAsync(new { season = seasonId, lastSession = lastSessionId }, x => ((ILeagueDBService)DbClient).GetTeamStandingsAsync(x.season, x.lastSession), UpdateKind.Loading);
        }

        public void SetDatabaseName(string databaseName)
        {
            ((ILeagueDBService)DbClient).SetDatabaseName(databaseName);
        }

        public async Task SetDatabaseNameAsync(string databaseName)
        {
            await ClientCallAsync(databaseName, x => ((ILeagueDBService)DbClient).SetDatabaseNameAsync(x), UpdateKind.Saving);
        }

        public ScoringDataDTO GetScoring(long scoringId)
        {
            return ((ILeagueDBService)DbClient).GetScoring(scoringId);
        }

        public async Task<ScoringDataDTO> GetScoringAsync(long scoringId)
        {
            return await ClientGetAsync(scoringId, x => ((ILeagueDBService)DbClient).GetScoringAsync(x), UpdateKind.Loading);
        }

        public ScoringDataDTO PutScoring(ScoringDataDTO scoring)
        {
            return ((ILeagueDBService)DbClient).PutScoring(scoring);
        }

        public async Task<ScoringDataDTO> PutScoringAsync(ScoringDataDTO scoring)
        {
            return await ClientGetAsync(scoring, x => ((ILeagueDBService)DbClient).PutScoringAsync(x), UpdateKind.Updating, scoring);
        }

        public ScoredResultDataDTO GetScoredResult(long sessionId, long scoringId)
        {
            return ((ILeagueDBService)DbClient).GetScoredResult(sessionId, scoringId);
        }

        public async Task<ScoredResultDataDTO> GetScoredResultAsync(long sessionId, long scoringId)
        {
            return await ClientGetAsync(new { sessionId, scoringId }, x => ((ILeagueDBService)DbClient).GetScoredResultAsync(x.sessionId, x.scoringId), UpdateKind.Loading);
        }

        public void CalculateScoredResults(long sessionId)
        {
            ((ILeagueDBService)DbClient).CalculateScoredResults(sessionId);
        }

        public async Task CalculateScoredResultsAsync(long sessionId)
        {
            await ClientCallAsync(sessionId, x => ((ILeagueDBService)DbClient).CalculateScoredResultsAsync(x), UpdateKind.Saving);
        }

        public GetItemsResponse MessageTest(GetItemsRequest request)
        {
            return ((ILeagueDBService)DbClient).MessageTest(request);
        }

        public Task<GetItemsResponse> MessageTestAsync(GetItemsRequest request)
        {
            return ((ILeagueDBService)DbClient).MessageTestAsync(request);
        }

        public GetItemsResponse GetFromDatabase(GetItemsRequest request)
        {
            return ((ILeagueDBService)DbClient).GetFromDatabase(request);
        }

        public Task<GetItemsResponse> GetFromDatabaseAsync(GetItemsRequest request)
        {
            return ((ILeagueDBService)DbClient).GetFromDatabaseAsync(request);
        }
    }
}
