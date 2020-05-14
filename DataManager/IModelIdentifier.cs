using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using iRLeagueManager.Models;

namespace iRLeagueManager
{
    public interface IModelIdentifier : IEquatable<IModelIdentifier>
    {
        Type ModelType { get; }
        long[] ModelId { get; }
    }
}
