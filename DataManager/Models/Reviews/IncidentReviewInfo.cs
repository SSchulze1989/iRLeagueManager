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
