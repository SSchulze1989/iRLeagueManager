using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

using iRLeagueManager.Models.Results;
using iRLeagueManager.Models.Reviews;

namespace iRLeagueManager.ViewModels
{
    public class ResultViewModel : LeagueContainerModel<ResultModel>
    {
        //public ResultModel Model { get => Source; set => SetSource(value); }

        protected override ResultModel Template => new ResultModel();

        public ObservableModelCollection<ResultRowViewModel, ResultRowModel> RawResults => new ObservableModelCollection<ResultRowViewModel, ResultRowModel>(Model?.RawResults);

        public ObservableCollection<IncidentReviewInfo> Reviews => Model?.Reviews;
    }
}
