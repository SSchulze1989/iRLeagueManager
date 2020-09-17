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
        public new UserViewModel CurrentUser
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
            if (LeagueContext == null)
            {
                GlobalSettings.SetGlobalLeagueContext(new LeagueContext());
                //LeagueContext.DbContext.OpenConnection();
                //await LeagueContext.UpdateMemberList();
            }

            LeagueContext.AddStatusItem(DbStatus);

            //UserLogin.Open();

            //LeagueContext.AddStatusItem(DbStatus);
            try
            {
                IsLoading = true;
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
