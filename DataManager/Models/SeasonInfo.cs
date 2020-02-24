﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iRLeagueManager.Interfaces;
using System.ComponentModel;

using iRLeagueManager.Attributes;

namespace iRLeagueManager.Models
{
    public class SeasonInfo : ModelBase, ISeasonInfo, INotifyPropertyChanged
    {
        private long? seasonId;
        [EqualityCheckProperty]
        public long? SeasonId { get => seasonId; internal set { seasonId = value; OnPropertyChanged(); } }

        public override long? ModelId => SeasonId;

        private string seasonName;
        public string SeasonName { get => seasonName; set { seasonName = value; OnPropertyChanged(); } }
    }
}
