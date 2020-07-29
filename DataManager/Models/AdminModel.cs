﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using iRLeagueManager.Enums;

namespace iRLeagueManager.Models
{
    public class AdminModel : MappableModel
    {
        public long AdminId { get; }

        public override long[] ModelId => new long[] { AdminId };

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