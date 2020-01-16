using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Windows.Input;

using iRLeagueManager.Data;
using iRLeagueManager.Models;

namespace iRLeagueManager.ViewModels
{
    public class MenuBarViewModel : ViewModelBase
    {
        private LeagueContext LeagueContext => GlobalSettings.LeagueContext;

        public Action<ViewModelBase> SetContentViewModel { get; set; }

        private ObservableCollection<SeasonInfo> seasonList;
        public ObservableCollection<SeasonInfo> SeasonList { get => seasonList; set => SetValue(ref seasonList, value); }

        public ICommand SchedulesButtonCmd { get; }

        private SeasonInfo selectedSeason;
        public SeasonInfo SelectedSeason
        {
            get => selectedSeason;
            set
            {
                if (SetValue(ref selectedSeason, value) && selectedSeason?.SeasonId != null)
                {
                    CurrentSeason.Load(selectedSeason);
                }
            }
        }

        public SeasonViewModel CurrentSeason { get; }

        public MenuBarViewModel()
        {
            SeasonList = new ObservableCollection<SeasonInfo>(new List<SeasonInfo>() { new SeasonInfo() { SeasonName = "Loading..." } });
            SelectedSeason = SeasonList.First();
            CurrentSeason = new SeasonViewModel();
        }

        public async void Load()
        {
            selectedSeason = SeasonList.First();
            SeasonList = new ObservableCollection<SeasonInfo>(await LeagueContext.GetSeasonListAsync());
            SelectedSeason = SeasonList.First();
        }
    }
}
