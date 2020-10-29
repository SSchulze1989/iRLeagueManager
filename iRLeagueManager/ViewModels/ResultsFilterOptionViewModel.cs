using iRLeagueManager.Enums;
using iRLeagueManager.Models;
using iRLeagueManager.Models.Filters;
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
        protected override ResultsFilterOptionModel Template => new ResultsFilterOptionModel() { FilterValues = new ObservableCollection<object>() };

        public long ResultsFilterId => Model.ResultsFilterId;
        public long ScoringId => Model.ScoringId;
        public string ResultsFilterType { get => Model.ResultsFilterType; set => Model.ResultsFilterType = value; }
        public string ColumnPropertyName { get => Model.ColumnPropertyName; set => Model.ColumnPropertyName = value; }
        public ComparatorTypeEnum Comparator { get => Model.Comparator; set => Model.Comparator = value; }
        public bool Exclude { get => Model.Exclude; set => Model.Exclude = value; }
        public ObservableCollection<object> FilterValues => Model.FilterValues;
        public Type ColumnPropertyType => typeof(ResultRowModel).GetProperty(ColumnPropertyName ?? "")?.PropertyType;
        public string FilterValueString 
        { 
            get => string.Join(";", FilterValues); 
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
                FilterValues.Add(value);
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

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            base.OnPropertyChanged(propertyName);

            switch (propertyName)
            {
                case nameof(ColumnPropertyName):
                    OnPropertyChanged(nameof(ColumnPropertyType));
                    break;
            }
        }

        public bool CanSubmit()
        {
            return ResultsFilterType != null && String.IsNullOrEmpty(ColumnPropertyName) == false && FilterValues.Count > 0;
        }
    }
}
