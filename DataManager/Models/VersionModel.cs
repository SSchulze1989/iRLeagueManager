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
