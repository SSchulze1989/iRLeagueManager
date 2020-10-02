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
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iRLeagueManager.ViewModels
{
    public class ReviewNavBarViewModel : ViewModelBase
    {
        private ReviewsPageViewModel reviewsPageViewModel;
        public ReviewsPageViewModel ReviewsPageViewModel
        {
            get => reviewsPageViewModel;
            set
            {
                if (SetValue(ref reviewsPageViewModel, value))
                {
                    OnPropertyChanged(nameof(CurrentReviews));
                }
            }
        }

        private bool isExpanded;
        public bool IsExpanded { get => isExpanded; set => SetValue(ref isExpanded, value); }

        private int totalReviews;
        public int TotalReviews { get => totalReviews; set => SetValue(ref totalReviews, value); }

        private int openReviews;
        public int OpenReviews { get => openReviews; set => SetValue(ref openReviews, value); }

        private int notVoted;
        public int NotVoted { get => notVoted; set => SetValue(ref notVoted, value); }

        private int voted;
        public int Voted { get => voted; set => SetValue(ref voted, value); }
        
        private int openAndAgreed;
        public int OpenAndAgreed { get => openAndAgreed; set => SetValue(ref openAndAgreed, value); }

        private int openAndDisagreed;
        public int OpenAndDisagreed { get => openAndDisagreed; set => SetValue(ref openAndDisagreed, value); }

        private int closedReviews;
        public int ClosedReviews { get => closedReviews; set => SetValue(ref closedReviews, value); }

        public ICollectionView CurrentReviews => ReviewsPageViewModel?.CurrentReviews;

        public ReviewNavBarViewModel()
        {
        }

        public void CalculateReviewsStatistics()
        {
            var reviews = CurrentReviews.OfType<IncidentReviewViewModel>();
            TotalReviews = reviews.Count();
            var openReviews = reviews.Where(x => x.CountAcceptedVotes.Count() == 0);
            OpenReviews = openReviews.Count();
            ClosedReviews = TotalReviews - OpenReviews;
            var voted = reviews.Where(x => x.Comments.Any(y => y.Votes.Count > 0));
            Voted = voted.Count();
            NotVoted = TotalReviews - Voted;
        }
    }
}
