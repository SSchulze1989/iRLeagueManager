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
