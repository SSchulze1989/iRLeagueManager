using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iRLeagueManager.Models
{
    public interface ICacheableModel 
    {
        object[] ModelId { get; }
        bool CompareIdentity(ICacheableModel comp);
        bool IsBaseType();
        Type GetBaseType();
    }
}
