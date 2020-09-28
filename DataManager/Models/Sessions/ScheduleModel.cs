// MIT License

// Copyright (c) 2020 Simon Schulze

// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:

// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.

// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

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
            return new ScheduleModel();
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
