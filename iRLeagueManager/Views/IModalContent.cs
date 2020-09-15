﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iRLeagueManager.Views
{
    public interface IModalContent
    {
        string Header { get; }
        string SubmitText { get; }
        string CancelText { get; }
        bool IsLoading { get; }

        void OnLoad();
        bool CanSubmit();
        Task<bool> OnSubmitAsync();
        void OnCancel();
    }
}
