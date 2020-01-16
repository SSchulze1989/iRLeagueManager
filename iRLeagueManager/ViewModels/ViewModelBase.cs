using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace iRLeagueManager.ViewModels
{
    /// <summary>
    /// Implements basic ViewModel functions:
    /// --> NotifyPropertyChanged() Method
    /// </summary>
    public class ViewModelBase : INotifyPropertyChanged
    {
        private bool isLoading = false;
        public bool IsLoading { get => isLoading; protected set => SetValue(ref isLoading, value); }

        public event PropertyChangedEventHandler PropertyChanged;

        public void Refresh(String propertyName = "")
        {
            OnPropertyChanged(propertyName);
        }

        protected virtual void OnPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected virtual bool SetValue<TValue>(ref TValue targetProperty, TValue value, [CallerMemberName] string propertyName = "")
        {
            return SetValue(ref targetProperty, value, (t, v) => t.Equals(v), propertyName);
        }

        protected virtual bool SetValue<TValue>(ref TValue targetProperty, TValue value, Func<TValue, TValue, bool> equalityComparer, [CallerMemberName] string propertyName = "")
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

            if (!equalityComparer(targetProperty, value))
            {
                targetProperty = value;
                OnPropertyChanged(propertyName);
                return true;
            }
            return false;
        }

        protected virtual bool SetValue<TValue>(Func<TValue> getFunc, Action<TValue> setFunc, TValue value , [CallerMemberName] string propertyName = "")
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
