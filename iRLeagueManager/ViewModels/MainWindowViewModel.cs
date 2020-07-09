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
using iRLeagueManager.User;
using iRLeagueManager.Logging;
using System.Runtime.CompilerServices;

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

        private LoginViewModel userLogin;
        public LoginViewModel UserLogin { get => userLogin; set => SetValue(ref userLogin, value); }

        private UserViewModel currentUser;
        public UserViewModel CurrentUser
        {
            get
            {
                if (currentUser.Model == null || !currentUser.Model.Equals(LeagueContext?.CurrentUser))
                {
                    currentUser.UpdateSource(LeagueContext?.CurrentUser);
                }
                return currentUser;
            }
        }

        private DatabaseStatusModel dbStatus;
        public DatabaseStatusModel DbStatus { get => dbStatus; set => SetValue(ref dbStatus, value); }

        private ViewModelBase contentViewModel;
        public ViewModelBase ContentViewModel { get => contentViewModel; set => SetValue(ref contentViewModel, value); }

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

        public MainWindowViewModel()
        {
            DbStatus = new DatabaseStatusModel();
            SeasonList = new ObservableCollection<SeasonModel>(new List<SeasonModel>() { new SeasonModel() { SeasonName = "Loading..." } });
            SelectedSeason = SeasonList.First();
            CurrentSeason = new SeasonViewModel();
            currentUser = new UserViewModel();
        }

        public async void Load()
        {
            if (LeagueContext == null)
            {
                GlobalSettings.SetGlobalLeagueContext(new LeagueContext());
                //LeagueContext.DbContext.OpenConnection();
                //await LeagueContext.UpdateMemberList();
            }

            LeagueContext.AddStatusItem(DbStatus);

            //LeagueContext.AddStatusItem(DbStatus);
            try
            {
                IsLoading = true;
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
            SelectedSeason = SeasonList?.FirstOrDefault();

            //await LeagueContext.UserLoginAsync("Master", Encoding.UTF8.GetBytes("TestPasswort"));
            OnPropertyChanged(null);
        }

        public void LogInOut()
        {
            if (CurrentUser != null)
            {
                LeagueContext.Reconnect();
                //UserContext.UserLogout();
            }
            else
            {
                UserLogin.Open();
            }
            OnPropertyChanged(null);
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
