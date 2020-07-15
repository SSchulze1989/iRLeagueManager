using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace iRLeagueManager.Data
{
    public interface ICredentialsProvider : ICredentials
    {
        ICredentials GetCredentials();
    }
}
