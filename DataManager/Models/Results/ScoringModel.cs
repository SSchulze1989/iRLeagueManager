using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.ComponentModel;
using System.Xml.Serialization;
using System.Collections.ObjectModel;
using iRLeagueManager.Interfaces;
using iRLeagueManager.Enums;
using iRLeagueManager.Models.Sessions;
using iRLeagueManager.Models.Results;

namespace iRLeagueManager.Models.Results
{
    public class ScoringModel : ModelBase, IScoring
    {
        public long ScoringId { get; set; }

        public int DropWeeks { get; set; }

        public int AverageRaceNr { get; set; }
        
        public IScheduleInfo Schedule { get; set; }
        IScheduleInfo IScoring.Schedule { get => Schedule; set => Schedule = value as IScheduleInfo; }

        [XmlIgnore]
        public ObservableCollection<IRaceSessionInfo> Races { get; }
        ReadOnlyObservableCollection<IRaceSessionInfo> IScoring.Races => new ReadOnlyObservableCollection<IRaceSessionInfo>(Races);

        //[XmlIgnore]
        //public IEnumerable<Result> Results { get => (Races.Count() > 0) ? client.Results.Where(x => Races.ToList().Exists(y => y.RaceId == x.SessionId)) : new Result[0]; }

        //[XmlArray("TotalScoringPoints")]
        //[XmlArrayItem("Entry")]
        //[Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        ////public XmlDictionary<uint, int> XmlScorinPoints { get => (TotalScoringPoints != null) ? TotalScoringPoints : new Dictionary<uint, int>(); }
        //public XmlDictionaryRow<uint, int>[] XmlScoringPoints { get => TotalScoringPoints.Cast<XmlDictionaryRow<uint, int>>().ToArray(); set => TotalScoringPoints = value.ToDictionary(k => k.Key, v => v.Value); }
        //public XmlDictionary<uint, int> XmlScoringPoints { get => TotalScoringPoints; set => TotalScoringPoints = value; }
        [XmlIgnore]
        public Dictionary<uint, int> TotalScoringPoints { get; set; }

        public ScoringRuleBase Rule { get; set; }

        public ScoringModel() { }

        //public void CalculateScoringPoints()
        //{
        //    Dictionary<uint, int>[] allPoints = Results.Select(x => Rule.GetChampPoints(x)).ToArray();
        //    TotalScoringPoints = allPoints.Aggregate((x, y) =>
        //    {
        //        foreach (var z in y)
        //        {
        //            if (x.ContainsKey(z.Key))
        //            {
        //                x[z.Key] += z.Value;
        //            }
        //            else
        //            {
        //                x.Add(z.Key, z.Value);
        //            }
        //        }
        //        return x;
        //    });
        //}
    }
}
