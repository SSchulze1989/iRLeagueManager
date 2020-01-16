using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace iRLeagueManager.Interfaces
{
    public interface ILeagueContext
    {
        ReadOnlyObservableCollection<ILeagueMember> MemberList { get; }
        ReadOnlyObservableCollection<ISeason> Seasons { get; }
    }
}
