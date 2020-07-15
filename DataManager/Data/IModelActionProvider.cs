using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using iRLeagueManager.LeagueDBServiceRef;

namespace iRLeagueManager.Data
{
    public interface IModelDataAndActionProvider<TModel, TKey> : IActionProvider<TModel, TKey>, IModelDataProvider<TModel, TKey>
    {
    }

    public interface IModelDataAndActionProvider : IModelDataProvider<MappableDTO, long>, IActionProvider, IModelDataProvider
    {
    }
}
