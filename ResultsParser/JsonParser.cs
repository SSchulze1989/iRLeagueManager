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

using iRLeagueManager.Models.Members;
using iRLeagueManager.Models.Results;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using iRLeagueManager.Enums;
using iRLeagueManager.Timing;

namespace iRLeagueManager.ResultsParser
{
    public class JsonParser : IResultsParser
    {
        private dynamic ResultData { get; set; }
        private dynamic SessionResults { get; set; }

        public IEnumerable<LeagueMember> MemberList { get; set; } = new List<LeagueMember>();
        public IEnumerable<TeamModel> TeamList { get; set; }

        public IEnumerable<LeagueMember> GetNewMemberList()
        {
            var memberList = new List<LeagueMember>();

            foreach (var result in SessionResults)
            {
                IRacingResultRow row = new IRacingResultRow();
                if (!MemberList.Any(x => x.IRacingId == (string)result.cust_id))
                {
                    //var newMember = LeagueClient.AddNewMember(line["Name"].Split(' ').First(), line["Name"].Split(' ').Last());
                    var newMember = new LeagueMember(0, ((string)result.display_name).Split(' ').First(), ((string)result.display_name).Split(' ').Skip(1).Aggregate((x, y) => x + " " + y));
                    //LeagueContext.MemberList.Add(newMember);
                    newMember.IRacingId = (string)result.cust_id;
                    //row.MemberId = newMember.MemberId;
                    row.Member = newMember;
                    memberList.Add(newMember);
                }
                else
                {
                    var member = MemberList.SingleOrDefault(x => x.IRacingId == (string)result.cust_id);
                    var names = ((string)result.display_name).Split(' ');
                    member.Firstname = names.First();
                    member.Lastname = names.Skip(1).Aggregate((x, y) => x + " " + y);
                }
            }
            return memberList;
        }

        public IEnumerable<ResultRowModel> GetResultRows()
        {
            List<IRacingResultRow> resultRows = new List<IRacingResultRow>();

            foreach (var resultRow in SessionResults)
            {
                IRacingResultRow row = new IRacingResultRow
                {
                    //FinalPosition = int.Parse(line["FinPos"]),
                    StartPosition = (int)resultRow.starting_position + 1,
                    IRacingId = (string)resultRow.cust_id,
                    FinishPosition = (int)resultRow.finish_position + 1,
                    CarNumber = resultRow.livery.car_number,
                    ClassId = resultRow.car_class_id,
                    Car = "",
                    CarClass = "",
                    CompletedLaps = resultRow.laps_complete,
                    LeadLaps = resultRow.laps_lead,
                    FastLapNr = resultRow.best_lap_num,
                    Incidents = resultRow.incidents,
                    Status = Enum.TryParse((string)resultRow.reason_out, out RaceStatusEnum statusEnum) ? statusEnum : RaceStatusEnum.Unknown,
                    QualifyingTime = new LapTime(TimeSpan.Zero)
                };
                var eventLaps = (int)ResultData.event_laps_complete;
                row.CompletedPct = eventLaps >= row.CompletedLaps ? (row.CompletedLaps / eventLaps)*100 : 100;
                //if (!LeagueClient.LeagueMembers.ToList().Exists(x => x.IRacingId == row.IRacingId))
                if (MemberList.Any(x => x.IRacingId == (string)resultRow.cust_id))
                {
                    row.Member = MemberList.SingleOrDefault(x => x.IRacingId == (string)resultRow.cust_id);
                }
                //row.Interval = new LapInterval(
                //    TimeSpan.TryParse("0:" + line["Interval"].Replace("-",""), culture, out TimeSpan intvTime) ? intvTime : TimeSpan.Zero,
                //    int.TryParse(line["Interval"].Replace("L", ""), out int intvLaps) ? intvLaps : 0);
                //row.Interval = new LapInterval(GetTimeSpanFromString(line["Interval"]), int.TryParse(line["Interval"].Replace("L", ""), out int intvLaps) ? intvLaps : 0);
                row.Interval = resultRow.interval >= 0 ? new LapInterval(new TimeSpan((long)resultRow.interval*(TimeSpan.TicksPerMillisecond/10))) : new LapInterval(TimeSpan.Zero, (int)ResultData.event_laps_complete - row.CompletedLaps);
                //row.AvgLapTime = new LapTime(TimeSpan.TryParse(PrepareTimeString(line["AverageLapTime"]), culture, out TimeSpan avgLap) ? avgLap : TimeSpan.Zero);
                row.AvgLapTime = new LapTime(new TimeSpan((long)resultRow.average_lap*(TimeSpan.TicksPerMillisecond / 10)));
                //row.FastestLapTime = new LapTime(TimeSpan.TryParse(PrepareTimeString(line["FastestLapTime"]), culture, out TimeSpan fastLap) ? fastLap : TimeSpan.Zero);
                row.FastestLapTime = new LapTime(new TimeSpan((long)resultRow.best_lap_time*(TimeSpan.TicksPerMillisecond / 10)));
                //row.PositionChange = row.StartPosition - row.FinishPosition;
                row.OldIRating = resultRow.oldi_rating;
                row.NewIRating = resultRow.newi_rating;
                row.OldSafetyRating = ((double)resultRow.old_sub_level) / 100;
                row.NewSafetyRating = ((double)resultRow.new_sub_level) / 100;

                resultRows.Add(row);
            }
            return resultRows;
        }

        public async Task ReadStreamAsync(StreamReader reader)
        {
            var jsonString = await reader.ReadToEndAsync();
         //   var result = JsonConvert.DeserializeAnonymousType(jsonString, new
         //   {
         //       subsession_id = "",
         //       session_name = "",
         //       num_laps_for_qual_average = 0,
         //       num_laps_for_solo_average = 0,
         //       event_type = 0,
         //       event_type_name = "",
         //       driver_changes = false,
         //       max_weeks = 0,
         //       points_type = "",
         //       num_cautions = "",
         //       track = new
         //       {
         //           track_id = 0,
         //           track_name = "",
         //           config_name = "",
         //           category_id = 0,
         //           category = ""
         //       },
         //       session_results = new[]
         //       {
         //           new
         //           {
         //               cust_id = 0,
         //               display_name = "",
         //               finish_position = 0,
         //               finish_position_in_class = 0,
         //               laps_lead = 0,
					    //laps_complete = 0,
					    //opt_laps_complete = 0,
					    //interval = TimeSpan.Zero,
					    //class_interval = TimeSpan.Zero,
					    //average_lap = TimeSpan.Zero,
					    //best_lap_num = 0,
					    //best_lap_time = TimeSpan.Zero,
					    //best_nlaps_num = 0,
					    //best_nlaps_time = TimeSpan.Zero,
					    //best_qual_lap_at = DateTime.MinValue,
					    //reason_out_id = 0,
					    //reason_out = "",
					    //champ_points = 0,
					    //drop_race = false,
					    //club_points = 0,
					    //position = 0,
					    //qual_lap_time = 0,
					    //starting_position = 0,
					    //car_class_id = 0,
					    //club_id = 0,
					    //club_name = "",
					    //club_shortname = "",
					    //division = 0,
					    //old_license_level = 0,
					    //old_sub_level = 0,
					    //old_cpi = 0,
					    //oldi_rating = 0,
					    //old_ttrating = 0,
					    //new_license_level = 0,
					    //new_sub_level = 0,
					    //new_cpi = 0,
					    //newi_rating = 0,
					    //new_ttrating = 0,
					    //multiplier = 0,
					    //license_change_oval = 0,
					    //license_change_road = 0,
					    //incidents = 0,
					    //max_percent_fuel_fill = 0,
					    //weight_penalty_kg = 0,
					    //league_points = 0,
					    //car_id = 0,
					    //aggregate_champ_points = 0,
         //           }
         //       }
         //   });

            dynamic dynResult = JsonConvert.DeserializeObject(jsonString);

            ResultData = dynResult;
            SessionResults = ResultData.session_results[0].results;
        }

        public SimSessionDetails GetSessionDetails()
        {
            var details = new SimSessionDetails();
            details.Category = ResultData.track.category;
            details.ConfigName = ResultData.track.track_name;
            details.CornersPerLap = ResultData.corners_per_lap;
            details.EventLapsComplete = ResultData.event_laps_complete;
            details.EndTime = ResultData.end_time;
            details.EventStrengthOfField = ResultData.event_strength_of_field;
            details.Fog = ResultData.weather.fog;
            details.DamageModel = ResultData.damage_model;
            details.LeaveMarbles = ResultData.track_state.leave_marbles;
            details.LicenseCategory = ResultData.license_category_id;
            details.NumCautionLaps = ResultData.num_caution_laps;
            details.NumCautions = ResultData.num_cautions;
            details.NumLeadChanges = ResultData.num_lead_changes;
            details.MaxWeeks = ResultData.max_weeks;
            details.PracticeGripCompound = ResultData.track_state.practice_grip_compound;
            details.PracticeRubber = ResultData.track_state.practice_rubber;
            details.QualifyGripCompund = ResultData.track_state.qualify_grip_compound;
            details.QualifyRubber = ResultData.track_state.qualify_rubber;
            details.RaceGripCompound = ResultData.track_state.race_grip_compound;
            details.RaceRubber = ResultData.track_state.race_rubber;
            details.RelHumidity = ResultData.weather.rel_humidity;
            details.TimeOfDay = ResultData.time_of_day;
            details.TempUnits = ResultData.weather.temp_units;
            details.TempValue = ResultData.weather.temp_value;
            details.TrackCategoryId = ResultData.track.category_id;
            details.TrackName = ResultData.track.track_name;
            details.IRTrackId = ResultData.track.track_id;
            //details.SimStartUTCTime = DateTime.TryParse(ResultData.weather.simulated_start_utc_time, out DateTime simStartUTCTime) ? (DateTime?)simStartUTCTime : null;
            details.SimStartUTCTime = ResultData.weather.simulated_start_utc_time;
            details.SimStartUTCOffset = ResultData.weather.simulated_start_utc_offset;
            details.SessionName = ResultData.session_name;
            details.Skies = ResultData.weather.skies;
            details.StartTime = ResultData.start_time;
            details.IRSeasonId = ResultData.season_id;
            details.IRRaceWeek = ResultData.race_week_num;
            details.IRSeasonName = ResultData.season_name;
            details.IRSeasonQuarter = ResultData.season_quarter;
            details.IRSeasonYear = ResultData.season_year;
            details.IRSessionId = ResultData.session_id;
            details.IRSubsessionId = ResultData.subsession_id;
            details.WarmupGripCompound = ResultData.track_state.warmup_grip_compound;
            details.WarmupRubber = ResultData.track_state.warmup_rubber;
            details.WeatherType = ResultData.weather.type;
            details.WeatherVarInitial = ResultData.weather.weather_var_initial;
            details.WeatherVarOngoing = ResultData.weather.weather_var_ongoing;
            details.WindDir = ResultData.weather.wind_dir;
            details.WindUnits = ResultData.weather.wind_units;

            return details;
        }
    }
}
