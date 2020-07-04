using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iRLeagueManager.ViewModels
{
    public interface IContainerModelBase<TSource> : IDisposable
    {
        TSource GetSource();

        bool UpdateSource(TSource source);

        void OnUpdateSource();
    }
}
