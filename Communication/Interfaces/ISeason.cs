using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.ComponentModel;
using System.Collections.ObjectModel;
using iRLeagueManager.Attributes;


namespace iRLeagueManager.Interfaces
{
    public interface ISeason : ISeasonInfo, INotifyPropertyChanged
    {
        new string SeasonName { get; set; }
        //ReadOnlyObservableCollection<IResult> Results { get; }
        ObservableCollection<ISchedule> Schedules { get; }
        //ReadOnlyObservableCollection<IScheduleInfo> Schedules { get; }
        //ReadOnlyObservableCollection<IScoringInfo> Scorings { get; }
        //ReadOnlyObservableCollection<IReviewInfo> Reviews { get; }

        IEnumerable<IResult> GetResults();

        //IResult AddNewResult();
        //ISchedule AddNewSchedule();
        //IScoring AddNewScoring();
        //ISession AddNewSession();
    }
}
