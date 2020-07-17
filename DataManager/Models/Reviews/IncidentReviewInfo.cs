using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using iRLeagueManager.Models.Members;
using iRLeagueManager.Models;
using iRLeagueManager.Interfaces;
using iRLeagueManager.Attributes;

namespace iRLeagueManager.Models.Reviews
{
    public class IncidentReviewInfo : ModelBase, IReviewInfo, IHierarchicalModel
    {
        private long? reviewId;
        [EqualityCheckProperty]
        public long? ReviewId { get => reviewId; internal set => reviewId = value; }

        public override long[] ModelId => new long[] { ReviewId.GetValueOrDefault() };

        public string AuthorName { get; internal set; }

        private LeagueMember author;
        public LeagueMember Author { get => author; internal set => SetValue(ref author, value); }
        IAdmin IReviewInfo.Author => Author;

        private int onLap;
        public int OnLap { get => onLap; set => SetValue(ref onLap, value); }

        private int corner;
        public int Corner { get => corner; set => SetValue(ref corner, value); }

        string IHierarchicalModel.Description => "L" + OnLap.ToString() + " - C" + Corner.ToString();

        IEnumerable<object> IHierarchicalModel.Children => new object[0];
    }
}
