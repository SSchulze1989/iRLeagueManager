using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace iRLeagueManager.Interfaces
{
    public interface IHierarchicalModel : INotifyPropertyChanged
    {
        string Description { get; }
        IEnumerable<object> Children { get; }
    }
}
