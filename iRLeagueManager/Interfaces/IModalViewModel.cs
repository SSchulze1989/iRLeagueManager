using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace iRLeagueManager.Interfaces
{
    public interface IModalViewModel
    {
        Action OnSubmitAction { get; set; }
        Action OnCancelAction { get; set; }

        ICommand SubmitCmd { get; }
        ICommand CancelCmd { get; }

        void Submit();
        void Cancel();
    }
}
