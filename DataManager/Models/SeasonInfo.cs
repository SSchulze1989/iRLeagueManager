using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iRLeagueManager.Interfaces;
using System.ComponentModel;

using iRLeagueManager.Attributes;

namespace iRLeagueManager.Models
{
    public class SeasonInfo : MappableModel, ISeasonInfo, INotifyPropertyChanged
    {
        private long? seasonId;
        [EqualityCheckProperty]
        public long? SeasonId { get => seasonId; internal set => SetValue(ref seasonId, value); }

        public override long[] ModelId => new long[] { SeasonId.GetValueOrDefault() };

        private string seasonName;
        public string SeasonName { get => seasonName; set => SetValue(ref seasonName, value); }
    }
}
