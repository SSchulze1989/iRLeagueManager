using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Serialization;

namespace iRLeagueManager.Timing
{
    public interface ILapTime
    {
        TimeSpan Time { get; set; }
        string ToString();
        bool IsValid();
    }
}
