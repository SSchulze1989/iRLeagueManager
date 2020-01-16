using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using iRLeagueManager.ViewModels;
using iRLeagueManager.Models.Sessions;
using iRLeagueManager.Models.Results;
using iRLeagueManager.Interfaces;

namespace iRLeagueManager.Converters
{
    public class ModelCollectionTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            FrameworkElement element = container as FrameworkElement;
            var parent = element.Parent;

            if (element != null && item != null && item is IEnumerable<object>)
            {
                IEnumerable<object> collection = item as IEnumerable<object>;

                Type itemType = collection.GetType().GetGenericArguments()[0];
                if (itemType == typeof(ScheduleModel))
                {
                    return element.FindResource("ScheduleModelCollectionTemplate") as DataTemplate;
                }

                if (itemType == typeof(ScoringModel))
                {
                    return element.FindResource("ScoringModelCollectionTemplate") as DataTemplate;
                }

                if (itemType == typeof(ResultModel))
                {
                    return element.FindResource("ResultModelCollectionTemplate") as DataTemplate;
                }
                if (itemType.IsAssignableFrom(typeof(IHierarchicalModel)))
                {
                    return element.FindResource("HierarchicalModelTemplate") as DataTemplate;
                }
            }
            else if (item != null && item.GetType().GetInterfaces().Contains(typeof(IHierarchicalModel)))
            {
                return element.FindResource("HierarchicalModelTemplate") as DataTemplate;
            }
            return null;
        }
    }
}
