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

using iRLeagueManager.Enums;
using iRLeagueManager.Data;
using iRLeagueManager.Models.Members;
using iRLeagueManager.Models.Results;
using iRLeagueManager.Timing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace iRLeagueManager.ResultsParser
{
    public class CSVParser : IResultsParser
    {
        //public ILeague LeagueClient { get; set; }

        //public LeagueContext LeagueContext { get; set; }

        private IEnumerable<IDictionary<string, string>> DataLines { get; set; }

        public IEnumerable<LeagueMember> MemberList { get; set; }

        public IEnumerable<TeamModel> TeamList { get; set; }

        public int HeaderLine { get; set; } = 9;

        public int FirstDataLine { get; set; } = 10;

        public char Delimiter { get; set; } = ',';

        public CSVParser() { }

        //public CSVParser(LeagueContext context)
        //{
        //    //LeagueClient = leagueClient;
        //    LeagueContext = context;
        //}

        public async Task ReadStreamAsync(StreamReader reader)
        {
            int currentLine = 0;

            if (FirstDataLine <= HeaderLine)
            {
                throw new ArgumentException("First data line is before header line. Header must always be above data!");
            }

            IEnumerable<string> Header = new string[0];
            var dataLines = new List<Dictionary<string, string>>();

            while (!reader.EndOfStream)
            {
                string line = await reader.ReadLineAsync();
                currentLine++;
                if (currentLine == HeaderLine)
                {
                    Header = line.Split(Delimiter).Select(x => x.Replace(" ", "").Replace("\"", ""));
                }
                if (currentLine >= FirstDataLine)
                {
                    IEnumerable<string> data = line.Replace("\"", "").Split(Delimiter);
                    dataLines.Add(data.Select((x, i) => new { k = Header.ElementAt(i), v = x }).ToDictionary(x => x.k, x => x.v));
                }
            }

            DataLines = dataLines;
        }

        public IEnumerable<string> GetResultNames()
        {
            return new string[] { "RACE" };
        }

        public IEnumerable<LeagueMember> GetNewMemberList()
        {
            var memberList = new List<LeagueMember>();

            foreach (var line in DataLines)
            {
                IRacingResultRow row = new IRacingResultRow();
                if (!MemberList.Any(x => x.IRacingId == line["CustID"]))
                {
                    if (MemberList.Any(x => x.IRacingId == "" && x.FullName == line["Name"]))
                    {
                        var member = memberList.SingleOrDefault(x => x.FullName == line["Name"]);
                        member.IRacingId = line["CustID"];
                    }
                    else
                    {
                        //var newMember = LeagueClient.AddNewMember(line["Name"].Split(' ').First(), line["Name"].Split(' ').Last());
                        var newMember = new LeagueMember(0, line["Name"].Split(' ').First(), line["Name"].Split(' ').Skip(1).Aggregate((x, y) => x + " " + y));
                        //LeagueContext.MemberList.Add(newMember);
                        newMember.IRacingId = line["CustID"];
                        //row.MemberId = newMember.MemberId;
                        row.Member = newMember;
                        memberList.Add(newMember);
                    }
                }
                else
                {
                    var member = MemberList.SingleOrDefault(x => x.IRacingId == line["CustID"]);
                    var names = line["Name"].Split(' ');
                    member.Firstname = names.First();
                    member.Lastname = names.Skip(1).Aggregate((x, y) => x + " " + y);
                }
            }
            return memberList;
        }

        public IEnumerable<ResultRowModel> GetResultRows(string resultName)
        {
            List<IRacingResultRow> resultRows = new List<IRacingResultRow>();

            var culture = System.Globalization.CultureInfo.GetCultureInfo("de-EN");

            foreach (var line in DataLines)
            {
                IRacingResultRow row = new IRacingResultRow
                {
                    //FinalPosition = int.Parse(line["FinPos"]),
                    StartPosition = int.Parse(line["StartPos"]),
                    IRacingId = line["CustID"],
                    FinishPosition = int.Parse(line["FinPos"]),
                    CarNumber = int.Parse(line["Car#"]),
                    ClassId = int.Parse(line["CarClassID"]),
                    Car = line["Car"],
                    CarClass = line["CarClass"],
                    CompletedLaps = int.Parse(line["LapsComp"]),
                    LeadLaps = int.Parse(line["LapsLed"]),
                    FastLapNr = int.TryParse(line["FastLap#"], out int fastLapNr) ? fastLapNr : 0,
                    Incidents = int.Parse(line["Inc"]),
                    Status = (RaceStatusEnum)Enum.Parse(typeof(RaceStatusEnum), line["Out"]),
                    QualifyingTime = new LapTime(TimeSpan.Zero)
                };
                //if (!LeagueClient.LeagueMembers.ToList().Exists(x => x.IRacingId == row.IRacingId))
                if (MemberList.Any(x => x.IRacingId == line["CustID"]))
                {
                    row.Member = MemberList.SingleOrDefault(x => x.IRacingId == line["CustID"]);
                    row.Team = row.Member.Team;
                }
                //row.Interval = new LapInterval(
                //    TimeSpan.TryParse("0:" + line["Interval"].Replace("-",""), culture, out TimeSpan intvTime) ? intvTime : TimeSpan.Zero,
                //    int.TryParse(line["Interval"].Replace("L", ""), out int intvLaps) ? intvLaps : 0);
                var teststrt = line["Interval"];
                var test = int.TryParse(line["Interval"].Replace("L", ""), out int intvtest) ? intvtest : 0;
                row.Interval = new LapInterval(GetTimeSpanFromString(line["Interval"]), int.TryParse(line["Interval"].Replace("L", ""), out int intvLaps) ? intvLaps : 0);
                row.AvgLapTime = new LapTime(TimeSpan.TryParse(PrepareTimeString(line["AverageLapTime"]), culture, out TimeSpan avgLap) ? avgLap : TimeSpan.Zero);
                row.FastestLapTime = new LapTime(TimeSpan.TryParse(PrepareTimeString(line["FastestLapTime"]), culture, out TimeSpan fastLap) ? fastLap : TimeSpan.Zero);
                //row.PositionChange = row.StartPosition - row.FinishPosition;
                resultRows.Add(row);
            }
            return resultRows;
        }

        private static string PrepareTimeString(string timeString)
        {
            var hms = timeString.Split(':');
            if (hms.Count() == 1)
            {
                return "0:00:" + timeString;
            }
            else if (hms.Count() == 2)
            {
                return "0:" + timeString;
            }
            else
            {
                return timeString;
            }
        }

        public static TimeSpan GetTimeSpanFromString(string str)
        {
            bool negative = str.Contains("-");
            var components = str.Replace("-", "").Split(':');
            try
            {
                double seconds = double.Parse(components.Last(), System.Globalization.NumberStyles.AllowDecimalPoint, System.Globalization.CultureInfo.GetCultureInfo("en-EN"));
                int minutes = ((components.Count() > 1) ? int.Parse(components.Reverse().ElementAt(1)) : 0);
                int hours = ((components.Count() > 2) ? int.Parse(components.Reverse().ElementAt(2)) : 0);

                var timeSpan = new TimeSpan(hours, minutes, 0);
                timeSpan = timeSpan.Add(TimeSpan.FromSeconds(seconds));
                if (negative)
                    timeSpan = timeSpan.Negate();
                return timeSpan;
            }
            catch
            {
                return TimeSpan.Zero;
            }
        }

        public SimSessionDetails GetSessionDetails()
        {
            return null;
        }
    }


}
