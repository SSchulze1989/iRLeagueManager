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
