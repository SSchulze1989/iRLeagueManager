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

using iRLeagueManager.Models.Members;
using iRLeagueManager.Models;
using iRLeagueManager.Interfaces;
using iRLeagueManager.Attributes;
using iRLeagueManager.Models.User;

namespace iRLeagueManager.Models.Reviews
{
    public class IncidentReviewInfo : MappableModel, IHierarchicalModel
    {
        private long? reviewId;
        [EqualityCheckProperty]
        public long? ReviewId { get => reviewId; internal set => reviewId = value; }

        public override long[] ModelId => new long[] { ReviewId.GetValueOrDefault() };

        public string AuthorName { get; internal set; }

        public UserModel Author { get; internal set; }

        private string onLap;
        public string OnLap { get => onLap; set => SetValue(ref onLap, value); }

        private string corner;
        public string Corner { get => corner; set => SetValue(ref corner, value); }

        string IHierarchicalModel.Description => "L" + OnLap.ToString() + " - C" + Corner.ToString();

        IEnumerable<object> IHierarchicalModel.Children => new object[0];

        public IncidentReviewInfo() { }

        public IncidentReviewInfo(UserModel author)
        {
            Author = author;
            AuthorName = author.UserName;
        }
    }
}
