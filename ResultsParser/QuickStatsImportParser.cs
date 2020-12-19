using iRLeagueManager.Models.Statistics;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenericParsing;
using iRLeagueManager.Models.Members;
using iRLeagueDatabase.DataTransfer.Statistics;
using System.Collections.ObjectModel;

namespace iRLeagueManager.ResultsParser
{
    public class QuickStatsImportParser
    {
        public DataTable Data { get; set; }

        public IEnumerable<LeagueMember> MemberList { get; set; } = new List<LeagueMember>();
        public IEnumerable<TeamModel> TeamList { get; set; }

        public QuickStatsImportParser()
        {

        }

        public IEnumerable<LeagueMember> GetNewMemberList()
        {
            var memberList = new List<LeagueMember>();

            foreach (DataRow row in Data.Rows)
            {
                if (string.IsNullOrEmpty((string)row["Name"]) == false && !MemberList.Any(x => x.FullName == (string)row["Name"]))
                {
                    if ((string)row["Name"] == "NewDriver")
                    {
                        continue;
                    }
                    var newMember = new LeagueMember(0, ((string)row["Name"]).Split(' ').First(), ((string)row["Name"]).Split(' ').Skip(1).Aggregate((x, y) => x + " " + y));
                    newMember.DanLisaId = (string)row["Drvid"];
                    memberList.Add(newMember);
                }
                else if (string.IsNullOrEmpty((string)row["Name"]) == false)
                {
                    var member = MemberList.SingleOrDefault(x => x.FullName == (string)row["Name"]);
                    member.DanLisaId = ((string)row["Drvid"]);
                }
            }
            return memberList;
        }

        public void LoadDataFromFile(string path)
        {
            using (var parser = new GenericParserAdapter(path)
            {
                FirstRowHasHeader = true,
                ColumnDelimiter = '\t',
                FirstRowSetsExpectedColumnCount = true,
                SkipEmptyRows = true,
            })
            {
                Data = parser.GetDataTable();
            }
        }

        public DriverStatisticModel GetDriverStatistic()
        {
            List<DriverStatisticRowModel> rows = new List<DriverStatisticRowModel>();

            foreach(DataRow row in Data.Rows)
            {
                if (MemberList.Any(x => x.DanLisaId == (string)row["Drvid"]))
                {
                    var setRow = new DriverStatisticRowModel();
                    setRow.MemberId = MemberList.Single(x => x.DanLisaId == (string)row["Drvid"]).MemberId.GetValueOrDefault();
                    setRow.StartIRating = int.TryParse((string)row["IRating"], out int ir) ? ir : 0;
                    setRow.EndIRating = int.TryParse((string)row["IRating"], out ir) ? ir : 0;
                    setRow.StartSRating = double.TryParse((string)row["SR"], out double sr) ? sr : 0;
                    setRow.EndSRating = double.TryParse((string)row["SR"], out sr) ? sr : 0;
                    var parts = ((string)row["ErstesRennen"]).Split(' ').First().Split('/');
                    setRow.FirstRaceDate = parts.Count() >= 3 ? new DateTime(int.Parse(parts[2]), int.Parse(parts[0]), int.Parse(parts[1])) : (DateTime?)null;
                    parts = ((string)row["LetztesRennen"]).Split(' ').First().Split('/');
                    setRow.LastRaceDate = parts.Count() >= 3 ? new DateTime(int.Parse(parts[2]), int.Parse(parts[0]), int.Parse(parts[1])) : (DateTime?)null;
                    setRow.LastRaceFinalPosition = int.TryParse((string)row["LetztePlatzierung"], out int lastPl) ? lastPl : 0;
                    setRow.AvgFinalPosition = double.TryParse((string)row["DurchschnittPlatzierung"], out double avgPl) ? avgPl : 0;
                    setRow.Poles = int.TryParse((string)row["Poles"], out int poles) ? poles : 0;
                    setRow.Races = int.TryParse((string)row["Rennen"], out int races) ? races : 0;
                    setRow.RacesCompleted = int.TryParse((string)row["Finish"], out int finish) ? finish : 0;
                    setRow.Titles = int.TryParse((string)row["Titel"], out int titles) ? titles : 0;
                    setRow.DrivenKm = double.TryParse((string)row["Distanz"], out double dist) ? dist : 0;
                    setRow.LeadingKm = double.TryParse((string)row["FuehrungsKm"], out dist) ? dist : 0;
                    setRow.Incidents = int.TryParse((string)row["Incidents"], out int incs) ? incs : 0;
                    setRow.RacePoints = int.TryParse((string)row["RawPunkte"], out int points) ? points : 0;
                    setRow.TotalPoints = int.TryParse((string)row["Punkte"], out points) ? points : 0;
                    setRow.PenaltyPoints = int.TryParse((string)row["Strafpunkte"], out points) ? points : 0;
                    setRow.AvgPointsPerRace = double.TryParse((string)row["PunkteProRennen"], out double avg) ? avg : 0;
                    setRow.BestStartPosition = int.TryParse((string)row["BestQuali"], out int best) ? best : 0;
                    setRow.BestFinalPosition = int.TryParse((string)row["BestErgebnis"], out best) ? best : 0;
                    setRow.Wins = int.TryParse((string)row["Siege"], out best) ? best : 0;
                    setRow.Top3 = int.TryParse((string)row["Podium"], out best) ? best : 0;
                    setRow.Top5 = int.TryParse((string)row["Top5"], out best) ? best : 0;
                    setRow.Top10 = int.TryParse((string)row["Top10"], out best) ? best : 0;
                    setRow.Top15 = int.TryParse((string)row["Top15"], out best) ? best : 0;
                    setRow.Top20 = int.TryParse((string)row["Top20"], out best) ? best : 0;
                    setRow.FastestLaps = int.TryParse((string)row["SchnellsteRunde"], out best) ? best : 0;
                    setRow.HardChargerAwards = int.TryParse((string)row["HardCharger"], out best) ? best : 0;
                    setRow.CleanestDriverAwards = int.TryParse((string)row["CleanestDriver"], out best) ? best : 0;
                    rows.Add(setRow);
                }
            }

            var driverStats = new DriverStatisticModel();
            driverStats.DriverStatisticRows = rows.ToArray();

            return driverStats;
        }
    }
}
