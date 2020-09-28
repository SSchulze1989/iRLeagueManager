// MIT License

// Copyright (c) 2020 Simon Schulze

// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:

// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.

// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

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
