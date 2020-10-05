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
                    StartPosition = resultRow.starting_position,
                    IRacingId = (string)resultRow.cust_id,
                    FinishPosition = resultRow.finish_position,
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
                //if (!LeagueClient.LeagueMembers.ToList().Exists(x => x.IRacingId == row.IRacingId))
                if (MemberList.Any(x => x.IRacingId == resultRow.cust_id))
                {
                    row.Member = MemberList.SingleOrDefault(x => x.IRacingId == resultRow.cust_id);
                }
                //row.Interval = new LapInterval(
                //    TimeSpan.TryParse("0:" + line["Interval"].Replace("-",""), culture, out TimeSpan intvTime) ? intvTime : TimeSpan.Zero,
                //    int.TryParse(line["Interval"].Replace("L", ""), out int intvLaps) ? intvLaps : 0);
                //row.Interval = new LapInterval(GetTimeSpanFromString(line["Interval"]), int.TryParse(line["Interval"].Replace("L", ""), out int intvLaps) ? intvLaps : 0);
                row.Interval = new LapInterval(new TimeSpan((long)resultRow.interval*(TimeSpan.TicksPerMillisecond/10)));
                //row.AvgLapTime = new LapTime(TimeSpan.TryParse(PrepareTimeString(line["AverageLapTime"]), culture, out TimeSpan avgLap) ? avgLap : TimeSpan.Zero);
                row.AvgLapTime = new LapTime(new TimeSpan((long)resultRow.average_lap*(TimeSpan.TicksPerMillisecond / 10)));
                //row.FastestLapTime = new LapTime(TimeSpan.TryParse(PrepareTimeString(line["FastestLapTime"]), culture, out TimeSpan fastLap) ? fastLap : TimeSpan.Zero);
                row.FastestLapTime = new LapTime(new TimeSpan((long)resultRow.best_lap_time*(TimeSpan.TicksPerMillisecond / 10)));
                //row.PositionChange = row.StartPosition - row.FinishPosition;
                row.OldIRating = resultRow.old_irating;
                row.NewIRating = resultRow.new_irating;
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
    }
}
