using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;

using iRLeagueManager;
using iRLeagueManager.Models;
using iRLeagueManager.Models.Sessions;
using iRLeagueManager.Interfaces;
using System.Windows.Input;

using iRLeagueManager.ViewModels;

namespace iRLeagueManager.ViewModels.Collections
{
    public class ModelCollectionViewModel<TViewModel, TModel> : ObservableModelCollection<TViewModel, TModel>, IMenuItemViewModel, INotifyPropertyChanged where TViewModel : ContainerModelBase<TModel>, new() where TModel : INotifyPropertyChanged
    {
        private string text;
        public string Text { get => text; set => SetValue(ref text, value); }

        private string imgPath;
        public string ImgPath { get => imgPath; set => SetValue(ref imgPath, value); }

        private ICommand command;
        public ICommand Command { get => command; set => SetValue(ref command, value); }

        public ModelCollectionViewModel(bool updateItemSources = true) : base(updateItemSources)
        {
        }

        public ModelCollectionViewModel(IEnumerable<TModel> collection, bool updateItemSources = true) : base(collection, updateItemSources)
        {
        }

        public ModelCollectionViewModel(Action<TViewModel> constructorAction, bool updateItemSources = true) : base(constructorAction, updateItemSources)
        {
        }

        public ModelCollectionViewModel(IEnumerable<TModel> collection, Action<TViewModel> constructorAction, bool updateItemSources = true) : base(collection, constructorAction, updateItemSources)
        {
        }

        public void Refresh(String propertyName = "")
        {
            OnPropertyChanged(propertyName);
        }

        protected void OnPropertyChanged([CallerMemberName] String propertyName = "")
        {
            base.OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
        }

        protected virtual bool SetValue<TValue>(ref TValue targetProperty, TValue value, [CallerMemberName] string propertyName = "")
        {
            if (targetProperty == null)
            {
                if (value != null)
                {
                    targetProperty = value;
                    OnPropertyChanged(propertyName);
                    return true;
                }
                return false;
            }

            if (!targetProperty.Equals(value))
            {
                targetProperty = value;
                OnPropertyChanged(propertyName);
                return true;
            }
            return false;
        }

        protected virtual bool SetValue<TValue>(Func<TValue> getFunc, Action<TValue> setFunc, TValue value, [CallerMemberName] string propertyName = "")
        {
            var target = getFunc.Invoke();

            if (target == null)
            {
                if (value != null)
                {
                    setFunc.Invoke(value);
                    OnPropertyChanged(propertyName);
                    return true;
                }
                return false;
            }

            if (!target.Equals(value))
            {
                setFunc.Invoke(value);
                OnPropertyChanged(propertyName);
                return true;
            }
            return false;
        }
    }
}
