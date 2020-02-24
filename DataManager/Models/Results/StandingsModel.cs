using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace iRLeagueManager.Models.Results
{
    public class StandingsModel : ModelBase
    {
        private string title;
        public string Title { get => title; set => SetValue(ref title, value); }

        public override long? ModelId => 0;

        private ObservableCollection<StandingsRowModel> rows;
        public ObservableCollection<StandingsRowModel> Rows { get => rows; set => SetValue(ref rows, value); }
    }
}
