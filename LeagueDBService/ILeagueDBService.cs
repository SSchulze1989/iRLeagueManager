using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using iRLeagueDatabase.DataTransfer;
using iRLeagueDatabase.DataTransfer.Sessions;
using iRLeagueDatabase.DataTransfer.Members;
using iRLeagueDatabase.DataTransfer.Reviews;
using iRLeagueDatabase.DataTransfer.Results;

namespace LeagueDBService
{
    // HINWEIS: Mit dem Befehl "Umbenennen" im Menü "Umgestalten" können Sie den Schnittstellennamen "IService1" sowohl im Code als auch in der Konfigurationsdatei ändern.
    [ServiceContract]
    [ServiceKnownType(typeof(RaceSessionDataDTO))]
    [ServiceKnownType(typeof(LeagueMemberDataDTO))]
    [ServiceKnownType(typeof(ResultDataDTO))]
    public interface ILeagueDBService
    {
        [OperationContract]
        string TestDB();

        [OperationContract]
        string Test(string name);

        [OperationContract]
        SeasonDataDTO GetSeason(int seasonId);

        [OperationContract]
        List<SeasonDataDTO> GetSeasons(int[] seasonIds = null);

        [OperationContract]
        SeasonDataDTO PutSeason(SeasonDataDTO season);

        [OperationContract]
        LeagueMemberDataDTO GetMember(int memberId);

        [OperationContract]
        List<LeagueMemberDataDTO> GetMembers(int[] memberId = null);

        [OperationContract]
        LeagueMemberDataDTO[] UpdateMemberList(LeagueMemberDataDTO[] members);

        [OperationContract]
        LeagueMemberDataDTO GetLastMember();

        [OperationContract]
        LeagueMemberDataDTO PutMember(LeagueMemberDataDTO member);

        [OperationContract]
        IncidentReviewDataDTO GetReview(int reviewId);

        [OperationContract]
        IncidentReviewDataDTO PutReview(IncidentReviewDataDTO review);

        [OperationContract]
        SessionDataDTO GetSession(int sessionId);

        [OperationContract]
        SessionDataDTO PutSession(SessionDataDTO session);

        [OperationContract]
        CommentDataDTO GetComment(int commentId);

        [OperationContract]
        CommentDataDTO PutComment(ReviewCommentDataDTO comment);

        [OperationContract]
        ScheduleDataDTO GetSchedule(int scheduleId);

        [OperationContract]
        List<ScheduleDataDTO> GetSchedules(int[] scheduleIds = null);

        [OperationContract]
        ScheduleDataDTO PutSchedule(ScheduleDataDTO schedule);

        [OperationContract]
        ResultDataDTO GetResult(int resultId);

        [OperationContract]
        ResultDataDTO PutResult(ResultDataDTO result);

        [OperationContract]
        StandingsRowDTO[] GetSeasonStandings(int seasonId, int? lastSessionId);

        [OperationContract]
        StandingsRowDTO[] GetTeamStandings(int seasonId, int? lastSessionId);

        [OperationContract]
        void CleanUpSessions();

        // TODO: Hier Dienstvorgänge hinzufügen
    }

    // Verwenden Sie einen Datenvertrag, wie im folgenden Beispiel dargestellt, um Dienstvorgängen zusammengesetzte Typen hinzuzufügen.
    // Sie können im Projekt XSD-Dateien hinzufügen. Sie können nach dem Erstellen des Projekts dort definierte Datentypen über den Namespace "LeagueDBService.ContractType" direkt verwenden.
    [DataContract]
    public class CompositeType
    {
        bool boolValue = true;
        string stringValue = "Hello ";

        [DataMember]
        public bool BoolValue
        {
            get { return boolValue; }
            set { boolValue = value; }
        }

        [DataMember]
        public string StringValue
        {
            get { return stringValue; }
            set { stringValue = value; }
        }
    }
}
