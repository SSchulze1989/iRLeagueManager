using iRLeagueManager;
using iRLeagueManager.Controls;
using iRLeagueManager.Converters;
using iRLeagueManager.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace iRLeagueManager.Controls
{
    public class FilterValueSelectTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            FrameworkElement element = container as FrameworkElement;
            var parent = element.Parent;

            if (element != null && item != null && item is ResultsFilterOptionViewModel filterOption)
            {
                if (filterOption.Comparator == Enums.ComparatorTypeEnum.InList)
                {
                    return element.FindResource("FlterValueListSelection") as DataTemplate;
                }
                else
                {
                    return element.FindResource("FilterValueEdit") as DataTemplate;
                }
            }
            return null;
        }
    }
}
