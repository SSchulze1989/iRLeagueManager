﻿using AutoMapper.Configuration.Conventions;
using iRLeagueManager.Models.Members;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations.Builders;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace iRLeagueManager.Models.Results
{
    public class StandingsRowModel : MappableModel
    {
        private ScoringInfo scoring;
        public ScoringInfo Scoring { get => scoring; internal set => SetValue(ref scoring, value); }

        private LeagueMember member;
        public LeagueMember Member { get => member; internal set => SetValue(ref member, value); }
        public override long[] ModelId => new long[] { Scoring.ScoringId.GetValueOrDefault(), Member.MemberId.GetValueOrDefault() };

        private int position;
        public int Position { get => position; internal set => SetValue(ref position, value); }

        private int lastPosition;
        public int LastPosition { get => lastPosition; internal set => SetValue(ref lastPosition, value); }

        private int classId;
        public int ClassId { get => classId; internal set => SetValue(ref classId, value); }

        private string carClass;
        public string CarClass { get => carClass; internal set => SetValue(ref carClass, value); }

        private int racePoints;
        public int RacePoints { get => racePoints; internal set => SetValue(ref racePoints, value); }

        private int racePointsChange;
        public int RacePointsChange { get => racePointsChange; internal set => SetValue(ref racePointsChange, value); }

        private int penaltyPoints;
        public int PenaltyPoints { get => penaltyPoints; internal set => SetValue(ref penaltyPoints, value); }

        private int penaltyPointsChange;
        public int PenaltyPointsChange { get => penaltyPointsChange; internal set => SetValue(ref penaltyPointsChange, value); }

        private int totalPoints;
        public int TotalPoints { get => totalPoints; internal set => SetValue(ref totalPoints, value); }

        private int totalPointsChange;
        public int TotalPointsChange { get => totalPointsChange; internal set => SetValue(ref totalPointsChange, value); }

        private int races;
        public int Races { get => races; internal set => SetValue(ref races, value); }

        private int racesCounted;
        public int RacesCounted { get => racesCounted; internal set => SetValue(ref racesCounted, value); }

        private int droppedResults;
        public int DroppedResults { get => droppedResults; internal set => SetValue(ref droppedResults, value); }

        private int completedLaps;
        public int CompletedLaps { get => completedLaps; internal set => SetValue(ref completedLaps, value); }

        private int completedLapsChange;
        public int CompletedLapsChange { get => completedLapsChange; internal set => SetValue(ref completedLapsChange, value); }

        private int leadLaps;
        public int LeadLaps { get => leadLaps; internal set => SetValue(ref leadLaps, value); }

        private int leadLapsChange;
        public int LeadLapsChange { get => leadLapsChange; internal set => SetValue(ref leadLapsChange, value); }

        private int fastestLaps;
        public int FastestLaps { get => fastestLaps; internal set => SetValue(ref fastestLaps, value); }

        private int fastestLapsChange;
        public int FastestLapsChange { get => fastestLapsChange; internal set => SetValue(ref fastestLapsChange, value); }

        private int polePositions;
        public int PolePositions { get => polePositions; internal set => SetValue(ref polePositions, value); }

        private int polePositionsChange;
        public int PolePositionsChange { get => polePositionsChange; internal set => SetValue(ref polePositionsChange, value); }

        private int wins;
        public int Wins { get => wins; internal set => SetValue(ref wins, value); }

        private int winsChange;
        public int WinsChange { get => winsChange; internal set => SetValue(ref winsChange, value); }

        private int top3;
        public int Top3 { get => top3; internal set => SetValue(ref top3, value); }

        private int top5;
        public int Top5 { get => top5; internal set => SetValue(ref top5, value); }

        private int top10;
        public int Top10 { get => top10; internal set => SetValue(ref top10, value); }

        private int incidents;
        public int Incidents { get => incidents; internal set => SetValue(ref incidents, value); }

        private int incidentsChange;
        public int IncidentsChange { get => incidentsChange; internal set => SetValue(ref incidentsChange, value); }

        private int positionChange;
        public int PositionChange { get => positionChange; internal set => SetValue(ref positionChange, value); }
    }
}