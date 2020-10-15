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
using System.Collections.Specialized;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace iRLeagueManager.Models
{
    public class VersionModel : ModelBase
    {
        private DateTime? createdOn;
        public DateTime? CreatedOn { get => createdOn; internal set { createdOn = value; OnPropertyChanged(); } }

        public string CreatedByUserId { get; internal set; }

        private DateTime? lastModifiedOn;
        public DateTime? LastModifiedOn { get => lastModifiedOn; internal set { lastModifiedOn = value; OnPropertyChanged(); } }

        public string LastModifiedByUserId { get; set; }

        private bool containsChanges;
        public virtual bool ContainsChanges { get => containsChanges; protected set { containsChanges = value; OnPropertyChanged(); } }

        private int version;
        public int Version { get => version; internal set { version = value; OnPropertyChanged(); } }

        public VersionModel()
        {
            CreatedOn = DateTime.Now;
            lastModifiedOn = DateTime.Now;
        }

        protected override bool SetValue<T>(ref T targetProperty, T value, [CallerMemberName] string propertyName = "")
        {
            if (base.SetValue(ref targetProperty, value, propertyName))
            {
                if (isInitialized)
                {
                    LastModifiedOn = DateTime.Now;
                    ContainsChanges = true;
                }
                return true;
            }
            return false;
        }

        protected override bool SetNotifyCollection<T>(ref T targetCollection, T value, [CallerMemberName] string propertyName = "")
        {
            if (base.SetNotifyCollection(ref targetCollection, value, propertyName))
            {
                if (isInitialized)
                {
                    LastModifiedOn = DateTime.Now;
                    ContainsChanges = true;
                }
                return true;
            }
            return false;
        }

        protected override void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (isInitialized)
            {
                LastModifiedOn = DateTime.Now;
                ContainsChanges = true;
            }
            base.OnCollectionChanged(sender, e);
        }

        public void ResetChangedState()
        {
            ContainsChanges = false;
        }
    }
}
