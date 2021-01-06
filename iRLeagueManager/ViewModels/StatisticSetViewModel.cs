using iRLeagueManager.Models.Statistics;
using iRLeagueManager.Timing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iRLeagueManager.ViewModels
{
    public class StatisticSetViewModel : LeagueContainerModel<StatisticSetModel>
    {
        public long Id => Model.Id;
        public string Name { get => Model.Name; set => Model.Name = value; }
        public TimeSpan UpdateInterval { get => Model.UpdateInterval; set => Model.UpdateInterval = value; }
        public DateTime UpdateTime { get => Model.Updatetime; set => Model.Updatetime = value; }
        public string StatisticSetType => Model.StatisticSetType;

        public TimeComponentVector UpdateIntervalComponents { get; }
        public DateTime UpdateTimeDate { get => UpdateTime.Date; set => UpdateTime = value.Date.Add(UpdateTime.TimeOfDay); }
        public TimeComponentVector UpdateTimeComponents { get; }

        private readonly DriverStatisticViewModel driverStatistic;
        public DriverStatisticViewModel DriverStatistic
        { 
            get
            {
                if (driverStatistic.Model.StatisticSetId != Id)
                {
                    _ = driverStatistic.Load(Id);
                }
                return driverStatistic;
            }
        }

        public virtual string ShortDescription => Name;

        protected override StatisticSetModel Template => new StatisticSetModel();

        public StatisticSetViewModel()
        {
            UpdateIntervalComponents = new TimeComponentVector(() => UpdateInterval, x => UpdateInterval = x);
            UpdateTimeComponents = new TimeComponentVector(() => UpdateTime.TimeOfDay, x => UpdateTime.Date.Add(x));
            driverStatistic = new DriverStatisticViewModel();
        }

        public override async Task Refresh()
        {
            LeagueContext.ModelManager.ForceExpireModels(new StatisticSetModel[] { Model });
            LeagueContext.ModelManager.ForceExpireModels(new DriverStatisticModel[] { DriverStatistic.Model });
            await Load(Id);
            await base.Refresh();
        }

        public static StatisticSetViewModel GetStatisticSetViewModel(Type modelType)
        {
            if (modelType.Equals(typeof(SeasonStatisticSetModel)))
            {
                return new SeasonStatisticSetViewModel();
            }
            else if (modelType.Equals(typeof(LeagueStatisticSetModel)))
            {
                return new LeagueStatisticSetViewModel();
            }
            else if (modelType.Equals(typeof(ImportedStatisticSetModel)))
            {
                return new ImportedStatisticSetViewModel();
            }
            else
            {
                return new StatisticSetViewModel();
            }
        }
    }
}
