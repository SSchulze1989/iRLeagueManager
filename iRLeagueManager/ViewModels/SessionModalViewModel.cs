using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

using iRLeagueManager.Models.Sessions;
using iRLeagueManager.Interfaces;

namespace iRLeagueManager.ViewModels
{
    public class SessionModalViewModel : SessionViewModel, IModalViewModel
    {
        public Action OnSubmitAction { get; set; }
        public Action OnCancelAction { get; set; }

        public ICommand SubmitCmd { get; }
        public ICommand CancelCmd { get; }

        public SessionModalViewModel(Action onSubmitAction = null, Action onCancelAction = null) : base()
        {
            OnSubmitAction = onSubmitAction;
            OnCancelAction = onCancelAction;
            SubmitCmd = new RelayCommand(o => Submit(), o => Model != null);
            CancelCmd = new RelayCommand(o => Cancel(), o => true);
        }

        public SessionModalViewModel(SessionModel sessionModel, Action onSubmitAction = null, Action onCancelAction = null) : this(onSubmitAction, onCancelAction)
        {
            Model = sessionModel;
        }

        public SessionModalViewModel(SessionViewModel sessionViewModel, Action onSubmitAction = null, Action onCancelAction = null) : this(sessionViewModel.GetSource(), onSubmitAction, onCancelAction)
        {
        }

        public void Submit()
        {

            OnSubmitAction?.Invoke();
        }

        public void Cancel()
        {

            OnCancelAction?.Invoke();
        }
    }
}
