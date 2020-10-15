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

using iRLeagueManager.Enums;
using iRLeagueManager.Models.Members;

namespace iRLeagueManager.Models.Reviews
{
    public class ReviewVoteModel : MappableModel
    {
        public long ReviewVoteId { get; internal set; }
        public override long[] ModelId => new long[] { ReviewVoteId };

        private VoteEnum vote;
        public VoteEnum Vote { get => vote; set => SetValue(ref vote, value); }

        private LeagueMember memberAtFault;
        public LeagueMember MemberAtFault { get => memberAtFault; set => SetValue(ref memberAtFault, value); }

        private VoteCategoryModel voteCategory;
        public VoteCategoryModel VoteCategory { get => voteCategory; set => SetValue(ref voteCategory, value); }

        public ReviewVoteModel() : base()
        {
        }

        public ReviewVoteModel(long reviewVoteId)
        {
            ReviewVoteId = reviewVoteId;
        }

        public override string ToString()
        {
            return ((VoteCategory != null) ? VoteCategory.Text : Vote.ToString()).ToUpper() + ": " + MemberAtFault?.FullName?.ToString();
        }
    }
}
