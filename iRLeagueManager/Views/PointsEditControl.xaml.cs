using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using iRLeagueManager.Models.Results;

namespace iRLeagueManager.Views
{
    /// <summary>
    /// Interaktionslogik für PointsEditControl.xaml
    /// </summary>
    public partial class PointsEditControl : UserControl
    {
        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            if (e.Property.Name == nameof(DataContext))
                OnDataContextChanged();

            base.OnPropertyChanged(e);
        }
        public PointsType PointsType { get; set; }
        public PointsEditControl()
        {
            InitializeComponent();
        }

        protected virtual void OnDataContextChanged()
        {
            if (DataContext != null)
            {
                if (DataContext is ICollection<ScoringModel.BasePointsValue>)
                {
                    ContentControl.ContentTemplate = (DataTemplate)Resources["BasePointsTemplate"];
                }
                if (DataContext is ICollection<ScoringModel.BonusPointsValue>)
                {
                    ContentControl.ContentTemplate = (DataTemplate)Resources["BonusPointsTemplate"];
                }
            }
        }

        private void AddBasePointsButton_Click(object sender, RoutedEventArgs e)
        {
            var basePoints = DataContext as ICollection<ScoringModel.BasePointsValue>;
            var lastPoints = basePoints.LastOrDefault();
            int lastPosition = (lastPoints != null) ? lastPoints.Key : 0;
            basePoints.Add(new ScoringModel.BasePointsValue(lastPosition + 1, 0));
        }

        private void DeleteBasePointsButton_Click(object sender, RoutedEventArgs e)
        {
            var basePoints = DataContext as ICollection<ScoringModel.BasePointsValue>;
            if (basePoints.Count > 0)
                basePoints.Remove(basePoints.Last());
        }
    }
    public enum PointsType
    {
        BasePoints,
        BonusPoints,
        IncPenaltyPoints
    }
}
