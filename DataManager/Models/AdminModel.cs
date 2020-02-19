using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using iRLeagueManager.Enums;

namespace iRLeagueManager.Models
{
    public class AdminModel : ModelBase
    {
        public long AdminId { get; }

        private string leagueName;
        public string LeagueName { get => leagueName; set => SetValue(ref leagueName, value); }

        private bool isOwner;
        public bool IsOwner { get => isOwner; set => SetValue(ref isOwner, value); }

        private AdminRights rights;
        public AdminRights Rights { get => rights; set => SetValue(ref rights, value); }

        public AdminModel(long adminId)
        {
            AdminId = adminId;
        }
    }
}
