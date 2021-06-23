using iRLeagueManager.Enums;
using iRLeagueManager.Models;
using iRLeagueManager.Models.Filters;
using iRLeagueManager.Models.Members;
using iRLeagueManager.Models.Results;
using iRLeagueManager.ViewModels.Collections;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;

namespace iRLeagueManager.ViewModels
{
    public class ResultsFilterOptionViewModel : LeagueContainerModel<ResultsFilterOptionModel>
    {
        protected override ResultsFilterOptionModel Template => new ResultsFilterOptionModel() { FilterValues = new ObservableCollection<FilterValueModel>() };

        public long ResultsFilterId => Model.ResultsFilterId;
        public long ScoringId => Model.ScoringId;
        public string ResultsFilterType { get => Model.ResultsFilterType; set => Model.ResultsFilterType = value; }
        public string ColumnPropertyName { get => Model.ColumnPropertyName; set => Model.ColumnPropertyName = value; }
        public ComparatorTypeEnum Comparator { get => Model.Comparator; set => Model.Comparator = value; }
        public bool Exclude { get => Model.Exclude; set => Model.Exclude = value; }
        public bool FilterPointsOnly { get => Model.FilterPointsOnly; set => Model.FilterPointsOnly = value; }
        public ObservableCollection<FilterValueModel> FilterValues => Model.FilterValues;
        public IEnumerable<int> IntFilterValues => Model.FilterValues.OfType<int>();
        public Type ColumnPropertyType => Model.ColumnPropertyType;
        public MemberListViewModel MemberList => new MemberListViewModel();
        public bool IsMemberType => ColumnPropertyType == typeof(LeagueMember);

        public string FilterValueString 
        { 
            get => string.Join(";", FilterValues.Select(x => x.Value)); 
            set 
            {
                try
                {
                    Convert.ChangeType(value, ColumnPropertyType);
                }
                catch (InvalidCastException e)
                {
                    throw new ArgumentException("Invalid value", e);
                }
                FilterValues.Clear();
                FilterValues.Add(new FilterValueModel(ColumnPropertyType, value));
            }
        }

        public ResultsFilterOptionViewModel()
        {
        }

        public IEnumerable<string> GetEnumValues(Type enumType)
        {
            if (typeof(Enum).IsAssignableFrom(enumType) == false)
            {
                return null;
            }

            return Enum.GetNames(enumType);
        }

        public void RefreshFilterValueString()
        {
            OnPropertyChanged(nameof(FilterValueString));
        }

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            base.OnPropertyChanged(propertyName);

            switch (propertyName)
            {
                case nameof(ColumnPropertyName):
                    OnPropertyChanged(nameof(ColumnPropertyType));
                    break;
                case nameof(ColumnPropertyType):
                    OnPropertyChanged(nameof(IsMemberType));
                    break;
            }
        }

        public bool CanSubmit()
        {
            return ResultsFilterType != null && String.IsNullOrEmpty(ColumnPropertyName) == false && FilterValues.Count > 0;
        }
    }
}
