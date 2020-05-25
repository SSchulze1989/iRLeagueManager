﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

using iRLeagueManager.Models;
using iRLeagueManager.Models.Results;
using iRLeagueManager.Models.Sessions;
using iRLeagueManager.ViewModels.Collections;

namespace iRLeagueManager.ViewModels
{
    public class SettingsPageViewModel : ViewModelBase
    {
        private SeasonViewModel season;
        public SeasonViewModel Season { get => season; set => SetValue(ref season, value); }

        private readonly ObservableModelCollection<ScoringViewModel,ScoringModel> scorings;
        public ObservableModelCollection<ScoringViewModel, ScoringModel> Scorings
        {
            get
            {
                if (scorings.GetSource() != Season?.Scorings)
                    scorings.UpdateSource(Season?.Scorings);
                return scorings;
            }
        }

        public ICommand AddScoringCmd { get; }
        public ICommand DeleteScoringCmd { get; }

        public SettingsPageViewModel() : base()
        {
            scorings = new ObservableModelCollection<ScoringViewModel, ScoringModel>();
            Season = new SeasonViewModel(SeasonModel.GetTemplate());
            AddScoringCmd = new RelayCommand(o => AddScoring(), o => Season != null);
            DeleteScoringCmd = new RelayCommand(o => DeleteScoring((o as ScoringViewModel).Model), o => o != null);
        }

        public async Task Load(SeasonModel season)
        {
            if (season == null)
            {
                Season = null;
                StatusMsg = "No season loaded";
                OnPropertyChanged(null);
                return;
            }

            Season = new SeasonViewModel(season);
            OnPropertyChanged(null);

            await LeagueContext.UpdateModelAsync(season);
            await LeagueContext.UpdateModelsAsync(season.Scorings);
        }

        public async void AddScoring()
        {
            try
            {
                var scoring = new ScoringModel() { Season = this.Season.Model, Name = "New Scoring" };
                scoring = await LeagueContext.AddModelAsync(scoring);
                Season.Scorings.Add(scoring);
                await LeagueContext.UpdateModelAsync(Season.Model);
                Scorings.UpdateCollection();
            }
            catch (Exception e)
            {
                GlobalSettings.LogError(e);
            }
            finally
            {

            }
        }

        public async void DeleteScoring(ScoringModel scoring)
        {
            if (scoring == null)
                return;

            try
            {
                await LeagueContext.DeleteModelsAsync(scoring);
                Season.Scorings.Remove(scoring);
                await LeagueContext.UpdateModelAsync(Season.Model);
                Scorings.UpdateCollection();
            }
            catch (Exception e)
            {
                GlobalSettings.LogError(e);
            }
            finally
            {

            }
        } 

        protected override void Dispose(bool disposing)
        {
            Season.Dispose();
            Scorings.Dispose();
            base.Dispose(disposing);
        }
    }
}
