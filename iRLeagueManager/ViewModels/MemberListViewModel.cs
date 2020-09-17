using iRLeagueManager.Models.Members;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Documents.DocumentStructures;

namespace iRLeagueManager.ViewModels
{
    public class MemberListViewModel : ViewModelBase
    {
        private CollectionViewSource memberListCollectionViewSource;

        private ICollectionView memberList;
        public ICollectionView MemberList { get => memberList; set => SetValue(ref memberList, value); }

        private string filter;
        public string Filter { get => filter; set => SetValue(ref filter, value); }

        public ObservableCollection<Func<LeagueMember, bool>> CustomFilters { get; } = new ObservableCollection<Func<LeagueMember, bool>>();

        public MemberListViewModel()
        {
            SetCollectionViewSource(LeagueContext?.MemberList);
        }

        public void SetCollectionViewSource(IEnumerable<LeagueMember> members)
        {
            memberListCollectionViewSource = new CollectionViewSource()
            {
                Source = members
            };
            MemberList = memberListCollectionViewSource.View;
            MemberList.Filter = ApplyFilter;
            MemberList.SortDescriptions.Add(new SortDescription(nameof(LeagueMember.Lastname), ListSortDirection.Ascending));
            MemberList.SortDescriptions.Add(new SortDescription(nameof(LeagueMember.Firstname), ListSortDirection.Ascending));
        }

        private bool ApplyFilter(object item)
        {
            if ((Filter == null || Filter == "") && CustomFilters.Count == 0)
                return true;

            if (item is LeagueMember member)
            {
                bool inFilter = true;
                if (Filter != null && Filter != "")
                    inFilter &= member.FullName.ToLower().Contains(Filter.ToLower());
                foreach (var customFilter in CustomFilters)
                {
                    if (customFilter != null)
                    {
                        inFilter &= customFilter(member);
                    }
                }

                return inFilter;
            }

            return false;
        }

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            switch (propertyName)
            {
                case nameof(Filter):
                    MemberList.Refresh();
                    break;
            }

            base.OnPropertyChanged(propertyName);
        }

        public override async Task Refresh()
        {
            MemberList.Refresh();
        }
    }
}
