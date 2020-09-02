﻿using iRLeagueManager.Models.Sessions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity.Migrations.Builders;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace iRLeagueManager.Models.Results
{
    public class ScoringTableModel : MappableModel, ICacheableModel
    {
        public long ScoringTableId { get; set; }
        public override long[] ModelId => new long[] { ScoringTableId };

        private string name;
        public string Name { get => name; set => SetValue(ref name, value); }

        private int dropWeeks;
        public int DropWeeks { get => dropWeeks; set => SetValue(ref dropWeeks, value); }

        private int averageRaceNr;
        public int AverageRaceNr { get => averageRaceNr; set => SetValue(ref averageRaceNr, value); }

        private ObservableCollection<MyKeyValuePair<ScoringInfo, double>> scorings;
        public ObservableCollection<MyKeyValuePair<ScoringInfo, double>> Scorings { get => scorings; set => SetNotifyCollection(ref scorings, value); }

        private ObservableCollection<SessionInfo> sessions;
        public ObservableCollection<SessionInfo> Sessions { get => sessions; set => SetValue(ref sessions, value); }
    }
}