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
using System.Windows.Controls;
using System.Windows;
using iRLeagueManager.Models.Results;
using iRLeagueManager.ViewModels;

namespace iRLeagueManager.Controls
{
    public class StandingsTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            FrameworkElement element = container as FrameworkElement;

            if (element != null && item != null)
            {
                if (item is ScoringTableViewModel scoringTableViewModel)
                {
                    if (scoringTableViewModel.IsTeamScoring)
                        return element.FindResource("TeamStandingsDataGrid") as DataTemplate;
                    else
                        return element.FindResource("StandingsDataGrid") as DataTemplate;
                }
                else if (item is TeamStandingsRowModel)
                {
                    return element.FindResource("TeamStandingsRowModelDataGrid") as DataTemplate;
                }
                else if (item is StandingsRowModel)
                {
                    return element.FindResource("StandingsRowModelDataGrid") as DataTemplate;
                }
            }

            return null;
        }
    }
}
