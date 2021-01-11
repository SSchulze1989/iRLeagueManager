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
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

using iRLeagueManager.Data;
using iRLeagueManager.Models.User;

namespace iRLeagueManager.ViewModels
{
    /// <summary>
    /// Implements basic ViewModel functions:
    /// --> INotifyPropertyChanged
    /// </summary>
    public class ViewModelBase : INotifyPropertyChanged, IDisposable
    {
        protected LeagueContext LeagueContext => GlobalSettings.LeagueContext;

        public UserModel CurrentUserModel => LeagueContext?.UserManager?.CurrentUser;

        private bool isLoading = false;
        public bool IsLoading { get => isLoading; protected set => SetValue(ref isLoading, value); }

        public event PropertyChangedEventHandler PropertyChanged;

        private string statusMsg;
        public string StatusMsg { get => statusMsg; set => SetValue(ref statusMsg, value); }

        private bool suppressPropertyChangedEvent;
        protected bool SuppressPropertyChangedEvent { get => suppressPropertyChangedEvent; set => SetValue(ref suppressPropertyChangedEvent, value); }

        public delegate void ActionDialogEventHandler<T>(T sender, string title, string message, Action<T> okAction);

        public ICommand RefreshCmd { get; protected set; }

        public ViewModelBase()
        {
            RefreshCmd = new RelayCommand(async o => await Refresh(), o => true);
        }

        ~ViewModelBase()
        {
            Dispose(false);
        }

#pragma warning disable CS1998 // Bei der asynchronen Methode fehlen "await"-Operatoren. Die Methode wird synchron ausgeführt.
        public virtual async Task Refresh()
#pragma warning restore CS1998 // Bei der asynchronen Methode fehlen "await"-Operatoren. Die Methode wird synchron ausgeführt.
        {
            OnPropertyChanged(null);
        }

        protected virtual void OnPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (SuppressPropertyChangedEvent)
                return;

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Set field value and invoke property changed event for caller property
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="target">/param>
        /// <param name="value"></param>
        /// <param name="propertyName">Specify propertyname if it differs from caller</param>
        /// <returns>True if value has changed</returns>
        protected virtual bool SetValue<TValue>(ref TValue target, TValue value, [CallerMemberName] string propertyName = "")
        {
            return SetValue(ref target, value, (t, v) => t.Equals(v), propertyName);
        }

        /// <summary>
        /// Set field value and invoke property changed event for caller property
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="target"></param>
        /// <param name="value"></param>
        /// <param name="equalityComparer">Func for comparing equal values. Used for determining wether a value hast changed.</param>
        /// <param name="propertyName">Specify propertyname if it differs from caller</param>
        /// <returns>True if value has changed</returns>
        protected virtual bool SetValue<TValue>(ref TValue target, TValue value, Func<TValue, TValue, bool> equalityComparer, [CallerMemberName] string propertyName = "")
        {
            if (target == null)
            {
                if (value != null)
                {
                    target = value;
                    OnPropertyChanged(propertyName);
                    return true;
                }
                return false;
            }

            if (!equalityComparer(target, value))
            {
                target = value;
                OnPropertyChanged(propertyName);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="getFunc"></param>
        /// <param name="setFunc"></param>
        /// <param name="value"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
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

        #region IDisposable Support
        private bool disposedValue = false; // Dient zur Erkennung redundanter Aufrufe.

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: verwalteten Zustand (verwaltete Objekte) entsorgen.
                }

                // TODO: nicht verwaltete Ressourcen (nicht verwaltete Objekte) freigeben und Finalizer weiter unten überschreiben.
                // TODO: große Felder auf Null setzen.

                // Deatch all listeners to propertyChange event
                PropertyChanged = null;

                disposedValue = true;
            }
        }

        // TODO: Finalizer nur überschreiben, wenn Dispose(bool disposing) weiter oben Code für die Freigabe nicht verwalteter Ressourcen enthält.
        // ~ViewModelBase() {
        //   // Ändern Sie diesen Code nicht. Fügen Sie Bereinigungscode in Dispose(bool disposing) weiter oben ein.
        //   Dispose(false);
        // }

        // Dieser Code wird hinzugefügt, um das Dispose-Muster richtig zu implementieren.
        public void Dispose()
        {
            // Ändern Sie diesen Code nicht. Fügen Sie Bereinigungscode in Dispose(bool disposing) weiter oben ein.
            Dispose(true);
            // TODO: Auskommentierung der folgenden Zeile aufheben, wenn der Finalizer weiter oben überschrieben wird.
            //// GC.SuppressFinalize(this);
        }
        #endregion
    }
}
