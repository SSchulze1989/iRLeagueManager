using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Windows.Input;
using iRLeagueManager.Models.Sessions;
using iRLeagueManager.ViewModels.Collections;
using System.Runtime.CompilerServices;
using System.Collections.Specialized;

namespace iRLeagueManager.ViewModels
{
    public class SessionSelectViewModel : ViewModelBase
    {
        private ReadOnlyObservableCollection<SessionViewModel> sessionList;
        public ReadOnlyObservableCollection<SessionViewModel> SessionList
        {
            get => sessionList;
            set
            {
                var temp = sessionList;
                if (SetValue(ref sessionList, value))
                {
                    if (temp != null)
                        ((INotifyCollectionChanged)temp).CollectionChanged -= OnSessionListChanged;
                    if (sessionList != null)
                    {
                        ((INotifyCollectionChanged)sessionList).CollectionChanged += OnSessionListChanged;
                        OnSessionListChanged(null, null);
                    }
                }
            }
        }

        private SessionViewModel selectedSession;
        public SessionViewModel SelectedSession
        {
            get => selectedSession;
            set
            {
                if (SetValue(ref selectedSession, value))
                {
                    //Task.Run(() => LoadResults());
                }
            }
        }

        public IEnumerable<SessionViewModel> FilteredSessions => SessionList.Where(SessionFilter);

        private Func<SessionViewModel, bool> sessionFilter;
        public Func<SessionViewModel, bool> SessionFilter 
        { 
            get 
            {
                if (sessionFilter != null)
                    return sessionFilter;

                return x => true;
            } 
            set => SetValue(ref sessionFilter, value); 
        }

        public ICommand NextSessionCmd { get; }
        public ICommand PreviousSessionCmd { get; }

        public SessionSelectViewModel()
        {
            NextSessionCmd = new RelayCommand(o => SelectNextSession(), o => CanSelectNextSession());
            PreviousSessionCmd = new RelayCommand(o => SelectPreviousSession(), o => CanSelectPreviousSession());
            SessionList = new ReadOnlyObservableCollection<SessionViewModel>(new ObservableCollection<SessionViewModel>());
        }

        public async Task LoadSessions(IEnumerable<SessionInfo> sessions)
        {
            ObservableModelCollection<SessionViewModel, SessionModel> sessionCollection = SessionList as ObservableModelCollection<SessionViewModel, SessionModel>;

            if (sessionCollection == null)
                SessionList = sessionCollection = new ObservableModelCollection<SessionViewModel, SessionModel>();

            IsLoading = true;
            var sessionModels = await LeagueContext.GetModelsAsync<SessionModel>(sessions.Select(x => x.ModelId), update: false, reload: false);

            var lastSelectedSession = SelectedSession;

            sessionCollection.UpdateSource(sessionModels.OrderBy(x => x.Date));

            if (lastSelectedSession == null || !SessionList.Contains(lastSelectedSession))
            {
                SelectedSession = SessionList.Where(SessionFilter).LastOrDefault();
            }
        }

        public void SelectNextSession()
        {
            var filteredSessions = SessionList.Where(SessionFilter).ToList();

            var currentSessionIndex = filteredSessions.IndexOf(SelectedSession);
            if (currentSessionIndex == -1)
                currentSessionIndex = 0;

            currentSessionIndex++;
            if (currentSessionIndex >= filteredSessions.Count())
                SelectedSession = filteredSessions.LastOrDefault();
            else
                SelectedSession = filteredSessions.ElementAt(currentSessionIndex);
        }

        public bool CanSelectNextSession()
        {
            if (SessionList == null)
                return false;

            var filteredSessions = SessionList.Where(SessionFilter).ToList();

            var currentSessionIndex = filteredSessions.IndexOf(SelectedSession);

            if (currentSessionIndex < filteredSessions.Count() - 1 || currentSessionIndex == -1)
                return true;

            return false;
        }

        public void SelectPreviousSession()
        {
            var filteredSessions = SessionList.Where(SessionFilter).ToList();

            var currentSessionIndex = filteredSessions.IndexOf(SelectedSession);
            if (currentSessionIndex == -1)
                currentSessionIndex = filteredSessions.Count();

            currentSessionIndex--;
            if (currentSessionIndex < 0)
                SelectedSession = filteredSessions.LastOrDefault();
            else
                SelectedSession = filteredSessions.ElementAt(currentSessionIndex);
        }

        public bool CanSelectPreviousSession()
        {
            if (SessionList == null)
                return false;

            var filteredSessions = SessionList.Where(SessionFilter).ToList();

            var currentSessionIndex = filteredSessions.IndexOf(SelectedSession);

            if (currentSessionIndex > 0 || currentSessionIndex == -1)
                return true;

            return false;
        }

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (propertyName == nameof(SelectedSession))
            {
            }

            base.OnPropertyChanged(propertyName);
        }

        protected void OnSessionListChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (SuppressPropertyChangedEvent)
                return;

            if (SessionList != null)
            {
                SessionList.Where(x => x.IsCurrentSession == true).ToList().ForEach(x => x.IsCurrentSession = false);
                var currentSession = FilteredSessions.LastOrDefault();
                if (currentSession != null)
                    currentSession.IsCurrentSession = true;
            }
        }
    }
}
