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
        public Action<ViewModelBase> SetContentViewModel { get; set; }

        private ObservableCollection<SeasonModel> seasonList;
        public ObservableCollection<SeasonModel> SeasonList { get => seasonList; set => SetValue(ref seasonList, value); }

        public ICommand SchedulesButtonCmd { get; }

        private SeasonModel selectedSeason;
        public SeasonModel SelectedSeason
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
            SeasonList = new ObservableCollection<SeasonModel>(new List<SeasonModel>() { new SeasonModel() { SeasonName = "Loading..." } });
            SelectedSeason = SeasonList.First();
            CurrentSeason = new SeasonViewModel();
        }

        public async void Load()
        {
            selectedSeason = SeasonList.First();
            SeasonList = new ObservableCollection<SeasonModel>(await LeagueContext.GetSeasonListAsync());
            SelectedSeason = SeasonList.First();
        }
    }
}
