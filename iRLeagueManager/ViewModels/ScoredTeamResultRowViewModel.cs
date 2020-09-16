using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

using iRLeagueManager.Models.Members;
using iRLeagueManager.Models.Results;
using iRLeagueManager.ViewModels.Collections;
using System.ComponentModel;
using System.Windows.Data;
using System.Runtime.CompilerServices;
using System.Collections.Specialized;

namespace iRLeagueManager.ViewModels
{
    public class ScoredTeamResultRowViewModel : ScoredResultRowViewModel, IContainerModelBase<ScoredTeamResultRowModel>
    {
        public new ScoredTeamResultRowModel Model => base.Model as ScoredTeamResultRowModel;

        public TeamModel Team => Model?.Team;

        private readonly ObservableModelCollection<ScoredResultRowViewModel, ScoredResultRowModel> scoredResultRows;
        public ICollectionView ScoredResultRows
        {
            get
            {
                if (scoredResultRows.GetSource() != Model?.ScoredResultRows)
                    scoredResultRows.UpdateSource(Model?.ScoredResultRows);
                return scoredResultRows.CollectionView;
            }
        }

        public ScoredTeamResultRowViewModel()
        {
            scoredResultRows = new ObservableModelCollection<ScoredResultRowViewModel, ScoredResultRowModel>();
            ScoredResultRows.SortDescriptions.Add(new SortDescription(nameof(FinalPosition), ListSortDirection.Ascending));
        }

        public bool UpdateSource(ScoredTeamResultRowModel source)
        {
            return base.UpdateSource(source);
        }

        ScoredTeamResultRowModel IContainerModelBase<ScoredTeamResultRowModel>.GetSource()
        {
            return Model;
        }
    }
}
