using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iRLeagueManager.Models.Statistics
{
    public class ImportedStatisticSetModel : StatisticSetModel
    {
        private string importSource;
        public string ImportSource { get => importSource; set => SetValue(ref importSource, value); }

        private string description;
        public string Description { get => description; set => SetValue(ref description, value); }

        private DateTime firstDate;
        public DateTime FirstDate { get => firstDate; set => SetValue(ref firstDate, value); }

        private DateTime lastDate;
        public DateTime LastDate { get => lastDate; set => SetValue(ref lastDate, value); }

        public override string StatisticSetType => "Imported";

        public ImportedStatisticSetModel()
        {
            FirstDate = DateTime.Now;
            LastDate = DateTime.Now;
        }
    }
}
