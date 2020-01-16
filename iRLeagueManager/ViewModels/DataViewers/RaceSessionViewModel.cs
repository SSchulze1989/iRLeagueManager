using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iRLeagueManager.Interfaces;
using iRLeagueManager.Attributes;
using iRLeagueManager.Enums;

namespace iRLeagueManager.ViewModels
{
    public class RaceSessionModel : SessionModel, IRaceSession
    {
        public override DateTime Date
        {
            get => base.Date;
            set
            {
                base.Date = value;
                Duration = RaceLength + QualyLength + PracticeLength;
                NotifyPropertyChanged(nameof(RaceStart));
                NotifyPropertyChanged(nameof(RaceStartHours));
                NotifyPropertyChanged(nameof(RaceStartMinutes));
            }
        }

        public uint RaceId { get => ((IRaceSession)Source).RaceId; }
        public int Laps { get => ((IRaceSession)Source).Laps; set { ((IRaceSession)Source).Laps = value; NotifyPropertyChanged(); } }
        public string IrResultLink { get => ((IRaceSession)Source).IrResultLink; set { ((IRaceSession)Source).IrResultLink = value; NotifyPropertyChanged(); } }
        public string IrSessionId { get => ((IRaceSession)Source).IrSessionId; set { ((IRaceSession)Source).IrSessionId = value; NotifyPropertyChanged(); } }

        public TimeSpan PracticeLength
        {
            get => (PracticeAttached) ? ((IRaceSession)Source).PracticeLength : TimeSpan.Zero;
            set
            {
                TimeSpan raceStart = RaceStart;
                ((IRaceSession)Source).PracticeLength = value;
                RaceStart = raceStart;
                NotifyPropertyChanged();
                NotifyPropertyChanged(nameof(PracticeLengthHours));
                NotifyPropertyChanged(nameof(PracticeLengthMinutes));
            }
        }
        public int PracticeLengthHours
        {
            get => (int)PracticeLength.TotalHours;
            set
            {
                PracticeLength = ((IRaceSession)Source).PracticeLength
                    .Subtract(TimeSpan.FromHours((int)PracticeLength.TotalHours))
                    .Add(TimeSpan.FromHours(value));
                NotifyPropertyChanged();
            }
        }
        public int PracticeLengthMinutes
        {
            get => PracticeLength.Minutes;
            set
            {
                PracticeLength = ((IRaceSession)Source).PracticeLength
                  .Subtract(TimeSpan.FromMinutes(PracticeLength.Minutes))
                  .Add(TimeSpan.FromMinutes(value));
                NotifyPropertyChanged();
            }
        }

        public TimeSpan QualyLength
        {
            get => (QualyAttached) ? ((IRaceSession)Source).QualyLength : TimeSpan.Zero;
            set
            {
                TimeSpan raceStart = RaceStart;
                ((IRaceSession)Source).QualyLength = value;
                RaceStart = raceStart;
                NotifyPropertyChanged();
                NotifyPropertyChanged(nameof(QualyLengthHours));
                NotifyPropertyChanged(nameof(QualyLengthMinutes));
            }
        }
        public int QualyLengthHours
        {
            get => (int)QualyLength.TotalHours;
            set
            {
                QualyLength = ((IRaceSession)Source).QualyLength
                    .Subtract(TimeSpan.FromHours((int)QualyLength.TotalHours))
                    .Add(TimeSpan.FromHours(value));
                NotifyPropertyChanged();
            }
        }
        public int QualyLengthMinutes
        {
            get => QualyLength.Minutes;
            set
            {
                QualyLength = ((IRaceSession)Source).QualyLength
                    .Subtract(TimeSpan.FromMinutes(QualyLength.Minutes))
                    .Add(TimeSpan.FromMinutes(value));
                NotifyPropertyChanged();
            }
        }

        public TimeSpan RaceLength
        {
            get => ((IRaceSession)Source).RaceLength;
            set
            {
                ((IRaceSession)Source).RaceLength = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged(nameof(RaceLengthHours));
                NotifyPropertyChanged(nameof(RaceLengthMinutes));
            }
        }
        public int RaceLengthHours
        {
            get => (int)RaceLength.TotalHours;
            set
            {
                RaceLength = RaceLength
                    .Subtract(TimeSpan.FromHours((int)RaceLength.TotalHours))
                    .Add(TimeSpan.FromHours(value));
                NotifyPropertyChanged();
            }
        }
        public int RaceLengthMinutes
        {
            get => RaceLength.Minutes;
            set
            {
                RaceLength = RaceLength
                  .Subtract(TimeSpan.FromMinutes(RaceLength.Minutes))
                  .Add(TimeSpan.FromMinutes(value));
                NotifyPropertyChanged();
            }
        }

        public bool QualyAttached { get => ((IRaceSession)Source).QualyAttached; set { ((IRaceSession)Source).QualyAttached = value; NotifyPropertyChanged(); } }
        public bool PracticeAttached { get => ((IRaceSession)Source).PracticeAttached; set { ((IRaceSession)Source).PracticeAttached = value; NotifyPropertyChanged(); } }

        public TimeSpan RaceStart
        {
            get => 
                Date.TimeOfDay
                    .Add((QualyAttached) ? QualyLength : TimeSpan.Zero)
                    .Add((PracticeAttached) ? PracticeLength : TimeSpan.Zero);
            set
            {
                Date = Date.Subtract(Date.TimeOfDay)
                    .Add(value)
                    .Subtract((PracticeAttached) ? PracticeLength : TimeSpan.Zero)
                    .Subtract((QualyAttached) ? QualyLength : TimeSpan.Zero);
                NotifyPropertyChanged();
                NotifyPropertyChanged(nameof(RaceStartHours));
                NotifyPropertyChanged(nameof(RaceStart));
            }
        }
        public int RaceStartHours
        {
            get => (int)RaceStart.TotalHours;
            set
            {
                RaceStart = RaceStart
                  .Subtract(TimeSpan.FromHours((int)RaceStart.TotalHours))
                  .Add(TimeSpan.FromHours(value));
                NotifyPropertyChanged();
            }
        }
        public int RaceStartMinutes
        {
            get => RaceStart.Minutes;
            set
            {
                RaceStart = RaceStart
                  .Subtract(TimeSpan.FromMinutes(RaceStart.Minutes))
                  .Add(TimeSpan.FromMinutes(value));
                NotifyPropertyChanged();
            }
        }

        public RaceSessionModel() : base() { }
        public RaceSessionModel(IRaceSession source) : base(source) { }
    }
}
