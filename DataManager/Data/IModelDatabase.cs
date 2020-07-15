using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using iRLeagueManager.Enums;
using iRLeagueManager.Interfaces;

namespace iRLeagueManager.Data
{
    public interface IModelDatabase : IModelDataAndActionProvider
    {
        DatabaseStatusEnum DatabaseStatus { get; }
        void AddDatabaseStatusListener(IDatabaseStatus listener);
        void RemoveDatabaseStatusListener(IDatabaseStatus listener);
    }
}
