using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using iRLeagueManager.Models;
using iRLeagueManager.Models.Sessions;

namespace iRLeagueManager.ViewModels.Collections
{
    public class ScheduleVMCollection : ObservableModelCollection<ScheduleViewModel, ScheduleModel>
    {
        public ScheduleVMCollection() : base(new ScheduleModel[] { ScheduleModel.GetTemplate() }) { }
    }
}
