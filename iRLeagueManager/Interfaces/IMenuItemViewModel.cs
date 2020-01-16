using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace iRLeagueManager.Interfaces
{
    public interface IMenuItemViewModel
    {
        string Text { get; }
        string ImgPath { get; }

        ICommand Command { get; }

        //...
    }
}
