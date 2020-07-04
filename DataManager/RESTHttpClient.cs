using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Runtime.CompilerServices;

namespace iRLeagueManager
{
    public class RESTHttpClient : HttpClient
    {
        private Uri RequestUri { get; }
        public RESTHttpClient()
        {
            BaseAddress = new Uri("https://localhos:44369/api");
            RequestUri = new Uri("https://localhos:44369/api/Home");
        }
    }
}
