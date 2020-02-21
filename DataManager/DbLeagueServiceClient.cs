using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public void CleanUpSessions()
        {
            ((ILeagueDBService)DbClient).CleanUpSessions();
        }

        public async Task CleanUpSessionsAsync()
        {
            if (!await StartUpdateWhenReady(UpdateKind.Saving))
                return;
            await ((ILeagueDBService)DbClient).CleanUpSessionsAsync();
            EndUpdate();
        }

        public CommentDataDTO GetComment(long commentId)
        {
            return ((ILeagueDBService)DbClient).GetComment(commentId);
        }

        public async Task<CommentDataDTO> GetCommentAsync(long commentId)
        {
            if (!await StartUpdateWhenReady(UpdateKind.Loading))
                return null;
            var retVal = await ((ILeagueDBService)DbClient).GetCommentAsync(commentId);
            EndUpdate();
            return retVal;
        }

        public LeagueMemberDataDTO GetLastMember()
        {
            return ((ILeagueDBService)DbClient).GetLastMember();
        }

        public async Task<LeagueMemberDataDTO> GetLastMemberAsync()
        {
            if (!await StartUpdateWhenReady(UpdateKind.Loading))
                return null;
            var retVal = await ((ILeagueDBService)DbClient).GetLastMemberAsync();
            EndUpdate();
            return retVal;
        }

        public LeagueMemberDataDTO GetMember(long memberId)
        {
            return ((ILeagueDBService)DbClient).GetMember(memberId);
        }

        public async Task<LeagueMemberDataDTO> GetMemberAsync(long memberId)
        {
            if (!await StartUpdateWhenReady(UpdateKind.Loading))
                return null;
            var retVal = await ((ILeagueDBService)DbClient).GetMemberAsync(memberId);
            EndUpdate();
            return retVal;
        }

        public LeagueMemberDataDTO[] GetMembers(long[] memberId)
        {
            return ((ILeagueDBService)DbClient).GetMembers(memberId);
        }

        public async Task<LeagueMemberDataDTO[]> GetMembersAsync(long[] memberId = null)
        {
            if (!await StartUpdateWhenReady(UpdateKind.Loading))
                return new LeagueMemberDataDTO[0];
            var retVal = await ((ILeagueDBService)DbClient).GetMembersAsync(memberId);
            EndUpdate();
            return retVal;
        }

        public ResultDataDTO GetResult(long resultId)
        {
            return ((ILeagueDBService)DbClient).GetResult(resultId);
        }

        public async Task<ResultDataDTO> GetResultAsync(long resultId)
        {
            if (!await StartUpdateWhenReady(UpdateKind.Loading))
                return null;
            var retVal = await ((ILeagueDBService)DbClient).GetResultAsync(resultId);
            EndUpdate();
            return retVal;
        }

        public IncidentReviewDataDTO GetReview(long reviewId)
        {
            return ((ILeagueDBService)DbClient).GetReview(reviewId);
        }

        public async Task<IncidentReviewDataDTO> GetReviewAsync(long reviewId)
        {
            if (!await StartUpdateWhenReady(UpdateKind.Loading))
                return null;
            var retVal = await ((ILeagueDBService)DbClient).GetReviewAsync(reviewId);
            EndUpdate();
            return retVal;
        }

        public ScheduleDataDTO GetSchedule(long scheduleId)
        {
            return ((ILeagueDBService)DbClient).GetSchedule(scheduleId);
        }

        public async Task<ScheduleDataDTO> GetScheduleAsync(long scheduleId)
        {
            if (!await StartUpdateWhenReady(UpdateKind.Loading))
                return null;
            var retVal = await ((ILeagueDBService)DbClient).GetScheduleAsync(scheduleId);
            EndUpdate();
            return retVal;
        }

        public ScheduleDataDTO[] GetSchedules(long[] scheduleIds)
        {
            return ((ILeagueDBService)DbClient).GetSchedules(scheduleIds);
        }

        public async Task<ScheduleDataDTO[]> GetSchedulesAsync(long[] scheduleIds)
        {
            if (!await StartUpdateWhenReady(UpdateKind.Loading))
                return new ScheduleDataDTO[0];
            var retVal = await ((ILeagueDBService)DbClient).GetSchedulesAsync(scheduleIds);
            EndUpdate();
            return retVal;
        }

        public SeasonDataDTO GetSeason(long seasonId)
        {
            return ((ILeagueDBService)DbClient).GetSeason(seasonId);
        }

        public async Task<SeasonDataDTO> GetSeasonAsync(long seasonId)
        {
            if (!await StartUpdateWhenReady(UpdateKind.Loading))
                return null;
            var retVal = await ((ILeagueDBService)DbClient).GetSeasonAsync(seasonId);
            EndUpdate();
            return retVal;
        }

        public SeasonDataDTO[] GetSeasons(long[] seasonIds)
        {
            return ((ILeagueDBService)DbClient).GetSeasons(seasonIds);
        }

        public async Task<SeasonDataDTO[]> GetSeasonsAsync(long[] seasonIds)
        {
            if (!await StartUpdateWhenReady(UpdateKind.Loading))
                return new SeasonDataDTO[0];
            var retVal = await ((ILeagueDBService)DbClient).GetSeasonsAsync(seasonIds);
            EndUpdate();
            return retVal;
        }

        public SessionDataDTO GetSession(long sessionId)
        {
            return ((ILeagueDBService)DbClient).GetSession(sessionId);
        }

        public async Task<SessionDataDTO> GetSessionAsync(long sessionId)
        {
            if (!await StartUpdateWhenReady(UpdateKind.Loading))
                return null;
            var retVal = await ((ILeagueDBService)DbClient).GetSessionAsync(sessionId);
            EndUpdate();
            return retVal;
        }

        public CommentDataDTO PutComment(ReviewCommentDataDTO comment)
        {
            return ((ILeagueDBService)DbClient).PutComment(comment);
        }

        public async Task<CommentDataDTO> PutCommentAsync(ReviewCommentDataDTO comment)
        {
            if (!await StartUpdateWhenReady(UpdateKind.Updating))
                return comment;
            var retVal = await ((ILeagueDBService)DbClient).PutCommentAsync(comment);
            EndUpdate();
            return retVal;
        }

        public LeagueMemberDataDTO PutMember(LeagueMemberDataDTO member)
        {
            return ((ILeagueDBService)DbClient).PutMember(member);
        }

        public async Task<LeagueMemberDataDTO> PutMemberAsync(LeagueMemberDataDTO member)
        {
            if (!await StartUpdateWhenReady(UpdateKind.Updating))
                return member;
            var retVal = await ((ILeagueDBService)DbClient).PutMemberAsync(member);
            EndUpdate();
            return retVal;
        }

        public ResultDataDTO PutResult(ResultDataDTO result)
        {
            return ((ILeagueDBService)DbClient).PutResult(result);
        }

        public async Task<ResultDataDTO> PutResultAsync(ResultDataDTO result)
        {
            if (!await StartUpdateWhenReady(UpdateKind.Updating))
                return result;
            var retVal = await ((ILeagueDBService)DbClient).PutResultAsync(result);
            EndUpdate();
            return retVal;
        }

        public IncidentReviewDataDTO PutReview(IncidentReviewDataDTO review)
        {
            return ((ILeagueDBService)DbClient).PutReview(review);
        }

        public async Task<IncidentReviewDataDTO> PutReviewAsync(IncidentReviewDataDTO review)
        {
            if (!await StartUpdateWhenReady(UpdateKind.Updating))
                return review;
            var retVal = await ((ILeagueDBService)DbClient).PutReviewAsync(review);
            EndUpdate();
            return retVal;
        }

        public ScheduleDataDTO PutSchedule(ScheduleDataDTO schedule)
        {
            return ((ILeagueDBService)DbClient).PutSchedule(schedule);
        }

        public async Task<ScheduleDataDTO> PutScheduleAsync(ScheduleDataDTO schedule)
        {
            if (!await StartUpdateWhenReady(UpdateKind.Updating))
                return schedule;
            var retVal = await ((ILeagueDBService)DbClient).PutScheduleAsync(schedule);
            EndUpdate();
            return retVal;
        }

        public SeasonDataDTO PutSeason(SeasonDataDTO season)
        {
            return ((ILeagueDBService)DbClient).PutSeason(season);
        }

        public async Task<SeasonDataDTO> PutSeasonAsync(SeasonDataDTO season)
        {
            if (!await StartUpdateWhenReady(UpdateKind.Updating))
                return season;
            var retVal = await ((ILeagueDBService)DbClient).PutSeasonAsync(season);
            EndUpdate();
            return retVal;
        }

        public SessionDataDTO PutSession(SessionDataDTO session)
        {
            return ((ILeagueDBService)DbClient).PutSession(session);
        }

        public async Task<SessionDataDTO> PutSessionAsync(SessionDataDTO session)
        {
            if (!await StartUpdateWhenReady(UpdateKind.Saving))
                return session;
            var retVal = await ((ILeagueDBService)DbClient).PutSessionAsync(session);
            EndUpdate();
            return retVal;
        }

        public LeagueMemberDataDTO[] UpdateMemberList(LeagueMemberDataDTO[] members)
        {
            return ((ILeagueDBService)DbClient).UpdateMemberList(members);
        }

        public async Task<LeagueMemberDataDTO[]> UpdateMemberListAsync(LeagueMemberDataDTO[] members)
        {
            if (!await StartUpdateWhenReady(UpdateKind.Updating))
                return members;
            var retVal = await ((ILeagueDBService)DbClient).UpdateMemberListAsync(members);
            EndUpdate();
            return retVal;
        }

        public string TestDB()
        {
            return ((ILeagueDBService)DbClient).TestDB();
        }

        public Task<string> TestDBAsync()
        {
            return ((ILeagueDBService)DbClient).TestDBAsync();
        }

        public string Test(string name)
        {
            return ((ILeagueDBService)DbClient).Test(name);
        }

        public Task<string> TestAsync(string name)
        {
            return ((ILeagueDBService)DbClient).TestAsync(name);
        }

        public StandingsRowDTO[] GetSeasonStandings(long seasonId, long? lastSessionId)
        {
            return ((ILeagueDBService)DbClient).GetSeasonStandings(seasonId, lastSessionId);
        }

        public Task<StandingsRowDTO[]> GetSeasonStandingsAsync(long seasonId, long? lastSessionId)
        {
            return ((ILeagueDBService)DbClient).GetSeasonStandingsAsync(seasonId, lastSessionId);
        }

        public StandingsRowDTO[] GetTeamStandings(long seasonId, long? lastSessionId)
        {
            return ((ILeagueDBService)DbClient).GetTeamStandings(seasonId, lastSessionId);
        }

        public Task<StandingsRowDTO[]> GetTeamStandingsAsync(long seasonId, long? lastSessionId)
        {
            return ((ILeagueDBService)DbClient).GetTeamStandingsAsync(seasonId, lastSessionId);
        }

        public void SetDatabaseName(string databaseName)
        {
            ((ILeagueDBService)DbClient).SetDatabaseName(databaseName);
        }

        public Task SetDatabaseNameAsync(string databaseName)
        {
            return ((ILeagueDBService)DbClient).SetDatabaseNameAsync(databaseName);
        }
    }
}
