using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iRLeagueManager.Views
{
    public interface IModalContent
    {
        string SubmitText { get; }
        string CancelText { get; }
        bool CanSubmit();
        Task<bool> SubmitAsync();
        void Cancel();
    }
}
