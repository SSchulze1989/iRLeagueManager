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
            if (Filter == null || Filter == "")
                return true;

            if (item is LeagueMember member)
            {
                return member.FullName.ToLower().Contains(Filter.ToLower());
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
    }
}
