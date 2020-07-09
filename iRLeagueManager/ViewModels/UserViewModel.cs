using iRLeagueManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using iRLeagueManager.Enums;

namespace iRLeagueManager.ViewModels
{
    public class UserViewModel : LeagueContainerModel<UserModel>
    {
        public long UserId => (Model?.UserId).GetValueOrDefault();
        public string UserName => Model?.UserName;

        public long? MemberId { get => Model?.MemberId; set => Model.MemberId = value; }

        public AdminRights AdminRights => (Model?.AdminRights).GetValueOrDefault();

        public string Firstname { get => Model?.Firstname; set => Model.Firstname = value; }

        public string Lastname { get => Model?.Lastname; set => Model.Lastname = value; }

        public string Email { get => Model?.Email; set => Model.Email = value; }

        public string ProfileText { get => Model?.ProfileText; set => Model.ProfileText = value; }

        public string FullName => Firstname + " " + Lastname;

        protected override UserModel Template => UserModel.GetAnonymous();
    }
}
