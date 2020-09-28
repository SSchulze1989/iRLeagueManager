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
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.Specialized;
using System.Collections;

namespace iRLeagueManager.Models
{
    public abstract class MappableModel : VersionModel, INotifyPropertyChanged, ICacheableModel
    {
        public abstract long[] ModelId { get; }

        object[] ICacheableModel.ModelId => ModelId?.Select(x => (object)x).ToArray();

        public MappableModel()
        {
            isInitialized = false;
        }

        public DateTime LastUpdate { get; internal set; }

        public TimeSpan LifeTime { get; set; } = TimeSpan.FromMinutes(5);

        private bool forceIsExpired;
        public bool IsExpired
        {
            get
            {
                return (LastUpdate + LifeTime) < DateTime.Now || forceIsExpired;
            }
            set => forceIsExpired = value;
        }

        public virtual bool CompareIdentity(ICacheableModel comp)
        {
            return this.ModelId.SequenceEqual(comp.ModelId.Cast<long>());
        }

        public virtual bool IsBaseType()
        {
            return this.GetType().Equals(typeof(MappableModel));
        }

        public virtual Type GetBaseType()
        {
            return typeof(MappableModel);
        }
    }
}
