using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.ComponentModel;
using System.Xml.Serialization;
using System.Collections.ObjectModel;
using iRLeagueManager.Interfaces;
using iRLeagueManager.Enums;
using iRLeagueManager.Models.Sessions;
using iRLeagueManager.Models.Results;
using iRLeagueManager.Models.Reviews;

namespace iRLeagueManager.Models
{
    public class SeasonModel : SeasonInfo, IHierarchicalModel //, ISeason
    {        
        private ObservableCollection<ScheduleInfo> schedules;
        public ObservableCollection<ScheduleInfo> Schedules
        {
            get => schedules;
            internal set => SetNotifyCollection(ref schedules, value);
        }
        //ReadOnlyObservableCollection<IScheduleInfo> ISeason.Schedules => new ReadOnlyObservableCollection<IScheduleInfo>(Schedules);

        private ObservableCollection<ScoringModel> scorings;
        public ObservableCollection<ScoringModel> Scorings { get => scorings; internal set => SetNotifyCollection(ref scorings, value); }
        //ReadOnlyObservableCollection<IScoringInfo> ISeason.Scorings => new ReadOnlyObservableCollection<IScoringInfo>(Scorings);

        private ObservableCollection<ScoringTableModel> scoringTables;
        public ObservableCollection<ScoringTableModel> ScoringTables { get => scoringTables; internal set => SetNotifyCollection(ref scoringTables, value); }

        private long? mainScoringId;
        public ScoringModel MainScoring
        {
            get => Scorings.SingleOrDefault(x => x.ScoringId == mainScoringId);
            set {
                if (Scorings.Any(x => x.ScoringId == value.ScoringId))
                {
                    var index = Scorings.IndexOf(Scorings.SingleOrDefault(x => x.ScoringId == value.ScoringId));
                    Scorings[index] = value;
                }
                else
                {
                    Scorings.Add(value);
                }
                SetValue(ref mainScoringId, value.ScoringId);
            }
        }

        //private ObservableCollection<IncidentReviewInfo> reviews;
        //public ObservableCollection<IncidentReviewInfo> Reviews { get => reviews; internal set => SetNotifyCollection(ref reviews, value); }
        //ReadOnlyObservableCollection<IncidentReview> ISeason.Reviews => new ReadOnlyObservableCollection<IReviewInfo>(Reviews);

        //private ObservableCollection<ResultInfo> results;
        //public ObservableCollection<ResultInfo> Results { get => results; internal set => SetNotifyCollection(ref results, value); }

        string IHierarchicalModel.Description => SeasonName;

        private DateTime seasonStart;
        public DateTime SeasonStart { get => seasonStart; internal set => SetValue(ref seasonStart, value); }

        private DateTime seasonEnd;
        public DateTime SeasonEnd { get => seasonEnd; internal set => SetValue(ref seasonEnd, value); }

        IEnumerable<object> IHierarchicalModel.Children => new List<IEnumerable<object>> { Schedules.Cast<object>() };

        public IEnumerable<ResultModel> GetResults()
        {
            return null;
        }
        // client.Results.Where(x => Sessions.Select(y => y.SessionId).Contains(x.SessionId));

        //public IEnumerable<ISession> Sessions => Schedules.Select(x => x.Sessions.AsEnumerable()).Aggregate((x, y) => x.Concat(y));

        //public int SessionCount => Sessions.Count();

        //public ISessionInfo LastSession => Sessions.OrderBy(x => x.Date).Last();

        //public IRaceInfo LastRace => Sessions.Where(x => x.SessionType == SessionType.Race).OrderBy(x => x.Date).Last() as IRaceInfo;

        public SeasonModel()
        {
            Schedules = new ObservableCollection<ScheduleInfo>();
            Scorings = new ObservableCollection<ScoringModel>();
            //Results = new ObservableCollection<ResultInfo>();
            //Reviews = new ObservableCollection<IncidentReviewInfo>();
            //Scorings = new ObservableCollection<IScoringInfo>();
        }

        public SeasonModel(long? seasonId) : this()
        {
            SeasonId = seasonId;
        }

        //public IEnumerable<SessionInfo> GetSessions()
        //{
        //    return Schedules.Select(x => x.Sessions.AsEnumerable()).Aggregate((x, y) => x.Concat(y));
        //}

        //public int GetSessionCount()
        //{
        //    return GetSessions().Count();
        //}

        //public SessionInfo GetLastSession()
        //{
        //    return GetSessions().OrderBy(x => x.Date).Last();
        //}

        //internal override void InitReset()
        //{
        //    foreach (var schedule in Schedules)
        //    {
        //        schedule.Season = this;
        //        schedule.InitReset();
        //    }

        //    foreach (var result in Results)
        //    {
        //        result.InitReset();
        //    }

        //    foreach (var review in Reviews)
        //    {
        //        review.InitReset();
        //    }
        //    base.InitReset();
        //}

        internal override void InitializeModel()
        {
            if (!isInitialized)
            {
                foreach (var schedule in Schedules)
                {
                    //schedule.Season = this;
                    schedule.InitializeModel();
                }
                foreach (var scoring in Scorings)
                {
                    scoring.Season = this;
                    scoring.InitializeModel();
                }
                foreach (var scoringTable in ScoringTables)
                {
                    for (int i = 0; i < scoringTable.Scorings.Count(); i++)
                    {
                        var scoring = Scorings.SingleOrDefault(x => x.ScoringId == scoringTable.Scorings.ElementAt(i).Key.ScoringId);
                        if (scoring != null)
                        {
                            scoringTable.Scorings.ElementAt(i).Key = scoring;
                        }
                    }
                }

                //foreach (var result in Results)
                //{
                //    result.InitializeModel();
                //}

                //foreach (var review in Reviews)
                //{
                //    review.InitializeModel();
                //}
            }
            base.InitializeModel();
        }

        public static SeasonModel GetTemplate()
        {
            return new SeasonModel()
            {
                SeasonId = 0,
                SeasonName = "Season Name"
            };
        }

        public static SeasonModel GetTemplate(SeasonModel seasonInfo)
        {
            return new SeasonModel(seasonInfo.SeasonId)
            {
                SeasonName = seasonInfo.SeasonName
            };
        }

        //public IRaceSession GetLastRace()
        //{
        //    return GetSessions().Where(x => x.SessionType == SessionType.Race).OrderBy(x => x.Date).Last() as IRaceSession;
        //}

        //internal void SetLeagueClient(IRLeagueClient leagueClient)
        //{
        //    client = leagueClient;
        //    // Error handling für nicht vorhandene Id
        //    Schedules = Schedules.Select(x => client.Schedules.Single(y => y.ScheduleId == x.ScheduleId)).ToList();
        //    Scorings.ForEach(x => ((Scoring)x).SetLeagueClient(client));
        //}

        //public IResult AddNewResult()
        //{
        //    Result result = new Result(client.GetNewResultId());
        //    result.SetLeagueClient(client);
        //    //result.Session = Sessions.SingleOrDefault(x => x.SessionId == result.SessionId).GetSourceObject() as SessionBase;
        //    client.Results.Add(result);
        //    return result;
        //}

        //public ISchedule AddNewSchedule()
        //{
        //    return client.AddSchedule();
        //}

        //public IScoring AddNewScoring()
        //{
        //    throw new NotImplementedException();
        //}

        //public ISession AddNewSession()
        //{
        //    throw new NotImplementedException();
        //}

        //public void AddSession(ISession session)
        //{
        //    throw new NotImplementedException();
        //}

        //public void RemoveSession(ISession session)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
