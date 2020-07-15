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

        public ObservableModelCollection<ResultRowViewModel, ResultRowModel> RawResults => 
            new ObservableModelCollection<ResultRowViewModel, ResultRowModel>(Model?.RawResults);
        public ObservableCollection<IncidentReviewInfo> Reviews => Model?.Reviews;

        private ScoringViewModel scoring;
        public ScoringViewModel Scoring { get => scoring; set => SetValue(ref scoring, value); }
        //public ObservableModelCollection<ScoredResultRowViewModel, ScoredResultRowModel> FinalResults =>
        //    new ObservableModelCollection<ScoredResultRowViewModel, ScoredResultRowModel>(Model?.FinalResults);

        private readonly ObservableModelCollection<ScoredResultRowViewModel, ScoredResultRowModel> finalResults;
        public ObservableModelCollection<ScoredResultRowViewModel, ScoredResultRowModel> FinalResults
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
            finalResults = new ObservableModelCollection<ScoredResultRowViewModel, ScoredResultRowModel>();
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
