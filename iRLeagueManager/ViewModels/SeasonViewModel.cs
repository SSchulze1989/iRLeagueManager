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
using System.Collections.ObjectModel;

using iRLeagueManager.Models;
using iRLeagueManager.Models.Sessions;
using iRLeagueManager.Models.Results;
using iRLeagueManager.Models.Reviews;
using iRLeagueManager.Models.User;
using iRLeagueManager.Data;
using iRLeagueManager.Interfaces;
using System.Windows.Input;
using System.Windows.Forms;

namespace iRLeagueManager.ViewModels
{
    public class SeasonViewModel : LeagueContainerModel<SeasonModel>
    {
        //public SeasonModel Model
        //{
        //    get => Source;
        //    private set
        //    {
        //        if (SetSource(value))
        //        {
        //            OnPropertyChanged(null);
        //        }
        //    }
        //}

        protected override SeasonModel Template => SeasonModel.GetTemplate();

        public long SeasonId => (Model?.SeasonId).GetValueOrDefault();

        public string SeasonName { get => Model?.SeasonName; set => Model.SeasonName = value; }

        public ObservableCollection<ScheduleInfo> Schedules => Model?.Schedules;

        public ObservableCollection<ScoringModel> Scorings => Model?.Scorings;

        public ObservableCollection<ScoringTableModel> ScoringTables => Model?.ScoringTables;
        //public ObservableCollection<ResultInfo> Results => Model?.Results;

        //public ObservableCollection<IncidentReviewInfo> Reviews => Model?.Reviews;

        public DateTime? SeasonStart => Model?.SeasonStart;

        public DateTime? SeasonEnd => Model?.SeasonEnd;

        public bool HideCommentsBeforeVoted { get => Model.HideCommentsBeforeVoted; set => Model.HideCommentsBeforeVoted = value; }

        public bool Finished { get => Model.Finished; set => Model.Finished = value; }

        public SeasonViewModel() : base()
        {
            SeasonModel.GetTemplate();
        }

        public SeasonViewModel(SeasonModel source) : base(source) { }

        public override async Task Load(params long[] modelId)
        {
            if (Model != null && Model.ModelId == modelId)
            {
                IsLoading = true;
                try
                {
                    await LeagueContext.UpdateModelAsync(Model);
                }
                catch (Exception e)
                {
                    GlobalSettings.LogError(e);
                }
                finally
                {
                    IsLoading = false;
                }
            }
            else
            {
                IsLoading = true;
                try
                {
                    Model = SeasonModel.GetTemplate();
                    Model = await LeagueContext.GetModelAsync<SeasonModel>(modelId);
                }
                catch (Exception e)
                {
                    GlobalSettings.LogError(e);
                }
                finally
                {
                    IsLoading = false;
                }
            }
        }
        public async void Load(SeasonModel season)
        {
            if (season == null)
            {
                Model = SeasonModel.GetTemplate();
                return;
            }


            if (Model == null || Model?.SeasonId != season?.SeasonId)
            {
                IsLoading = true;
                try
                {
                    Model = Model = SeasonModel.GetTemplate(season);
                    Model = await LeagueContext.GetModelAsync<SeasonModel>(season.SeasonId.Value);
                }
                catch (Exception e)
                {
                    GlobalSettings.LogError(e);
                }
                finally
                {
                    IsLoading = false;
                }
            }
            else if (season.SeasonId != null)
            {
                await Load(season.SeasonId.Value);
            }
            else
            {
                Model = season;
            }
        }
    }
}
