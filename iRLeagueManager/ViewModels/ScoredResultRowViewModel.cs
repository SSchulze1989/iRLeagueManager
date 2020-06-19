using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using iRLeagueManager.Models.Results;
using iRLeagueManager.Models.Members;
using iRLeagueManager.Enums;
using iRLeagueManager.Timing;
using System.Windows.Input;

namespace iRLeagueManager.ViewModels
{
    public class ScoredResultRowViewModel : LeagueContainerModel<ScoredResultRowModel>
    {
        protected override ScoredResultRowModel Template => new ScoredResultRowModel();

        public ICommand AddPenaltyCmd { get; }
        public ICommand StartEditPenaltyCmd { get; }
        public ICommand EndEditPenaltyCmd { get; }
        public ICommand DeletePenaltyCmd { get; }

        public long ResultRowId => Source.ResultRowId.GetValueOrDefault();
        public long ScoredResultRowId => Source.ScoredResultRowId.GetValueOrDefault();
        //public int FinalPosition { get => Source.FinalPosition; set => Source.FinalPosition = value; }
        public int StartPosition { get => Source.StartPosition; set => Source.StartPosition = value; }
        public int FinishPosition { get => Source.FinishPosition; set => Source.FinishPosition = value; }

        public LeagueMember Member => Source.Member;

        public int CarNumber { get => Source.CarNumber; set => Source.CarNumber = value; }
        public int ClassId { get => Source.ClassId; set => Source.ClassId = value; }
        public string Car { get => Source.Car; set => Source.Car = value; }
        public string CarClass { get => Source.CarClass; set => Source.CarClass = value; }
        public int CompletedLaps { get => Source.CompletedLaps; set => Source.CompletedLaps = value; }
        public int LeadLaps { get => Source.LeadLaps; set => Source.LeadLaps = value; }
        public int FastLapNr { get => Source.FastLapNr; set => Source.FastLapNr = value; }
        public int Incidents { get => Source.Incidents; set => Source.Incidents = value; }
        public RaceStatusEnum Status { get => Source.Status; set => Source.Status = value; }
        //public int RacePoints { get => Source.RacePoints; set => Source.RacePoints = value; }
        //public int BonusPoints { get => Source.BonusPoints; set => Source.BonusPoints = value; }
        public LapTime QualifyingTime { get => Source.QualifyingTime; set => Source.QualifyingTime = value; }
        public LapInterval Interval { get => Source.Interval; set => Source.Interval = value; }
        public LapTime AvgLapTime { get => Source.AvgLapTime; set => Source.AvgLapTime = value; }
        public LapTime FastestLapTime { get => Source.FastestLapTime; set => Source.FastestLapTime = value; }
        public int PositionChange { get => Source.PositionChange; }

        public int RacePoints { get => Model.RacePoints; set => Model.RacePoints = value; }
        public int BonusPoints { get => Model.BonusPoints; set => Model.BonusPoints = value; }
        public int PenaltyPoints { get => Model.PenaltyPoints; set => Model.PenaltyPoints = value; }
        public int FinalPosition { get => Model.FinalPosition; set => Model.FinalPosition = value; }
        public int TotalPoints { get => Model.TotalPoints; }

        private AddPenaltyModel addPenalty;
        public AddPenaltyModel AddPenalty { get => addPenalty; set => SetValue(ref addPenalty, value); }
        private bool isPenaltyEdit;
        public bool IsPenaltyEdit { get => isPenaltyEdit; set => SetValue(ref isPenaltyEdit, value); }

        public ScoredResultRowViewModel()
        {
            AddPenaltyCmd = new RelayCommand(async o => { await AddPenaltyToRow(Model); StartEditRowPenalty(); }, o => Model != null);
            StartEditPenaltyCmd = new RelayCommand(o => StartEditRowPenalty(), o => AddPenalty != null);
            EndEditPenaltyCmd = new RelayCommand(async o => await EndEditRowPenalty(), o => AddPenalty != null);
            DeletePenaltyCmd = new RelayCommand(async o => await DeleteRowPenalty(), o => AddPenalty != null);
        }

        public override async void OnUpdateSource()
        {
            if (Model.PenaltyPoints != 0)
            {
                try
                {
                    AddPenalty = await LeagueContext.GetModelAsync<AddPenaltyModel>(Model.ScoredResultRowId.GetValueOrDefault());
                }
                catch (Exception e)
                {
                    GlobalSettings.LogError(e);
                }
            }
            base.OnUpdateSource();
        }

        public async Task AddPenaltyToRow(ScoredResultRowModel row)
        {
            if (row == null)
                return;

            try
            {
                var newPenalty = new AddPenaltyModel(row.ScoredResultRowId);
                newPenalty = await LeagueContext.AddModelAsync(newPenalty);
                AddPenalty = newPenalty;
            }
            catch (Exception e)
            {
                GlobalSettings.LogError(e);
            }
            finally
            {

            }
        }

        public void StartEditRowPenalty()
        {
            IsPenaltyEdit = true;
        }

        public async Task EndEditRowPenalty()
        {
            if (AddPenalty == null)
                return;

            try
            {
                AddPenalty = await LeagueContext.UpdateModelAsync(AddPenalty);
                await Load(Model.ModelId);
                IsPenaltyEdit = false;
            }
            catch (Exception e)
            {
                GlobalSettings.LogError(e);
            }
            finally
            {

            }
        }

        public async Task DeleteRowPenalty()
        {
            if (AddPenalty == null)
                return;

            try
            {
                await LeagueContext.DeleteModelsAsync(AddPenalty);
                AddPenalty = null;
            }
            catch (Exception e)
            {
                GlobalSettings.LogError(e);
            }
            finally
            {

            }
        }
    }
}
