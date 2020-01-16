using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iRLeagueManager.Interfaces
{
    public interface IToken
    {
        int GetHashCode();
        bool Equals(Object obj);
    }
}
