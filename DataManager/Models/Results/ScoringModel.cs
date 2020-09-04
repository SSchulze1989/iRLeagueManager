using iRLeagueManager.Exceptions;
using iRLeagueManager.Models.Sessions;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using iRLeagueManager.Enums;

namespace iRLeagueManager.Models.Results
{
    public class ScoringModel : ScoringInfo
    {
        private string name;
        public string Name { get => name; set => SetValue(ref name, value); }

        private int dropWeeks;
        public int DropWeeks { get => dropWeeks; set => SetValue(ref dropWeeks, value); }

        private int averageRaceNr;
        public int AverageRaceNr { get => averageRaceNr; set => SetValue(ref averageRaceNr, value); }

        private ScoringKindEnum scoringKind;
        public ScoringKindEnum ScoringKind { get => scoringKind; set => SetValue(ref scoringKind, value); }

        private int maxResultsPerGroup;
        public int MaxResultsPerGroup { get => maxResultsPerGroup; set => SetValue(ref maxResultsPerGroup, value); }

        private bool takeGroupAverage;
        public bool TakeGroupAverage { get => takeGroupAverage; set => SetValue(ref takeGroupAverage, value); }

        private ObservableCollection<SessionInfo> sessions;
        public ObservableCollection<SessionInfo> Sessions { get => sessions; set => SetNotifyCollection(ref sessions, value); }

        //private ObservableCollection<ResultInfo> results;
        //public ObservableCollection<ResultInfo> Results { get => results; set => SetNotifyCollection(ref results, value); }

        //private long seasonId;
        //public long SeasonId { get => seasonId; set => SetValue(ref seasonId, value); }
        public long SeasonId => (Season?.SeasonId).GetValueOrDefault();

        private SeasonModel season;
        public SeasonModel Season { get => season; set => SetValue(ref season, value); }

        private ObservableCollection<BasePointsValue> basePoints;
        //public string BasePoints { get => basePoints; set => SetValue(ref basePoints, value); }
        public ObservableCollection<BasePointsValue> BasePoints { get => basePoints; set => SetNotifyCollection(ref basePoints, value); }

        private ObservableCollection<BonusPointsValue> bonusPoints;
        public ObservableCollection<BonusPointsValue> BonusPoints { get => bonusPoints; set => SetNotifyCollection(ref bonusPoints, value); }

        private ObservableCollection<IncidentPointsValue> incPenaltyPoints;
        public ObservableCollection<IncidentPointsValue> IncPenaltyPoints { get => incPenaltyPoints; set => SetNotifyCollection(ref incPenaltyPoints, value); }

        private bool isMultiScoring;
        public bool IsMultiScoring { get => isMultiScoring; set => SetValue(ref isMultiScoring, value); }

        private ObservableCollection<MyKeyValuePair<ScoringInfo, double>> multiScoringResults;
        public ObservableCollection<MyKeyValuePair<ScoringInfo, double>> MultiScoringResults { get => multiScoringResults; set => SetNotifyCollection(ref multiScoringResults, value); }

        private ObservableCollection<StandingsRowModel> standings;
        public ObservableCollection<StandingsRowModel> Standings { get => standings; set => SetNotifyCollection(ref standings, value); }

        private ScheduleInfo connectedschedule;
        public ScheduleInfo ConnectedSchedule
        {
            get => connectedschedule;
            set => SetValue(ref connectedschedule, value);
        }
        public ScoringModel() : base()
        {
            ScoringId = null;
            Sessions = new ObservableCollection<SessionInfo>();
            BonusPoints = new ObservableCollection<BonusPointsValue>();
            BasePoints = new ObservableCollection<BasePointsValue>();
            IncPenaltyPoints = new ObservableCollection<IncidentPointsValue>();
            MultiScoringResults = new ObservableCollection<MyKeyValuePair<ScoringInfo, double>>();
            Standings = new ObservableCollection<StandingsRowModel>();
            //Schedules = new ObservableCollection<ScheduleInfo>();
        }

        public ScoringModel(long? scoringId) : this()
        {
            ScoringId = scoringId;
        }

        internal override void InitializeModel()
        {
            if (Season != null)
            {
                for (int i = 0; i < MultiScoringResults.Count(); i++)
                {
                    var multiScoring = Season.Scorings.SingleOrDefault(x => x.ScoringId == MultiScoringResults.ElementAt(i).Key.ScoringId);
                    if (multiScoring != null)
                    {
                        MultiScoringResults[i] = new MyKeyValuePair<ScoringInfo, double>(multiScoring, MultiScoringResults[i].Value);
                    }
                    else
                    {
                        throw new ModelInitializeException("Error initializing Scoring Model. Could not find Scoring Model (ScoringId=" + MultiScoringResults.ElementAt(i).Key.ScoringId + ") in Season.Scorings\n" +
                            "Error in ScoringModel (ScoringId=" + ScoringId + ") - SeasonModel (SeasonId=" + Season.SeasonId, new NullReferenceException());
                    }
                }
            }
            base.InitializeModel();
        }

        public class BonusPointsValue : MyKeyValuePair<string, int>
        {
            public new string Key { get => base.Key; set { base.Key = value; } }

            public BonusPointsValue() : this("", 0) { }
            public BonusPointsValue(string identifier, int points) : base(identifier, points) { }
        }

        public class BasePointsValue : MyKeyValuePair<int, int>
        {
            public BasePointsValue(int place, int points) : base(place, points) { }
        }

        public class IncidentPointsValue : MyKeyValuePair<int, int>
        {
            public new int Key { get => base.Key; set { base.Key = value; } }

            public IncidentPointsValue() : this(0, 0) { }
            public IncidentPointsValue(int incidents, int penaltyPoints) : base(incidents, penaltyPoints) { }
        }

        //public void CalculateScoringPoints()
        //{
        //    Dictionary<uint, int>[] allPoints = Results.Select(x => Rule.GetChampPoints(x)).ToArray();
        //    TotalScoringPoints = allPoints.Aggregate((x, y) =>
        //    {
        //        foreach (var z in y)
        //        {
        //            if (x.ContainsKey(z.Key))
        //            {
        //                x[z.Key] += z.Value;
        //            }
        //            else
        //            {
        //                x.Add(z.Key, z.Value);
        //            }
        //        }
        //        return x;
        //    });
        //}
    }
}
