using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iRLeagueManager.Enums;
using iRLeagueManager.Interfaces;


namespace iRLeagueManager.Models
{
    public class UserModel : ModelBase, IAdmin
    {
        public long? UserId { get; internal set; }

        public override long[] ModelId => new long[] { UserId.GetValueOrDefault() };

        private string userName;
        public string UserName { get => userName; set => SetValue(ref userName, value); }

        private long? memberId;
        public long? MemberId { get => memberId; set => SetValue(ref memberId, value); }
        long? IAdmin.MemberId => (MemberId == null) ? 0 : (int)MemberId;

        private AdminRights adminRights;
        public AdminRights AdminRights { get => adminRights; internal set => SetValue(ref adminRights, value); }

        private string firstname;
        public string Firstname
        {
            get => firstname;
            set
            {
                if (SetValue(ref firstname, value))
                    OnPropertyChanged(nameof(FullName));
            }
        }

        private string lastname;
        public string Lastname
        {
            get => lastname;
            set
            {
                if (SetValue(ref lastname, value))
                    OnPropertyChanged(nameof(FullName));
            }
        }

        private string email;
        public string Email { get => email; set => SetValue(ref email, value); }

        private string profileText;
        public string ProfileText { get => profileText; set => SetValue(ref profileText, value); }

        //public string FullName { get => Firstname + " " + Lastname;  }
        public string FullName => UserName;

        public UserModel(long userId)
        {
            UserId = userId;
        }

        public static UserModel GetAnonymous()
        {
            return new UserModel(0) { UserName = "AnonymousUser", AdminRights = AdminRights.Member, Firstname = "Anonymous", Lastname = "User" };
        }
    }
}
