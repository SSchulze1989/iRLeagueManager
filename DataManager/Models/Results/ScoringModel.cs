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
using iRLeagueManager.Exceptions;

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
        
        private ObservableCollection<SessionInfo> sessions;
        public ObservableCollection<SessionInfo> Sessions { get => sessions; set => SetNotifyCollection(ref sessions, value); }

        private ObservableCollection<ResultInfo> results;
        public ObservableCollection<ResultInfo> Results { get => results; set => SetNotifyCollection(ref results, value); }
        
        private long seasonId;
        public long SeasonId { get => seasonId; set => SetValue(ref seasonId, value); }
        
        private SeasonModel season;
        public SeasonModel Season { get => season; set => SetValue(ref season, value); }

        private ObservableCollection<KeyValuePair<int, int>> basePoints;
        //public string BasePoints { get => basePoints; set => SetValue(ref basePoints, value); }
        public ObservableCollection<KeyValuePair<int, int>> BasePoints { get => basePoints; set => SetNotifyCollection(ref basePoints, value); }
        
        private ObservableCollection<KeyValuePair<string,int>> bonusPoints;
        public ObservableCollection<KeyValuePair<string,int>> BonusPoints { get => bonusPoints; set => SetNotifyCollection(ref bonusPoints, value); }
        
        private ObservableCollection<KeyValuePair<int,int>> incPenaltyPoints;
        public ObservableCollection<KeyValuePair<int,int>> IncPenaltyPoints { get => incPenaltyPoints; set => SetNotifyCollection(ref incPenaltyPoints, value); }
        
        private ObservableCollection<KeyValuePair<ScoringModel, double>> multiScoringResults;
        public ObservableCollection<KeyValuePair<ScoringModel, double>> MultiScoringResults { get => multiScoringResults; set => SetNotifyCollection(ref multiScoringResults, value); }

        private ObservableCollection<StandingsRowModel> standings;
        public ObservableCollection<StandingsRowModel> Standings { get => standings; set => SetNotifyCollection(ref standings, value); }

        public ScoringModel() : base()
        {
            ScoringId = null;
            Sessions = new ObservableCollection<SessionInfo>();
            BonusPoints = new ObservableCollection<KeyValuePair<string, int>>();
            BasePoints = new ObservableCollection<KeyValuePair<int, int>>();
            IncPenaltyPoints = new ObservableCollection<KeyValuePair<int, int>>();
            MultiScoringResults = new ObservableCollection<KeyValuePair<ScoringModel, double>>();
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
                        MultiScoringResults[i] = new KeyValuePair<ScoringModel, double>(multiScoring, MultiScoringResults[i].Value);
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
