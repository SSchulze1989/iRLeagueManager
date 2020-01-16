using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using iRLeagueManager.Data;
using iRLeagueManager.Interfaces;
using iRLeagueManager.Enums;
using iRLeagueManager.Timing;
using iRLeagueManager.Models.Results;
using iRLeagueManager.Models.Members;

namespace iRLeagueManager.Services
{
    public class ResultParserService
    {
        //public ILeague LeagueClient { get; set; }

        public LeagueContext LeagueContext { get; set; }

        public char Delimiter { get; set; } = ',';

        public ResultParserService() { }

        public ResultParserService(LeagueContext context)
        {
            //LeagueClient = leagueClient;
            LeagueContext = context;
        }

        public IEnumerable<Dictionary<string, string>> ParseCSV(StreamReader reader, int headerLine = 9, int firstDataLine = 10)
        {
            int currentLine = 0;

            if (firstDataLine <= headerLine)
            {
                throw new ArgumentException("First data line is before header line. Header must always be above data!");
            }

            IEnumerable<string> Header = new string[0];
            List<Dictionary<string, string>> DataLines = new List<Dictionary<string, string>>();

            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine();
                currentLine++;
                if (currentLine == headerLine)
                {
                    Header = line.Split(Delimiter).Select(x => x.Replace(" ", "").Replace("\"", ""));
                }
                if (currentLine >= firstDataLine)
                {
                    IEnumerable<string> data = line.Replace("\"", "").Split(Delimiter);
                    DataLines.Add(data.Select((x, i) => new { k = Header.ElementAt(i), v = x }).ToDictionary(x => x.k, x => x.v));
                }
            }

            return DataLines;
        }

        public IEnumerable<LeagueMember> GetNewMemberList(IEnumerable<Dictionary<string, string>> dataLines)
        {
            var memberList = new List<LeagueMember>();

            foreach (var line in dataLines)
            {
                IRacingResultRow row = new IRacingResultRow();
                if (!LeagueContext.MemberList.Any(x => x.IRacingId == line["CustID"]))
                {
                    //var newMember = LeagueClient.AddNewMember(line["Name"].Split(' ').First(), line["Name"].Split(' ').Last());
                    var newMember = new LeagueMember(0, line["Name"].Split(' ').First(), line["Name"].Split(' ').Last());
                    //LeagueContext.MemberList.Add(newMember);
                    newMember.IRacingId = line["CustID"];
                    //row.MemberId = newMember.MemberId;
                    row.Member = newMember;
                    memberList.Add(newMember);
                }
            }
            return memberList;
        }

        public IEnumerable<ResultRowModel> GetResultRows(IEnumerable<Dictionary<string, string>> dataLines)
        {
            List<IRacingResultRow> resultRows = new List<IRacingResultRow>();

            var culture = System.Globalization.CultureInfo.GetCultureInfo("de-EN");

            foreach (var line in dataLines)
            {
                IRacingResultRow row = new IRacingResultRow
                {
                    FinalPosition = int.Parse(line["FinPos"]),
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
                    RacePoints = 0,
                    BonusPoints = 0,
                    QualifyingTime = new LapTime(TimeSpan.Zero)
                };
                //if (!LeagueClient.LeagueMembers.ToList().Exists(x => x.IRacingId == row.IRacingId))
                if (LeagueContext.MemberList.Any(x => x.IRacingId == line["CustID"]))
                {
                    row.Member = LeagueContext.MemberList.SingleOrDefault(x => x.IRacingId == line["CustID"]);
                }
                //row.Interval = new LapInterval(
                //    TimeSpan.TryParse("0:" + line["Interval"].Replace("-",""), culture, out TimeSpan intvTime) ? intvTime : TimeSpan.Zero,
                //    int.TryParse(line["Interval"].Replace("L", ""), out int intvLaps) ? intvLaps : 0);
                var teststrt = line["Interval"];
                var test = int.TryParse(line["Interval"].Replace("L", ""), out int intvtest) ? intvtest : 0;
                row.Interval = new LapInterval(GetTimeSpanFromString(line["Interval"]), int.TryParse(line["Interval"].Replace("L", ""), out int intvLaps) ? intvLaps : 0);
                row.AvgLapTime = new LapTime(TimeSpan.TryParse("0:" + line["AverageLapTime"], culture, out TimeSpan avgLap) ? avgLap : TimeSpan.Zero);
                row.FastestLapTime = new LapTime(TimeSpan.TryParse("0:" + line["FastestLapTime"], culture, out TimeSpan fastLap) ? fastLap : TimeSpan.Zero);
                row.PositionChange = row.StartPosition - row.FinishPosition;
                resultRows.Add(row);
            }
            return resultRows;
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
                int days = ((components.Count() > 3) ? int.Parse(components.Reverse().ElementAt(3)) : 0);

                var timeSpan = new TimeSpan(days, hours, minutes);
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
    }


}
