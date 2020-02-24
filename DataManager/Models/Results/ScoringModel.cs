﻿using System;
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
    public class ScoringModel : ModelBase
    {
        public long? ScoringId { get; internal set; }

        public override long? ModelId => ScoringId;

        private string name;
        public string Name { get => name; set => SetValue(ref name, value); }

        private int dropWeeks;
        public int DropWeeks { get => dropWeeks; set => SetValue(ref dropWeeks, value); }
        
        private int averageRaceNr;
        public int AverageRaceNr { get => averageRaceNr; set => SetValue(ref averageRaceNr, value); }
        
        private ObservableCollection<SessionInfo> sessions;
        public ObservableCollection<SessionInfo> Sessions { get => sessions; set => SetNotifyCollection(ref sessions, value); }
        
        private long seasonId;
        public long SeasonId { get => seasonId; set => SetValue(ref seasonId, value); }
        
        private SeasonModel season;
        public SeasonModel Season { get => season; set => SetValue(ref season, value); }
        
        private string basePoints;
        public string BasePoints { get => basePoints; set => SetValue(ref basePoints, value); }
        
        private string bonusPoints;
        public string BonusPoints { get => bonusPoints; set => SetValue(ref bonusPoints, value); }
        
        private string incPenaltyPoints;
        public string IncPenaltyPoints { get => incPenaltyPoints; set => SetValue(ref incPenaltyPoints, value); }
        
        private string multiScoringFactors;
        public string MultiScoringFactors { get => multiScoringFactors; set => SetValue(ref multiScoringFactors, value); }
        
        private ObservableCollection<ScoringModel> multiScoringResults;
        public ObservableCollection<ScoringModel> MultiScoringResults { get => multiScoringResults; set => SetNotifyCollection(ref multiScoringResults, value); }

        private ObservableCollection<StandingsRowModel> standings;
        public ObservableCollection<StandingsRowModel> Standings { get => standings; set => SetNotifyCollection(ref standings, value); }

        public ScoringModel()
        {
            ScoringId = null;
            Sessions = new ObservableCollection<SessionInfo>();
            MultiScoringResults = new ObservableCollection<ScoringModel>();
            Standings = new ObservableCollection<StandingsRowModel>();
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
                    var multiScoring = Season.Scorings.SingleOrDefault(x => x.ScoringId == MultiScoringResults.ElementAt(i).ScoringId);
                    if (multiScoring != null)
                    {
                        MultiScoringResults[i] = multiScoring;
                    }
                    else
                    {
                        throw new ModelInitializeException("Error initializing Scoring Model. Could not find Scoring Model (ScoringId=" + MultiScoringResults.ElementAt(i).ScoringId + ") in Season.Scorings\n" +
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
