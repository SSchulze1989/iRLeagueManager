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
