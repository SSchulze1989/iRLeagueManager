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
using System.Windows.Input;
using System.Windows;
using System.Collections.ObjectModel;
using System.IO;
using Microsoft.Win32;

using iRLeagueManager.Models.Sessions;
using iRLeagueManager.Models;
using iRLeagueManager.Interfaces;
using iRLeagueManager.ResultsParser;
using iRLeagueManager.Models.Results;
using iRLeagueManager.Enums;
using iRLeagueManager.Locations;
using iRLeagueManager.Timing;
using iRLeagueManager.Models.Members;
using System.Runtime.CompilerServices;
using iRLeagueManager.ViewModels.Collections;
using iRLeagueManager.Models.Reviews;
using iRLeagueManager.Models.User;
using System.Windows.Markup.Localizer;

namespace iRLeagueManager.ViewModels
{

    public static class EnumerableExtensions
    {
        public static int IndexOf<T>(this IList<T> list, Func<T, bool> selector)
        {
            int i = 0;
            foreach(var item in list)
            {
                if (selector(item))
                    return i;
                i++;
            }
            return -1;
        }
    }

    public class SessionViewModel : LeagueContainerModel<SessionModel>
    {
        //public SessionModel Model
        //{
        //    get => Source;
        //    protected set
        //    {
        //        if (SetSource(value))
        //        {
        //            OnPropertyChanged(null);
        //        }
        //    }
        //}

        public ICommand UploadFileCmd { get; }

        protected override SessionModel Template => SessionModel.GetTemplate();

        private LocationCollection Locations => GlobalSettings.Locations;

        private ScheduleViewModel schedule;
        public ScheduleViewModel Schedule { get => schedule; set => SetValue(ref schedule, value); }

        public long SessionId => (Model?.SessionId).GetValueOrDefault();

        public ObservableCollection<IncidentReviewInfo> Reviews => Model.Reviews;

        public int? SessionNumber => Schedule?.Sessions.IndexOf(x => x.SessionId == SessionId) + 1;

        public SessionType SessionType { get => Model.SessionType; set => Model.SessionType = value; }
        public DateTime FullDate { get => Model.Date; set => Model.Date = value; }
        public DateTime Date { get => Model.Date.Date; set => Model.Date = value.Date.Add(Model.Date.TimeOfDay); }
        public DateTime RaceDate => Date.Add(RaceStart);
        public TimeSpan SessionStart { get => Model.Date.TimeOfDay; set => Model.Date = Date.Date.Add(value); }
        public TimeSpan SessionEnd => SessionStart.Add(Duration);
        public TimeComponentVector TimeOfDayComponents { get; }

        public TimeSpan Duration { get => Model.Duration; set => Model.Duration = value; }
        public TimeComponentVector DurationComponents { get; }

        //public RaceTrack Track { get => Model.Location?.GetTrackInfo(); set => Model.Location = new Location(value.Configs.First()); }
        //public IEnumerable<TrackConfig> TrackConfigs { get => Track?.Configs; }
        //public TrackConfig Config { get => Model.Location?.GetConfigInfo(); set => Model.Location = new Location(value); }

        public int TrackId { get => Model.TrackId; set => Model.TrackId = value; }
        public int TrackIndex { get => TrackId - 1; set => TrackId = value + 1; }
        public int ConfigId { get => Model.ConfigId; set => Model.ConfigId = value; }
        public int ConfigIndex { get => ConfigId - 1; set => ConfigId = value + 1; }

        public RaceTrack Track => Locations.FirstOrDefault(x => x.GetTrackInfo().TrackId == TrackId)?.GetTrackInfo();
        public IEnumerable<TrackConfig> TrackConfigs => Track?.Configs;
        public TrackConfig Config => Track?.Configs.SingleOrDefault(x => x.ConfigId == ConfigId);
        public Location Location => Locations.FirstOrDefault(x => x.LocationId == Model.LocationId);

        public int Laps { get => ((Model as RaceSessionModel)?.Laps).GetValueOrDefault(); set { if (Model is RaceSessionModel race) { race.Laps = value; } } }
        public string IrResultLink { get => (Model as RaceSessionModel)?.IrResultLink; set { if (Model is RaceSessionModel race) { race.IrResultLink = value; } } }
        public string IrSessionId { get => (Model as RaceSessionModel)?.IrSessionId; set { if (Model is RaceSessionModel race) { race.IrSessionId = value; } } }
        public TimeSpan PracticeLength { get => ((Model as RaceSessionModel)?.PracticeLength).GetValueOrDefault(); set { if (Model is RaceSessionModel race) { race.PracticeLength = value; } } }
        public TimeComponentVector PracticeLenghtComponents { get; }
        public TimeSpan PracticeStart => SessionStart;
        public TimeSpan PracticeEnd => SessionStart.Add(PracticeLength);
        public TimeSpan QualyLength { get => ((Model as RaceSessionModel)?.QualyLength).GetValueOrDefault(); set { if (Model is RaceSessionModel race) { race.QualyLength = value; } } }
        public TimeComponentVector QualyLengthComponents { get; }
        public TimeSpan QualyStart => PracticeStart.Add(PracticeLength);
        public TimeSpan QualyEnd => QualyStart.Add(QualyLength);
        public TimeSpan RaceLength { get => ((Model as RaceSessionModel)?.RaceLength).GetValueOrDefault(); set { if (Model is RaceSessionModel race) { race.RaceLength = value; } } }
        public TimeComponentVector RaceLengthComponents { get; }
        public TimeSpan RaceStart => QualyStart.Add(QualyLength);
        public TimeSpan RaceEnd => RaceStart.Add(RaceLength);
        public bool QualyAttached { get => ((Model as RaceSessionModel)?.QualyAttached).GetValueOrDefault(); set { if (Model is RaceSessionModel race) { race.QualyAttached = value; } } }
        public bool PracticeAttached { get => ((Model as RaceSessionModel)?.PracticeAttached).GetValueOrDefault(); set { if (Model is RaceSessionModel race) { race.PracticeAttached = value; } } }

        public long? RaceId => (Model as RaceSessionModel)?.RaceId;

        public bool ResultAvailable => Model?.SessionResult != null;

        private bool isCurrentSession;
        public bool IsCurrentSession { get => isCurrentSession; set => SetValue(ref isCurrentSession, value); }

        public SessionViewModel() : base()
        {
            SetSource(RaceSessionModel.GetTemplate());
            TimeOfDayComponents = new TimeComponentVector(() => SessionStart, x => SessionStart = x);
            DurationComponents = new TimeComponentVector(() => Duration, x => Duration = x);
            PracticeLenghtComponents = new TimeComponentVector(() => PracticeLength, x => PracticeLength = x);
            QualyLengthComponents = new TimeComponentVector(() => QualyLength, x => QualyLength = x);
            RaceLengthComponents = new TimeComponentVector(() => RaceLength, x => RaceLength = x);
            UploadFileCmd = new RelayCommand(o => UploadFile(Model), o => !(Model?.IsReadOnly).GetValueOrDefault());
        }

        public async void UploadFile(SessionModel session)
        {
            OpenFileDialog openDialog = new OpenFileDialog
            {
                Filter = "Json Dateien (*.json)|*.json|CSV Dateien (*.csv)|*.csv",
                Multiselect = false
            };
            if (openDialog.ShowDialog() == false)
            {
                return;
            }

            var fileName = openDialog.FileName;

            Stream stream = null;

            // get file type from extension
            var ext = fileName.Split('.').Last().ToLower();

            ResultsFileTypeEnum resultsFileType;
            switch (ext)
            {
                case "csv":
                    resultsFileType = ResultsFileTypeEnum.CSV;
                    break;
                case "json":
                    resultsFileType = ResultsFileTypeEnum.Json;
                    break;
                default:
                    resultsFileType = ResultsFileTypeEnum.CSV;
                    break;
            }
            var parserService = ResultsParserFactory.GetResultsParser(resultsFileType);
            //IEnumerable<Dictionary<string, string>> lines = null; 

            try
            {
                stream = File.Open(fileName, FileMode.Open, FileAccess.Read);
                await parserService.ReadStreamAsync(new StreamReader(stream, Encoding.UTF8));
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString(), "Error parsing result File", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            finally
            {
                stream?.Dispose();
            }


            try
            {
                //Update LeagueMember database
                await LeagueContext.UpdateMemberList();
                var memberList = LeagueContext.MemberList;
                parserService.MemberList = memberList;
                var newMembers = parserService.GetNewMemberList().Where(x => !memberList.Any(y => y.IRacingId == x.IRacingId));

                newMembers = await LeagueContext.AddModelsAsync(newMembers.ToArray());
                foreach(var member in newMembers)
                {
                    LeagueContext.MemberList.Add(member);
                }
                await LeagueContext.UpdateModelsAsync(LeagueContext.MemberList);

                //await GlobalSettings.LeagueContext.UpdateModelsAsync(newMembers);
                //var sessionModel = season.GetSessions().SingleOrDefault(x => x.SessionId == session.SessionId);
                if (session == null)
                    return;

                var resultRows = parserService.GetResultRows();
                ResultModel result;
                if (session.SessionResult != null)
                {
                    result = await LeagueContext.GetModelAsync<ResultModel>(session.SessionResult.ResultId.GetValueOrDefault());
                    resultRows = result.RawResults.MapToCollection(resultRows);                    
                    await LeagueContext.DeleteModelsAsync(result.RawResults.Except(resultRows).ToArray());
                    resultRows.ToList().ForEach(x => x.ResultId = result.ResultId.GetValueOrDefault());
                    resultRows = (await LeagueContext.AddModelsAsync(resultRows.ToArray()));
                    result.RawResults = new ObservableCollection<ResultRowModel>(resultRows);
                    await GlobalSettings.LeagueContext.UpdateModelAsync(result);
                }
                else
                {
                    //result = await GlobalSettings.LeagueContext.CreateResultAsync(sessionModel);
                    result = new ResultModel(session);
                    result = await LeagueContext.AddModelAsync(result);
                    session.SessionResult = result;
                    //result = await GlobalSettings.LeagueContext.UpdateModelAsync(result);
                    //session = await GlobalSettings.LeagueContext.UpdateModelAsync(session);
                    resultRows.ToList().ForEach(x => x.ResultId = result.ResultId.GetValueOrDefault());
                    await LeagueContext.AddModelsAsync(resultRows.ToArray());
                    result.RawResults = new ObservableCollection<ResultRowModel>(resultRows);

                    //await GlobalSettings.LeagueContext.UpdateModelAsync(result);
                    await GlobalSettings.LeagueContext.UpdateModelAsync(session);
                }
            //CurrentResult = await LeagueContext.GetModelAsync<ResultModel>(season.Results.OrderBy(x => x.Session.Date).LastOrDefault().ResultId);
            }
            catch (Exception e)
            {
                GlobalSettings.LogError(e);
            }
            finally
            {
            }
        }

        public async Task DeleteResultFile()
        {
            if (Model == null)
                return;

            try
            {
                await LeagueContext.DeleteModelAsync<ResultModel>(SessionId);
                await LeagueContext.UpdateModelAsync(Model);
            }
            catch (Exception e)
            {
                GlobalSettings.LogError(e);
            }
            finally
            {

            }
        }

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            base.OnPropertyChanged(propertyName);

            switch (propertyName)
            {
                case nameof(Schedule):
                    OnPropertyChanged(nameof(SessionNumber));
                    break;
                case nameof(Model.LocationId):
                    OnPropertyChanged(nameof(Track));
                    OnPropertyChanged(nameof(TrackId));
                    OnPropertyChanged(nameof(TrackIndex));
                    OnPropertyChanged(nameof(TrackConfigs));
                    OnPropertyChanged(nameof(Config));
                    OnPropertyChanged(nameof(ConfigId));
                    OnPropertyChanged(nameof(ConfigIndex));
                    OnPropertyChanged(nameof(Location));
                    break;
                case nameof(Model.SessionResult):
                    OnPropertyChanged(nameof(ResultAvailable));
                    break;
                case nameof(Duration):
                    OnPropertyChanged(nameof(SessionEnd));
                    OnPropertyChanged(nameof(DurationComponents));
                    break;
                case nameof(Date):
                    OnPropertyChanged(null);
                    break;
                case nameof(PracticeLength):
                    OnPropertyChanged(nameof(PracticeLenghtComponents));
                    OnPropertyChanged(nameof(PracticeEnd));
                    OnPropertyChanged(nameof(QualyStart));
                    OnPropertyChanged(nameof(QualyEnd));
                    OnPropertyChanged(nameof(RaceStart));
                    OnPropertyChanged(nameof(RaceEnd));
                    OnPropertyChanged(nameof(RaceDate));
                    break;
                case nameof(QualyLength):
                    OnPropertyChanged(nameof(QualyLengthComponents));
                    OnPropertyChanged(nameof(QualyEnd));
                    OnPropertyChanged(nameof(RaceStart));
                    OnPropertyChanged(nameof(RaceEnd));
                    OnPropertyChanged(nameof(RaceDate));
                    break;
                case nameof(RaceLength):
                    OnPropertyChanged(nameof(QualyLengthComponents));
                    OnPropertyChanged(nameof(QualyEnd));
                    OnPropertyChanged(nameof(RaceStart));
                    OnPropertyChanged(nameof(RaceEnd));
                    break;
                default:
                    break;
            }
        }
    }
}
