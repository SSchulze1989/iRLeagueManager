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
using System.Xml;
using System.Xml.Serialization;
using iRLeagueDatabase.DataTransfer.Members;
using iRLeagueDatabase.DataTransfer.Sessions;
using iRLeagueManager.Enums;
using System.Runtime.Serialization;

namespace iRLeagueDatabase.DataTransfer.Reviews
{
    [Serializable]
    [DataContract]
    public class IncidentReviewDataDTO : IncidentReviewInfoDTO
    {
        public override Type Type => typeof(IncidentReviewDataDTO);

        //[DataMember]
        //public int ReviewId { get; set; }
        //[DataMember]
        //public int SeasonId;
        //[DataMember]
        //public LeagueMemberDTO Author { get; set; }
        [DataMember]
        public SessionInfoDTO Session { get; set; }
        [DataMember]
        public string IncidentNr { get; set; }
        [DataMember]
        public string IncidentKind { get; set; }
        [DataMember]
        public string FullDescription { get; set; }
        //[DataMember]
        //public LeagueMemberInfoDTO Author { get; set; }
        [DataMember]
        public string OnLap { get; set; }
        [DataMember]
        public string Corner { get; set; }
        [DataMember]
        public TimeSpan TimeStamp { get; set; }
        [DataMember]
        public ICollection<LeagueMemberInfoDTO> InvolvedMembers { get; set; }
        [DataMember]
        public ICollection<ReviewCommentDataDTO> Comments { get; set; }
        [DataMember]
        public ReviewVoteDataDTO[] AcceptedReviewVotes { get; set; }

        [DataMember]
        public string ResultLongText { get; set; }
        //[DataMember]
        //public LeagueMemberInfoDTO MemberAtFault { get; set; }
        //[DataMember]
        //public VoteEnum VoteResult { get; set; }
        //[DataMember]
        //public VoteState VoteState { get; set; }

        //[DataMember]
        //public LeagueMemberInfoDTO CreatedBy { get; set; }
        //[DataMember]
        //public LeagueMemberInfoDTO LastModifiedBy { get; set; }

        public IncidentReviewDataDTO() { }
    }
}
