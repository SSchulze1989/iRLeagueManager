using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Windows.Input;

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
                if (SetValue(ref sessionList, value))
                {
                    if (SelectedSession == null)
                        SelectedSession = null;
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

        public ICommand NextSessionCmd { get; }
        public ICommand PreviousSessionCmd { get; }

        public SessionSelectViewModel()
        {
            NextSessionCmd = new RelayCommand(o => SelectNextSession(), o => CanSelectNextSession());
            PreviousSessionCmd = new RelayCommand(o => SelectPreviousSession(), o => CanSelectPreviousSession());
            SessionList = new ReadOnlyObservableCollection<SessionViewModel>(new ObservableCollection<SessionViewModel>());
        }

        public void SelectNextSession()
        {
            var currentSessionIndex = SessionList.IndexOf(SelectedSession);
            if (currentSessionIndex == -1)
                currentSessionIndex = 0;

            currentSessionIndex++;
            if (currentSessionIndex >= SessionList.Count())
                SelectedSession = SessionList.LastOrDefault();
            else
                SelectedSession = SessionList.ElementAt(currentSessionIndex);
        }

        public bool CanSelectNextSession()
        {
            if (SessionList == null)
                return false;

            var currentSessionIndex = SessionList.IndexOf(SelectedSession);

            if (currentSessionIndex < SessionList.Count() - 1 || currentSessionIndex == -1)
                return true;

            return false;
        }

        public void SelectPreviousSession()
        {
            var currentSessionIndex = SessionList.IndexOf(SelectedSession);
            if (currentSessionIndex == -1)
                currentSessionIndex = SessionList.Count();

            currentSessionIndex--;
            if (currentSessionIndex < 0)
                SelectedSession = SessionList.LastOrDefault();
            else
                SelectedSession = SessionList.ElementAt(currentSessionIndex);
        }

        public bool CanSelectPreviousSession()
        {
            if (SessionList == null)
                return false;

            var currentSessionIndex = SessionList.IndexOf(SelectedSession);

            if (currentSessionIndex > 0 || currentSessionIndex == -1)
                return true;

            return false;
        }
    }
}
