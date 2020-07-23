using iRLeagueManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using iRLeagueManager.Enums;
using iRLeagueManager.Models.User;

namespace iRLeagueManager.ViewModels
{
    public class UserViewModel : ContainerModelBase<UserModel>
    {
        internal UserModel Model => Source;
        public string UserId => (Model?.UserId);
        public string UserName => Model?.UserName;

        public long? MemberId { get => Model?.MemberId; set => Model.MemberId = value; }

        public AdminRights AdminRights => (Model?.AdminRights).GetValueOrDefault();

        public string Firstname { get => Model?.Firstname; set => Model.Firstname = value; }

        public string Lastname { get => Model?.Lastname; set => Model.Lastname = value; }

        public string Email { get => Model?.Email; set => Model.Email = value; }

        public string ProfileText { get => Model?.ProfileText; set => Model.ProfileText = value; }

        //public string FullName => Firstname + " " + Lastname;
        public string FullName
        {
            get
            {
                if (Model == null)
                    return "(null)";

                if (Firstname != null || Lastname != null)
                    return Firstname + " " + Lastname;

                if (UserName != null)
                    return UserName;

                return UserId;
            }
        }

        public UserViewModel()
        {

        }
    }
}
