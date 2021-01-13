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
using iRLeagueManager.Models.Results;
using iRLeagueManager.Models.Reviews;
using iRLeagueManager.Models.Sessions;
using System.Windows.Input;

using iRLeagueManager.Data;

using iRLeagueManager.ViewModels.Collections;

namespace iRLeagueManager.ViewModels
{
    public class ScoredResultViewModel : LeagueContainerModel<ScoredResultModel>
    {
        protected override ScoredResultModel Template => ScoredResultModel.GetTemplate();

        private SessionViewModel session;
        public SessionViewModel Session { get => session; set => SetValue(ref session, value); }

        public ObservableViewModelCollection<ResultRowViewModel, ResultRowModel> RawResults => 
            new ObservableViewModelCollection<ResultRowViewModel, ResultRowModel>(Model?.RawResults);
        public ObservableCollection<IncidentReviewInfo> Reviews => Model?.Reviews;

        private ScoringViewModel scoring;
        public ScoringViewModel Scoring { get => scoring; set => SetValue(ref scoring, value); }
        //public ObservableModelCollection<ScoredResultRowViewModel, ScoredResultRowModel> FinalResults =>
        //    new ObservableModelCollection<ScoredResultRowViewModel, ScoredResultRowModel>(Model?.FinalResults);

        public string ScoringName => Model?.ScoringName;

        private readonly ObservableViewModelCollection<ScoredResultRowViewModel, ScoredResultRowModel> finalResults;
        public ObservableViewModelCollection<ScoredResultRowViewModel, ScoredResultRowModel> FinalResults
        {
            get
            {
                if (finalResults.GetSource() != Model?.FinalResults)
                    finalResults.UpdateSource(Model?.FinalResults);
                return finalResults;
            }
        }

        public ICommand CalculateResultsCmd { get; private set; }

        public ScoredResultViewModel() : base()
        {
            Model = Template;
            Session = new SessionViewModel();
            CalculateResultsCmd = new RelayCommand(o => CalculateResults(), o => (Session != null && Scoring != null));
            finalResults = new ObservableViewModelCollection<ScoredResultRowViewModel, ScoredResultRowModel>()
            {
                PreserveViewModels = false
            };
        }

        //public async Task Load()
        //{
        //    try 
        //    {
        //        IsLoading = true;
        //        await LeagueContext.UpdateMemberList();
        //        await Load((session?.SessionId).GetValueOrDefault(), (scoring?.ScoringId).GetValueOrDefault());
        //    }
        //    catch (Exception e)
        //    {
        //        GlobalSettings.LogError(e);
        //    }
        //    finally
        //    {
        //        IsLoading = false;
        //    }
        //    Session = session;
        //    Scoring = scoring;
        //}
        public async override Task Load(params long[] modelId)
        {
            try
            {
                IsLoading = true;
                await LeagueContext.UpdateMemberList();
            }
            catch (Exception e)
            {
                GlobalSettings.LogError(e);
            }
            finally
            {
                IsLoading = false;
            }
            await base.Load(modelId);
        }

        public async void CalculateResults()
        {
            try
            {
                IsLoading = true;
                await LeagueContext.ModelContext.CalculateScoredResultsAsync(session.SessionId);
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

        //public override async Task Load(long modelId, long modelId2nd)
        //{
        //    await base.Load(modelId, modelId2nd);
        //    if (Model == null)
        //    {
        //        Model = new ScoredResultModel();
        //    }
        //}
    }
}
