using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
//using System.Xml;
//using System.Xml.Serialization;
using iRLeagueManager.Interfaces;
using iRLeagueManager.Enums;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace iRLeagueManager.Models.Sessions
{
    //[Serializable()]
    //[XmlInclude(typeof(SessionModel))]
    public class ScheduleModel : ScheduleInfo, IHierarchicalModel//, ISchedule
    {
        //private SeasonModel season;
        //public SeasonModel Season { get => season; set => SetValue(ref season, value); }

        private ObservableCollection<SessionModel> sessions;
        public ObservableCollection<SessionModel> Sessions
        {
            get => sessions;
            set => SetNotifyCollection(ref sessions, value);
        }
        //ReadOnlyObservableCollection<ISession> ISchedule.Sessions => new ReadOnlyObservableCollection<ISession>(Sessions);

        public new int SessionCount { get => (Sessions?.Count()).GetValueOrDefault(); }

        public int RacesCount { get => Sessions.Where(x => x.SessionType == SessionType.Race).Count(); }

        string IHierarchicalModel.Description => Name;

        IEnumerable<object> IHierarchicalModel.Children => Sessions.Cast<object>();

        //private Schedule()
        //{
        //    Sessions = new ObservableCollection<ISession>();
        //}

        //internal void SetLeagueClient(IRLeagueClient leagueClient)
        //{
        //    client = leagueClient;
        //}

        public ScheduleModel() : base()
        {
            Sessions = new ObservableCollection<SessionModel>();
        }

        public ScheduleModel(long? scheduleId) : base(scheduleId)
        {
            Sessions = new ObservableCollection<SessionModel>();
        }

        public static ScheduleModel GetTemplate()
        {
            return new ScheduleModel()
            {
                Name = "...",
                Sessions = null
            };
        }

        internal override void InitializeModel()
        {
            if (!isInitialized)
            {
                foreach (var session in Sessions)
                {
                    //session.Schedule = this;
                    session.InitializeModel();
                }
            }
            base.InitializeModel();
        }

        //public string Name => client.Name + " - " + ScheduleId.ToString();
    }
}
