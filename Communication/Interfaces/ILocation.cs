using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iRLeagueManager.Attributes;

namespace iRLeagueManager.Interfaces
{
    public interface ILocation
    {
        [EqualityCheckProperty]
        string LocationId { get; }
        [EqualityCheckProperty]
        string FullName { get; }
    }
}
