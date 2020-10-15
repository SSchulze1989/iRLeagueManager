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
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using System.IO;
using Microsoft.Win32;

using iRLeagueManager;
using iRLeagueManager.Models;
using iRLeagueManager.Data;
using iRLeagueManager.Models.Sessions;
using iRLeagueManager.Interfaces;
using iRLeagueManager.Models.Results;
using iRLeagueManager.Services;
using iRLeagueManager.ViewModels.Collections;
using System.ComponentModel;
using System.Collections.Specialized;
using System.Runtime.CompilerServices;
using System.Collections;

namespace iRLeagueManager.ViewModels
{
    public class ScheduleViewModel : LeagueContainerModel<ScheduleModel>
    {
        private readonly ObservableModelCollection<SessionViewModel, SessionModel> sessions;
        public ObservableModelCollection<SessionViewModel, SessionModel> Sessions
        {
            get
            {
                if (sessions.GetSource() != Model?.Sessions)
                    sessions.UpdateSource(Model?.Sessions);
                return sessions;
            }
        }

        private bool expanded = true;
        public bool Expanded 
        { 
            get => expanded; 
            set => SetValue(ref expanded, value);
        }

        public long? ScheduleId => Model?.ScheduleId;

        public string Name { get => Model?.Name; set => Model.Name = value; }

        public int SessionCount => (Model?.SessionCount).Value;

        public DateTime? ScheduleStart => (Sessions?.Count > 0) ? (DateTime?)Sessions.Min(x => x.Date) : null;

        public DateTime? ScheduleEnd => (Sessions?.Count > 0) ? (DateTime?)Sessions.Max(x => x.Date) : null;

        public ICommand AddSessionCmd { get; }
        public ICommand DeleteSessionsCmd { get; }
        public ICommand UploadFileCmd { get; protected set; }

        private SessionViewModel selectedSession;
        public SessionViewModel SelectedSession { get => selectedSession; set => SetValue(ref selectedSession, value); }

        protected override ScheduleModel Template => ScheduleModel.GetTemplate();

        public ScheduleViewModel() : base()
        {
            Model = ScheduleModel.GetTemplate();
            sessions = new ObservableModelCollection<SessionViewModel, SessionModel>(Model?.Sessions, x => x.Schedule = this);
            ((INotifyCollectionChanged)sessions).CollectionChanged += OnSessionCollectionChange;
            Sessions.UpdateSource(new SessionModel[0]);
            AddSessionCmd = new RelayCommand(async o => await AddSessionAsync(), o => Model?.Sessions != null && (!Model?.IsReadOnly).GetValueOrDefault());
            DeleteSessionsCmd = new RelayCommand(async o => await DeleteSessionsAsync(o), o => SelectedSession != null && (!Model?.IsReadOnly).GetValueOrDefault());
            UploadFileCmd = new RelayCommand(o => UploadFile(o as SessionModel), o => false);
            Expanded = true;
        }

        public ScheduleViewModel(ScheduleModel source) : this()
        {
            Model = source;
        }

        public override void OnUpdateSource()
        {
            //if (Sessions != null)
            //    Sessions.UpdateSource(Model?.Sessions);
            //base.OnUpdateSource();
        }

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            switch(propertyName)
            {
                case nameof(Sessions):
                    //OnSessionCollectionChange(null, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
                    break;
            }
            base.OnPropertyChanged(propertyName);
        }

        private void OnSessionCollectionChange(object sender, NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged(nameof(ScheduleStart));
            OnPropertyChanged(nameof(ScheduleEnd));
            OnPropertyChanged(nameof(SessionCount));
        }

        public async Task AddSessionAsync()
        {
            if (Model?.Sessions == null)
                return;

            var newSession = new RaceSessionModel(Model);
            await AddSessionAsync(newSession);
        }

        public async Task AddSessionAsync(SessionModel session)
        {
            Model.Sessions.Add(session);

            IsLoading = true;
            try
            {
                await LeagueContext.UpdateModelAsync(Model);
            }
            catch (Exception e)
            {
                GlobalSettings.LogError(e);
            }
            finally
            {
                IsLoading = false;
            }
        }

        public async Task DeleteSessionsAsync(object selection)
        {
            if (Model == null)
                return;
            try
            {
                if (MessageBox.Show("Would you really like to delete the selected Sessions?\nThis action can not be undone!", "Delete Sessions", MessageBoxButton.YesNo) != MessageBoxResult.Yes)
                {
                    return;
                }

                var removeList = new List<SessionModel>();
                if (selection is SessionViewModel session) {
                    await LeagueContext.DeleteModelsAsync(session.GetSource());
                    removeList.Add(session.GetSource());
                }
                else if (selection is IList multiselection)
                {
                    var sessions = multiselection.OfType<SessionViewModel>();
                    await LeagueContext.DeleteModelsAsync(sessions.Select(x => x.GetSource()).ToArray());
                    removeList.AddRange(sessions.Select(x => x.GetSource()));
                }
                else if (selection == null)
                {
                    await LeagueContext.DeleteModelsAsync(SelectedSession.GetSource());
                    removeList.Add(selectedSession.GetSource());
                }

                IsLoading = true;

                if (Model.ContainsChanges)
                {
                    foreach (var removeSession in removeList)
                    {
                        Model.Sessions.Remove(removeSession);
                    }
                    await LeagueContext.UpdateModelAsync(Model);
                }
                else
                {
                    await LeagueContext.GetModelAsync<ScheduleModel>(Model.ModelId, update: false, reload: true);
                }
            }
            catch (Exception e)
            {
                GlobalSettings.LogError(e);
            }
            finally
            {
                IsLoading = false;
            }
        }

        protected override void Dispose(bool disposing)
        {
            Sessions.Dispose();
            base.Dispose(disposing);
        }

        //public override async void Load(long scheduleId)
        //{
        //    if (Model?.ScheduleId == null || Model.ScheduleId != scheduleId )
        //    {
        //        Model = ScheduleModel.GetTemplate();
        //    }

        //    try
        //    {
        //        IsLoading = true;
        //        Model = await LeagueContext.GetModelAsync<ScheduleModel>(scheduleId);
        //    }
        //    catch (Exception e)
        //    {
        //        GlobalSettings.LogError(e);
        //    }
        //    finally
        //    {
        //        IsLoading = false;
        //    }
        //}

        public async void UploadFile(SessionModel session)
        {
            OpenFileDialog openDialog = new OpenFileDialog
            {
                Filter = "CSV Dateien (*.csv)|*.csv",
                Multiselect = false
            };
            if (openDialog.ShowDialog() == false)
            {
                return;
            }

            var fileName = openDialog.FileName;

            Stream stream = File.Open(fileName, FileMode.Open, FileAccess.Read);
            ResultParserService parserService = new ResultParserService(GlobalSettings.LeagueContext);
            var lines = parserService.ParseCSV(new StreamReader(stream));
            stream.Dispose();

            //Update LeagueMember database
            var newMembers = parserService.GetNewMemberList(lines);
            foreach (var member in newMembers)
            {
                await GlobalSettings.LeagueContext.UpdateModelsAsync(newMembers);
            }
            //var sessionModel = season.GetSessions().SingleOrDefault(x => x.SessionId == session.SessionId);
            if (session == null)
                return;

            var resultRows = parserService.GetResultRows(lines);
            ResultModel result;
            if (session.SessionResult != null)
            {
                result = await LeagueContext.GetModelAsync<ResultModel>(session.SessionResult.ResultId.GetValueOrDefault());
                result.RawResults = new ObservableCollection<ResultRowModel>(resultRows);
                await GlobalSettings.LeagueContext.UpdateModelAsync(result);
            }
            else
            {
                //result = await GlobalSettings.LeagueContext.CreateResultAsync(sessionModel);
                result = new ResultModel(session);
                session.SessionResult = result;
                result.RawResults = new ObservableCollection<ResultRowModel>(resultRows);
                await GlobalSettings.LeagueContext.UpdateModelAsync(result);
                await GlobalSettings.LeagueContext.UpdateModelAsync(session);
            }
            //CurrentResult = await LeagueContext.GetModelAsync<ResultModel>(season.Results.OrderBy(x => x.Session.Date).LastOrDefault().ResultId);
        }

        //internal override bool UpdateSource(ScheduleModel source)
        //{
        //    var result = base.UpdateSource(source);
        //    Model = Source;
        //    return result;
        //}
    }
}
