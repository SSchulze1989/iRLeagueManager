﻿// MIT License

// Copyright (c) 2020 Simon Schulze

// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:

// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.

// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Collections.ObjectModel;
using iRLeagueManager.Data;
using iRLeagueManager.Enums;
using iRLeagueManager.Interfaces;
using iRLeagueManager.Models;
using iRLeagueManager.ViewModels;
using iRLeagueManager.Models.Database;
using iRLeagueManager.Models.Sessions;
//using iRLeagueManager.User;
using iRLeagueManager.Logging;
using System.Runtime.CompilerServices;
using System.Collections.Specialized;
using System.Windows.Forms;

namespace iRLeagueManager.ViewModels
{
    public class MainWindowViewModel : ViewModelBase, IDisposable
    {
        //private LeagueDatabase _leagueDb;
        //public LeagueDatabase LeagueDb { get => _leagueDb; set { _leagueDb = value; NotifyPropertyChanged(); } }
        //private LeagueContext LeagueContext => GlobalSettings.LeagueContext;
        //private ModelManager LeagueContext => GlobalSettings.ModelManager;
        //public LeagueContext LeagueContext { get => leagueContext; private set { leagueContext = value; OnPropertyChanged(); } }

        public ReadOnlyObservableCollection<ExceptionLogMessage> ErrorLog => GlobalSettings.Logger.ErrorMessages;

        //private UserContext UserContext => GlobalSettings.UserContext;

        //private LoginViewModel userLogin;
        //public LoginViewModel UserLogin { get => userLogin; set => SetValue(ref userLogin, value); }

        private UserViewModel currentUser;
        public UserViewModel CurrentUser
        {
            get
            {
                if (currentUser.Model == null || !currentUser.Model.Equals(LeagueContext?.UserManager?.CurrentUser))
                {
                    currentUser.UpdateSource(LeagueContext?.UserManager?.CurrentUser);
                }
                return currentUser;
            }
        }

        public string CurrentLeagueName => LeagueContext.LeagueName;

        //private bool isUserLoggedIn;
        //public bool IsUserLoggedIn { get => isUserLoggedIn; set => SetValue(ref isUserLoggedIn, value); }

        private DatabaseStatusModel dbStatus;
        public DatabaseStatusModel DbStatus { get => dbStatus; set => SetValue(ref dbStatus, value); }

        private ViewModelBase contentViewModel;
        public ViewModelBase ContentViewModel { get => contentViewModel; set => SetValue(ref contentViewModel, value); }

        private ObservableCollection<SeasonModel> seasonList;
        public ObservableCollection<SeasonModel> SeasonList { get => seasonList; set => SetValue(ref seasonList, value); }

        private bool isErrorsOpen;
        public bool IsErrorsOpen { get => isErrorsOpen; set => SetValue(ref isErrorsOpen, value); }

        public ICommand SchedulesButtonCmd { get; }

        public ICommand CloseErrorsCmd { get; }

        private SeasonModel selectedSeason;
        public SeasonModel SelectedSeason
        {
            get => selectedSeason;
            set
            {
                if (SetValue(ref selectedSeason, value))
                {
                    CurrentSeason.Load(selectedSeason);
                    SeasonChanged?.Invoke(this, new EventArgs());
                }
            }
        }

        public event EventHandler SeasonChanged;

        public SeasonViewModel CurrentSeason { get; }

        private LeagueModel league;
        public LeagueModel League { get => league; set => SetValue(ref league, value); }

        public MainWindowViewModel()
        {
            DbStatus = new DatabaseStatusModel();
            SeasonList = new ObservableCollection<SeasonModel>(new List<SeasonModel>() { new SeasonModel() { SeasonName = "Loading..." } });
            CurrentSeason = new SeasonViewModel();
            SelectedSeason = SeasonList.FirstOrDefault();
            //UserLogin = new LoginViewModel(this);
            currentUser = new UserViewModel();
            CloseErrorsCmd = new RelayCommand(o => IsErrorsOpen = false, o => true);
            ((INotifyCollectionChanged)ErrorLog).CollectionChanged += OnErrorLogChanged;
            IsErrorsOpen = false;
        }

        private void OnErrorLogChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (ErrorLog.Count > 0)
                IsErrorsOpen = true;
        }

        public override async Task Refresh()
        {
            await Load();
            await base.Refresh();
        }

        public async Task Load()
        {
            IsLoading = true;
            if (LeagueContext == null)
            {
                GlobalSettings.SetGlobalLeagueContext(new LeagueContext());
                //LeagueContext.DbContext.OpenConnection();
                //await LeagueContext.UpdateMemberList();
            }

            LeagueContext.AddStatusItem(DbStatus);

            try
            {
                League = await LeagueContext.GetLeagueDetails();
                await LeagueContext.LoadTrackList();
                GlobalSettings.SetLocationCollection(LeagueContext.Locations);
            }
            catch (Exception e)
            {
                GlobalSettings.LogError(e);
            }
            //UserLogin.Open();

            //LeagueContext.AddStatusItem(DbStatus);
            try
            { 
                //await LeagueContext.UserLoginAsync("TestUser", "testuser");
                await LeagueContext.UpdateMemberList();
                SeasonList = new ObservableCollection<SeasonModel>(await LeagueContext.GetModelsAsync<SeasonModel>());
            }
            catch (Exception e)
            {
                GlobalSettings.LogError(e);
            }
            finally
            {
                IsLoading = false;
            }
            SelectedSeason = SeasonList?.LastOrDefault();

            //await LeagueContext.UserLoginAsync("Master", Encoding.UTF8.GetBytes("TestPasswort"));
            OnPropertyChanged(null);
        }

        public void LogInOut()
        {
            if (CurrentUser != null)
            {
                //LeagueContext.Reconnect();
                //UserContext.UserLogout();
            }
            else
            {
                //UserLogin.Open();
            }
            OnPropertyChanged(null);
        }

        public async Task<SeasonModel> AddSeason(SeasonModel season)
        {
            if (season == null)
            {
                return null;
            }

            try
            {
                IsLoading = true;
                season = await LeagueContext.AddModelAsync(season);
                SeasonList.Add(season);
                SelectedSeason = season;
                return season;
            }
            catch (Exception e)
            {
                GlobalSettings.LogError(e);
            }
            finally
            {
                IsLoading = false;
            }
            return null;
        }

        public async Task<bool> RemoveSeason(SeasonModel season)
        {
            if (season == null || SeasonList.Contains(season) == false)
            {
                return false;
            }

            try
            {
                IsLoading = true;
                await LeagueContext.DeleteModelsAsync(season);
                SeasonList.Remove(season);
                SelectedSeason = SeasonList.LastOrDefault();
                return true;
            }
            catch (Exception e)
            {
                GlobalSettings.LogError(e);
            }
            finally
            {
                IsLoading = false;
            }
            return false;
        }

        protected override void Dispose(bool disposing)
        {
            if (LeagueContext != null)
            {
                LeagueContext.RemoveStatusItem(DbStatus);
            }
            base.Dispose(disposing);
        }
    }
}
