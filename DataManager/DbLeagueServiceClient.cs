using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using System.ServiceModel;

using iRLeagueManager.LeagueDBServiceRef;
using iRLeagueManager.Models.Database;
using iRLeagueManager.Enums;
using iRLeagueManager.Interfaces;

namespace iRLeagueManager.Data
{
    public sealed class DbLeagueServiceClient : DbServiceClientBase, ILeagueDBService
    {
        private string EndpointConfigurationName { get; } = "";

        private string DatabaseName { get; set; }

        //private LeagueDBServiceClient DbClient
        //{
        //    get
        //    {
        //        if (ConnectionStatus == ConnectionStatusEnum.Disconnected)
        //            //Status.ConnectionStatus = Enums.ConnectionStatusEnum.Conecting;
        //            SetConnectionStatus(Token, ConnectionStatusEnum.Connecting);

        //        var retVal = (EndpointConfigurationName == "") ? new LeagueDBServiceClient() : new LeagueDBServiceClient(EndpointConfigurationName);

        //        if (ConnectionStatus == ConnectionStatusEnum.Connecting)
        //            //Status.ConnectionStatus = Enums.ConnectionStatusEnum.Connected;
        //            SetConnectionStatus(Token, ConnectionStatusEnum.Connected);

        //        return retVal;
        //    }
        //}

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
            using (var DbClient = new LeagueDBServiceClient())
            {
                base.SetDatabaseStatus(token, status, DbClient.Endpoint.Address.Uri.AbsoluteUri);
            }
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
            using (var DbClient = new LeagueDBServiceClient())
            {
                ((ILeagueDBService)DbClient).CleanUpSessions();
            }
        }

        public async Task CleanUpSessionsAsync()
        {
            using (var DbClient = new LeagueDBServiceClient())
            {
                await ClientCallAsync(() => ((ILeagueDBService)DbClient).CleanUpSessionsAsync(), UpdateKind.Saving);
            }
        }

        #region POST
        public TTarget Post<TTarget>(TTarget item) where TTarget : MappableDTO
        {
            return Post(new TTarget[] { item }).FirstOrDefault();
        }

        public TTarget[] Post<TTarget>(TTarget[] items) where TTarget : MappableDTO
        {
            POSTItemsRequestMessage requestMessage = new POSTItemsRequestMessage
            {
                databaseName = DatabaseName,
                userName = username,
                password = password,
                requestItemType = typeof(TTarget).Name,
                requestResponse = true,
                items = items,
            };

            return DatabasePOST(requestMessage).items.Cast<TTarget>().ToArray();
        }

        public async Task<TTarget> PostAsync<TTarget>(TTarget item) where TTarget : MappableDTO
        {
            return (await PostAsync(new TTarget[] { item })).FirstOrDefault();
        }

        public async Task<TTarget[]> PostAsync<TTarget>(TTarget[] items) where TTarget : MappableDTO
        {
            POSTItemsRequestMessage requestMessage = new POSTItemsRequestMessage
            {
                databaseName = DatabaseName,
                userName = username,
                password = password,
                requestItemType = typeof(TTarget).Name,
                requestResponse = true,
                items = items,
            };

            return await ClientGetAsync(async () => (await DatabasePOSTAsync(requestMessage)).items.Cast<TTarget>().ToArray(), UpdateKind.Saving);
        }
        #endregion

        #region GET
        public TTarget Get<TTarget>(long[] requestId) where TTarget : MappableDTO
        {
            return Get<TTarget>(new long[][] { requestId }).FirstOrDefault();
        }

        public TTarget[] Get<TTarget>(long[][] requestIds = null) where TTarget : MappableDTO
        {
            GETItemsRequestMessage requestMessage = new GETItemsRequestMessage
            {
                databaseName = DatabaseName,
                userName = username,
                password = password,
                requestItemType = typeof(TTarget).Name,
                requestResponse = true,
                requestItemIds = requestIds
            };

            return DatabaseGET(requestMessage).items.Cast<TTarget>().ToArray();
        }

        public async Task<TTarget> GetAsync<TTarget>(long[] requestId) where TTarget : MappableDTO
        {
            return (await GetAsync<TTarget>(new long[][] { requestId })).FirstOrDefault();
        }

        public async Task<TTarget[]> GetAsync<TTarget>(long[][] requestIds = null) where TTarget : MappableDTO
        {
            GETItemsRequestMessage requestMessage = new GETItemsRequestMessage
            {
                databaseName = DatabaseName,
                userName = username,
                password = password,
                requestItemType = typeof(TTarget).Name,
                requestResponse = true,
                requestItemIds = requestIds
            };

            return await ClientGetAsync(async () => (await DatabaseGETAsync(requestMessage)).items.Cast<TTarget>().ToArray(), UpdateKind.Loading);
        }
        #endregion

        #region PUT
        public TTarget Put<TTarget>(TTarget item) where TTarget : MappableDTO
        {
            return Put(new TTarget[] { item }).FirstOrDefault();
        }

        public TTarget[] Put<TTarget>(TTarget[] items) where TTarget : MappableDTO
        {
            PUTItemsRequestMessage requestMessage = new PUTItemsRequestMessage
            {
                databaseName = DatabaseName,
                userName = username,
                password = password,
                requestItemType = typeof(TTarget).Name,
                requestResponse = true,
                items = items,
            };

            return DatabasePUT(requestMessage).items.Cast<TTarget>().ToArray();
        }

        public async Task<TTarget> PutAsync<TTarget>(TTarget item) where TTarget : MappableDTO
        {
            return (await PutAsync(new TTarget[] { item })).FirstOrDefault();
        }

        public async Task<TTarget[]> PutAsync<TTarget>(TTarget[] items) where TTarget : MappableDTO
        {
            PUTItemsRequestMessage requestMessage = new PUTItemsRequestMessage
            {
                databaseName = DatabaseName,
                userName = username,
                password = password,
                requestItemType = typeof(TTarget).Name,
                requestResponse = true,
                items = items,
            };

            return await ClientGetAsync(async () => (await DatabasePUTAsync(requestMessage)).items.Cast<TTarget>().ToArray(), UpdateKind.Saving);
        }

        public async Task<object[]> PutAsync(object[] items, Type type)
        {
            if (!type.IsSubclassOf(typeof(MappableDTO)))
            {
                throw new ArgumentException("Could not finish Database request. Requestet type is not inherited from MappableDTO.");
            }

            PUTItemsRequestMessage requestMessage = new PUTItemsRequestMessage
            {
                databaseName = DatabaseName,
                userName = username,
                password = password,
                requestItemType = type.Name,
                requestResponse = true,
                items = items.Select(x => x as MappableDTO).ToArray()
            };

            return await ClientGetAsync(async () => (await DatabasePUTAsync(requestMessage)).items.ToArray(), UpdateKind.Saving);
        }
        #endregion

        #region DEL
        public bool Del<TTarget>(long[] requestId) where TTarget : MappableDTO
        {
            return Del<TTarget>(new long[][] { requestId });
        }

        public bool Del<TTarget>(long[][] requestIds) where TTarget : MappableDTO
        {
            DELItemsRequestMessage requestMessage = new DELItemsRequestMessage
            {
                databaseName = DatabaseName,
                userName = username,
                password = password,
                requestItemType = typeof(TTarget).Name,
                requestResponse = true,
                requestItemIds = requestIds
            };

            return DatabaseDEL(requestMessage).success;
        }

        public async Task<bool> DelAsync<TTarget>(long[] requestId) where TTarget : MappableDTO
        {
            return await DelAsync<TTarget>(new long[][] { requestId });
        }

        public async Task<bool> DelAsync<TTarget>(long[][] requestIds) where TTarget : MappableDTO
        {
            DELItemsRequestMessage requestMessage = new DELItemsRequestMessage
            {
                databaseName = DatabaseName,
                userName = username,
                password = password,
                requestItemType = typeof(TTarget).Name,
                requestResponse = true,
                requestItemIds = requestIds
            };

            return (await ClientGetAsync(async () => await DatabaseDELAsync(requestMessage), UpdateKind.Saving)).success;
        }
        #endregion

        //public CommentDataDTO GetComment(long commentId)
        //{
        //    return ((ILeagueDBService)DbClient).GetComment(commentId);
        //}

        //public async Task<CommentDataDTO> GetCommentAsync(long commentId)
        //{
        //    return await ClientGetAsync(commentId, x => ((ILeagueDBService)DbClient).GetCommentAsync(x), UpdateKind.Loading);
        //}

        //public LeagueMemberDataDTO GetLastMember()
        //{
        //    return ((ILeagueDBService)DbClient).GetLastMember();
        //}

        //public async Task<LeagueMemberDataDTO> GetLastMemberAsync()
        //{
        //    return await ClientGetAsync(() => ((ILeagueDBService)DbClient).GetLastMemberAsync(), UpdateKind.Loading);
        //}

        //public LeagueMemberDataDTO GetMember(long memberId)
        //{
        //    return ((ILeagueDBService)DbClient).GetMember(memberId);
        //}

        //public async Task<LeagueMemberDataDTO> GetMemberAsync(long memberId)
        //{
        //    return await ClientGetAsync(memberId, x => ((ILeagueDBService)DbClient).GetMemberAsync(x), UpdateKind.Loading);
        //}

        //public LeagueMemberDataDTO[] GetMembers(long[] memberId)
        //{
        //    return ((ILeagueDBService)DbClient).GetMembers(memberId);
        //}

        //public async Task<LeagueMemberDataDTO[]> GetMembersAsync(long[] memberId = null)
        //{
        //    return await ClientGetAsync(memberId, x => ((ILeagueDBService)DbClient).GetMembersAsync(x), UpdateKind.Loading);
        //}

        //public ResultDataDTO GetResult(long resultId)
        //{
        //    return ((ILeagueDBService)DbClient).GetResult(resultId);
        //}

        //public async Task<ResultDataDTO> GetResultAsync(long resultId)
        //{
        //    return await ClientGetAsync(resultId, x => ((ILeagueDBService)DbClient).GetResultAsync(x), UpdateKind.Loading);
        //}

        //public IncidentReviewDataDTO GetReview(long reviewId)
        //{
        //    return ((ILeagueDBService)DbClient).GetReview(reviewId);
        //}

        //public async Task<IncidentReviewDataDTO> GetReviewAsync(long reviewId)
        //{
        //    return await ClientGetAsync(reviewId, x => ((ILeagueDBService)DbClient).GetReviewAsync(x), UpdateKind.Loading);
        //}

        //public ScheduleDataDTO GetSchedule(long scheduleId)
        //{
        //    return ((ILeagueDBService)DbClient).GetSchedule(scheduleId);
        //}

        //public async Task<ScheduleDataDTO> GetScheduleAsync(long scheduleId)
        //{
        //    return await ClientGetAsync(scheduleId, x => ((ILeagueDBService)DbClient).GetScheduleAsync(x), UpdateKind.Loading);
        //}

        //public ScheduleDataDTO[] GetSchedules(long[] scheduleIds)
        //{
        //    return ((ILeagueDBService)DbClient).GetSchedules(scheduleIds);
        //}

        //public async Task<ScheduleDataDTO[]> GetSchedulesAsync(long[] scheduleIds)
        //{
        //    return await ClientGetAsync(scheduleIds, x => ((ILeagueDBService)DbClient).GetSchedulesAsync(x), UpdateKind.Loading);
        //}

        //public SeasonDataDTO GetSeason(long seasonId)
        //{
        //    return ((ILeagueDBService)DbClient).GetSeason(seasonId);
        //}

        //public async Task<SeasonDataDTO> GetSeasonAsync(long seasonId)
        //{

        //    return await ClientGetAsync(seasonId, x => ((ILeagueDBService)DbClient).GetSeasonAsync(x), UpdateKind.Loading);
        //}

        //public SeasonDataDTO[] GetSeasons(long[] seasonIds)
        //{
        //    return ((ILeagueDBService)DbClient).GetSeasons(seasonIds);
        //}

        //public async Task<SeasonDataDTO[]> GetSeasonsAsync(long[] seasonIds)
        //{
        //    return await ClientGetAsync(seasonIds, x => ((ILeagueDBService)DbClient).GetSeasonsAsync(x), UpdateKind.Loading);
        //}

        //public SessionDataDTO GetSession(long sessionId)
        //{
        //    return ((ILeagueDBService)DbClient).GetSession(sessionId);
        //}

        //public async Task<SessionDataDTO> GetSessionAsync(long sessionId)
        //{
        //    return await ClientGetAsync(sessionId, x => ((ILeagueDBService)DbClient).GetSessionAsync(x), UpdateKind.Loading);
        //}

        //public CommentDataDTO PutComment(ReviewCommentDataDTO comment)
        //{
        //    return ((ILeagueDBService)DbClient).PutComment(comment);
        //}

        //public async Task<CommentDataDTO> PutCommentAsync(ReviewCommentDataDTO comment)
        //{
        //    return await ClientGetAsync(comment, x => ((ILeagueDBService)DbClient).PutCommentAsync(x), UpdateKind.Updating, comment);
        //}

        //public LeagueMemberDataDTO PutMember(LeagueMemberDataDTO member)
        //{
        //    return ((ILeagueDBService)DbClient).PutMember(member);
        //}

        //public async Task<LeagueMemberDataDTO> PutMemberAsync(LeagueMemberDataDTO member)
        //{
        //    return await ClientGetAsync(member, x => ((ILeagueDBService)DbClient).PutMemberAsync(x), UpdateKind.Updating, member);
        //}

        //public ResultDataDTO PutResult(ResultDataDTO result)
        //{
        //    return ((ILeagueDBService)DbClient).PutResult(result);
        //}

        //public async Task<ResultDataDTO> PutResultAsync(ResultDataDTO result)
        //{
        //    return await ClientGetAsync(result, x => ((ILeagueDBService)DbClient).PutResultAsync(x), UpdateKind.Updating, result);
        //}

        //public IncidentReviewDataDTO PutReview(IncidentReviewDataDTO review)
        //{
        //    return ((ILeagueDBService)DbClient).PutReview(review);
        //}

        //public async Task<IncidentReviewDataDTO> PutReviewAsync(IncidentReviewDataDTO review)
        //{
        //    return await ClientGetAsync(review, x => ((ILeagueDBService)DbClient).PutReviewAsync(x), UpdateKind.Updating, review);
        //}

        //public ScheduleDataDTO PutSchedule(ScheduleDataDTO schedule)
        //{
        //    return ((ILeagueDBService)DbClient).PutSchedule(schedule);
        //}

        //public async Task<ScheduleDataDTO> PutScheduleAsync(ScheduleDataDTO schedule)
        //{
        //    return await ClientGetAsync(schedule, x => ((ILeagueDBService)DbClient).PutScheduleAsync(x), UpdateKind.Updating, schedule);
        //}

        //public SeasonDataDTO PutSeason(SeasonDataDTO season)
        //{
        //    return ((ILeagueDBService)DbClient).PutSeason(season);
        //}

        //public async Task<SeasonDataDTO> PutSeasonAsync(SeasonDataDTO season)
        //{
        //    return await ClientGetAsync(season, x => ((ILeagueDBService)DbClient).PutSeasonAsync(x), UpdateKind.Updating, season);
        //}

        //public SessionDataDTO PutSession(SessionDataDTO session)
        //{
        //    return ((ILeagueDBService)DbClient).PutSession(session);
        //}

        //public async Task<SessionDataDTO> PutSessionAsync(SessionDataDTO session)
        //{
        //    return await ClientGetAsync(session, x => ((ILeagueDBService)DbClient).PutSessionAsync(x), UpdateKind.Updating, session);
        //}

        //public LeagueMemberDataDTO[] UpdateMemberList(LeagueMemberDataDTO[] members)
        //{
        //    return ((ILeagueDBService)DbClient).UpdateMemberList(members);
        //}

        //public async Task<LeagueMemberDataDTO[]> UpdateMemberListAsync(LeagueMemberDataDTO[] members)
        //{
        //    return await ClientGetAsync(members, x => ((ILeagueDBService)DbClient).UpdateMemberListAsync(x), UpdateKind.Updating, members);
        //}

        public string TestDB()
        {
            using (var DbClient = new LeagueDBServiceClient())
            {
                return ((ILeagueDBService)DbClient).TestDB();
            }
        }

        public async Task<string> TestDBAsync()
        {
            using (var DbClient = new LeagueDBServiceClient())
            {
                return await ClientGetAsync(() => ((ILeagueDBService)DbClient).TestDBAsync(), UpdateKind.Loading);
            }
        }

        public string Test(string name)
        {
            using (var DbClient = new LeagueDBServiceClient())
            {
                return ((ILeagueDBService)DbClient).Test(name);
            }
        }

        public async Task<string> TestAsync(string name)
        {
            using (var DbClient = new LeagueDBServiceClient())
            {
                return await ClientGetAsync(name, x => ((ILeagueDBService)DbClient).TestAsync(x), UpdateKind.Loading);
            }
        }

        //public StandingsRowDTO[] GetSeasonStandings(long seasonId, long? lastSessionId)
        //{
        //    return ((ILeagueDBService)DbClient).GetSeasonStandings(seasonId, lastSessionId);
        //}

        //public async Task<StandingsRowDTO[]> GetSeasonStandingsAsync(long seasonId, long? lastSessionId)
        //{
        //    return await ClientGetAsync(new { season = seasonId, lastSession = lastSessionId }, x => ((ILeagueDBService)DbClient).GetSeasonStandingsAsync(x.season, x.lastSession), UpdateKind.Loading);
        //}

        //public StandingsRowDTO[] GetTeamStandings(long seasonId, long? lastSessionId)
        //{
        //    return ((ILeagueDBService)DbClient).GetTeamStandings(seasonId, lastSessionId);
        //}

        //public async Task<StandingsRowDTO[]> GetTeamStandingsAsync(long seasonId, long? lastSessionId)
        //{
        //    return await ClientGetAsync(new { season = seasonId, lastSession = lastSessionId }, x => ((ILeagueDBService)DbClient).GetTeamStandingsAsync(x.season, x.lastSession), UpdateKind.Loading);
        //}

        public void SetDatabaseName(string databaseName)
        {
            DatabaseName = databaseName;
        }

        public async Task SetDatabaseNameAsync(string databaseName)
        {
            await ClientCallAsync(() => new Task(() => { DatabaseName = databaseName; }), UpdateKind.Saving);
        }

        //public ScoringDataDTO GetScoring(long scoringId)
        //{
        //    return ((ILeagueDBService)DbClient).GetScoring(scoringId);
        //}

        //public async Task<ScoringDataDTO> GetScoringAsync(long scoringId)
        //{
        //    return await ClientGetAsync(scoringId, x => ((ILeagueDBService)DbClient).GetScoringAsync(x), UpdateKind.Loading);
        //}

        //public ScoringDataDTO PutScoring(ScoringDataDTO scoring)
        //{
        //    return ((ILeagueDBService)DbClient).PutScoring(scoring);
        //}

        //public async Task<ScoringDataDTO> PutScoringAsync(ScoringDataDTO scoring)
        //{
        //    return await ClientGetAsync(scoring, x => ((ILeagueDBService)DbClient).PutScoringAsync(x), UpdateKind.Updating, scoring);
        //}

        //public ScoredResultDataDTO GetScoredResult(long sessionId, long scoringId)
        //{
        //    return ((ILeagueDBService)DbClient).GetScoredResult(sessionId, scoringId);
        //}

        //public async Task<ScoredResultDataDTO> GetScoredResultAsync(long sessionId, long scoringId)
        //{
        //    return await ClientGetAsync(new { sessionId, scoringId }, x => ((ILeagueDBService)DbClient).GetScoredResultAsync(x.sessionId, x.scoringId), UpdateKind.Loading);
        //}

        public void CalculateScoredResults(long sessionId)
        {
            using (var DbClient = new LeagueDBServiceClient())
            {
                ((ILeagueDBService)DbClient).CalculateScoredResults(sessionId);
            }
        }

        public async Task CalculateScoredResultsAsync(long sessionId)
        {
            using (var DbClient = new LeagueDBServiceClient())
            {
                await ClientCallAsync(sessionId, x => ((ILeagueDBService)DbClient).CalculateScoredResultsAsync(x), UpdateKind.Saving);
            }
        }

        //public GetItemsResponse MessageTest(GetItemsRequest request)
        //{
        //    return ((ILeagueDBService)DbClient).MessageTest(request);
        //}

        //public Task<GetItemsResponse> MessageTestAsync(GetItemsRequest request)
        //{
        //    return ((ILeagueDBService)DbClient).MessageTestAsync(request);
        //}

        //public GetItemsResponse GetFromDatabase(GetItemsRequest request)
        //{
        //    DbClient.Open();
        //    var retVal = ((ILeagueDBService)DbClient).GetFromDatabase(request);
        //    DbClient.Close();
        //    return retVal;
        //}

        //public Task<GetItemsResponse> GetFromDatabaseAsync(GetItemsRequest request)
        //{
        //    Task<GetItemsResponse> retVal = null;
        //    using (var client = DbClient)
        //    {
        //        retVal = ((ILeagueDBService)DbClient).GetFromDatabaseAsync(request);
        //    }
        //    return retVal;
        //}

        private void SetMessageHeader(RequestMessage message)
        {
            message.userName = username;
            message.password = password;
        }

        public ResponseMessage MessageTest(RequestMessage request)
        {
            using (var DbClient = new LeagueDBServiceClient())
            {
                return ((ILeagueDBService)DbClient).MessageTest(request);
            }
        }

        public Task<ResponseMessage> MessageTestAsync(RequestMessage request)
        {
            using (var DbClient = new LeagueDBServiceClient())
            {
                return ((ILeagueDBService)DbClient).MessageTestAsync(request);
            }
        }

        public POSTItemsResponseMessage DatabasePOST(POSTItemsRequestMessage request)
        {
            using (var DbClient = new LeagueDBServiceClient())
            {
                return ((ILeagueDBService)DbClient).DatabasePOST(request);
            }
        }

        public Task<POSTItemsResponseMessage> DatabasePOSTAsync(POSTItemsRequestMessage request)
        {
            using (var DbClient = new LeagueDBServiceClient())
            {
                return ((ILeagueDBService)DbClient).DatabasePOSTAsync(request);
            }
        }

        public GETItemsResponseMessage DatabaseGET(GETItemsRequestMessage request)
        {
            using (var DbClient = new LeagueDBServiceClient())
            {
                return ((ILeagueDBService)DbClient).DatabaseGET(request);
            }
        }

        public async Task<GETItemsResponseMessage> DatabaseGETAsync(GETItemsRequestMessage request)
        {
            using (var DbClient = new LeagueDBServiceClient())
            {
                return await ((ILeagueDBService)DbClient).DatabaseGETAsync(request).ConfigureAwait(false);
            }
        }

        public PUTItemsResponseMessage DatabasePUT(PUTItemsRequestMessage request)
        {
            using (var DbClient = new LeagueDBServiceClient())
            {
                return ((ILeagueDBService)DbClient).DatabasePUT(request);
            }
        }

        public async Task<PUTItemsResponseMessage> DatabasePUTAsync(PUTItemsRequestMessage request)
        {
            using (var DbClient = new LeagueDBServiceClient())
            {
                return await ((ILeagueDBService)DbClient).DatabasePUTAsync(request).ConfigureAwait(false);
            }
        }

        public DELItemsResponseMessage DatabaseDEL(DELItemsRequestMessage request)
        {
            using (var DbClient = new LeagueDBServiceClient())
            {
                return ((ILeagueDBService)DbClient).DatabaseDEL(request);
            }
        }

        public async Task<DELItemsResponseMessage> DatabaseDELAsync(DELItemsRequestMessage request)
        {
            using (var DbClient = new LeagueDBServiceClient())
            {
                return await ((ILeagueDBService)DbClient).DatabaseDELAsync(request).ConfigureAwait(false);
            }
        }
    }
}

namespace iRLeagueManager.LeagueDBServiceRef
{
    partial class LeagueDBServiceClient : IDisposable
    {

        #region IDisposable Support
        private bool disposedValue = false; // Dient zur Erkennung redundanter Aufrufe.

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    try
                    {
                        if (State == CommunicationState.Faulted)
                            Abort();
                        else
                        {
                            try
                            {
                                Close();
                            }
                            catch (Exception closeException)
                            {
                                try
                                {
                                    Abort();
                                }
                                catch (Exception abortException)
                                {
                                    throw new AggregateException(closeException, abortException);
                                }
                                throw;
                            }
                        }
                    }
                    finally
                    {
                        disposedValue = true;
                    }
                }

                // TODO: nicht verwaltete Ressourcen (nicht verwaltete Objekte) freigeben und Finalizer weiter unten überschreiben.
                // TODO: große Felder auf Null setzen.

                disposedValue = true;
            }
        }

        // TODO: Finalizer nur überschreiben, wenn Dispose(bool disposing) weiter oben Code für die Freigabe nicht verwalteter Ressourcen enthält.
        // ~LeagueDBService()
        // {
        //   // Ändern Sie diesen Code nicht. Fügen Sie Bereinigungscode in Dispose(bool disposing) weiter oben ein.
        //   Dispose(false);
        // }

        // Dieser Code wird hinzugefügt, um das Dispose-Muster richtig zu implementieren.
        public void Dispose()
        {
            // Ändern Sie diesen Code nicht. Fügen Sie Bereinigungscode in Dispose(bool disposing) weiter oben ein.
            Dispose(true);
            // TODO: Auskommentierung der folgenden Zeile aufheben, wenn der Finalizer weiter oben überschrieben wird.
            // GC.SuppressFinalize(this);
        }
        #endregion

    }
}
