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


        public void CleanUpSessions()
        {
            ((ILeagueDBService)DbClient).CleanUpSessions();
        }

        public async Task CleanUpSessionsAsync()
        {
            await StartUpdateWhenReady(UpdateKind.Saving);
            await ((ILeagueDBService)DbClient).CleanUpSessionsAsync();
            EndUpdate();
        }

        public CommentDataDTO GetComment(int commentId)
        {
            return ((ILeagueDBService)DbClient).GetComment(commentId);
        }

        public async Task<CommentDataDTO> GetCommentAsync(int commentId)
        {
            await StartUpdateWhenReady(UpdateKind.Loading);
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
            await StartUpdateWhenReady(UpdateKind.Loading);
            var retVal = await ((ILeagueDBService)DbClient).GetLastMemberAsync();
            EndUpdate();
            return retVal;
        }

        public LeagueMemberDataDTO GetMember(int memberId)
        {
            return ((ILeagueDBService)DbClient).GetMember(memberId);
        }

        public async Task<LeagueMemberDataDTO> GetMemberAsync(int memberId)
        {
            await StartUpdateWhenReady(UpdateKind.Loading);
            var retVal = await ((ILeagueDBService)DbClient).GetMemberAsync(memberId);
            EndUpdate();
            return retVal;
        }

        public LeagueMemberDataDTO[] GetMembers(int[] memberId)
        {
            return ((ILeagueDBService)DbClient).GetMembers(memberId);
        }

        public async Task<LeagueMemberDataDTO[]> GetMembersAsync(int[] memberId)
        {
            await StartUpdateWhenReady(UpdateKind.Loading);
            var retVal = await ((ILeagueDBService)DbClient).GetMembersAsync(memberId);
            EndUpdate();
            return retVal;
        }

        public ResultDataDTO GetResult(int resultId)
        {
            return ((ILeagueDBService)DbClient).GetResult(resultId);
        }

        public async Task<ResultDataDTO> GetResultAsync(int resultId)
        {
            await StartUpdateWhenReady(UpdateKind.Loading);
            var retVal = await ((ILeagueDBService)DbClient).GetResultAsync(resultId);
            EndUpdate();
            return retVal;
        }

        public IncidentReviewDataDTO GetReview(int reviewId)
        {
            return ((ILeagueDBService)DbClient).GetReview(reviewId);
        }

        public async Task<IncidentReviewDataDTO> GetReviewAsync(int reviewId)
        {
            await StartUpdateWhenReady(UpdateKind.Loading);
            var retVal = await ((ILeagueDBService)DbClient).GetReviewAsync(reviewId);
            EndUpdate();
            return retVal;
        }

        public ScheduleDataDTO GetSchedule(int scheduleId)
        {
            return ((ILeagueDBService)DbClient).GetSchedule(scheduleId);
        }

        public async Task<ScheduleDataDTO> GetScheduleAsync(int scheduleId)
        {
            await StartUpdateWhenReady(UpdateKind.Loading);
            var retVal = await ((ILeagueDBService)DbClient).GetScheduleAsync(scheduleId);
            EndUpdate();
            return retVal;
        }

        public ScheduleDataDTO[] GetSchedules(int[] scheduleIds)
        {
            return ((ILeagueDBService)DbClient).GetSchedules(scheduleIds);
        }

        public async Task<ScheduleDataDTO[]> GetSchedulesAsync(int[] scheduleIds)
        {
            await StartUpdateWhenReady(UpdateKind.Loading);
            var retVal = await ((ILeagueDBService)DbClient).GetSchedulesAsync(scheduleIds);
            EndUpdate();
            return retVal;
        }

        public SeasonDataDTO GetSeason(int seasonId)
        {
            return ((ILeagueDBService)DbClient).GetSeason(seasonId);
        }

        public async Task<SeasonDataDTO> GetSeasonAsync(int seasonId)
        {
            await StartUpdateWhenReady(UpdateKind.Loading);
            var retVal = await ((ILeagueDBService)DbClient).GetSeasonAsync(seasonId);
            EndUpdate();
            return retVal;
        }

        public SeasonDataDTO[] GetSeasons(int[] seasonIds)
        {
            return ((ILeagueDBService)DbClient).GetSeasons(seasonIds);
        }

        public async Task<SeasonDataDTO[]> GetSeasonsAsync(int[] seasonIds)
        {
            await StartUpdateWhenReady(UpdateKind.Loading);
            var retVal = await ((ILeagueDBService)DbClient).GetSeasonsAsync(seasonIds);
            EndUpdate();
            return retVal;
        }

        public SessionDataDTO GetSession(int sessionId)
        {
            return ((ILeagueDBService)DbClient).GetSession(sessionId);
        }

        public async Task<SessionDataDTO> GetSessionAsync(int sessionId)
        {
            await StartUpdateWhenReady(UpdateKind.Loading);
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
            await StartUpdateWhenReady(UpdateKind.Updating);
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
            await StartUpdateWhenReady(UpdateKind.Updating);
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
            await StartUpdateWhenReady(UpdateKind.Updating);
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
            await StartUpdateWhenReady(UpdateKind.Updating);
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
            await StartUpdateWhenReady(UpdateKind.Saving);
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
            await StartUpdateWhenReady(UpdateKind.Saving);
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
            await StartUpdateWhenReady(UpdateKind.Saving);
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
            await StartUpdateWhenReady(UpdateKind.Updating);
            var retVal = await ((ILeagueDBService)DbClient).UpdateMemberListAsync(members);
            EndUpdate();
            return retVal;
        }
    }
}
