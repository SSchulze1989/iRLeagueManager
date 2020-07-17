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
            memberListCollectionViewSource = new CollectionViewSource()
            {
                Source = LeagueContext?.MemberList
            };
            MemberList = memberListCollectionViewSource.View;
            MemberList.Filter = ApplyFilter;
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
