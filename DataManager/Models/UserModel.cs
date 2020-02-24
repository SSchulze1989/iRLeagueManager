using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using iRLeagueManager.Interfaces;


namespace iRLeagueManager.Models
{
    public class UserModel : ModelBase, IAdmin
    {
        public int UserId { get; }

        public override long? ModelId => UserId;

        private string userName;
        public string UserName { get => userName; set => SetValue(ref userName, value); }

        private int? memberId;
        public int? MemberId { get => memberId; set => SetValue(ref memberId, value); }
        long IAdmin.MemberId => (MemberId == null) ? 0 : (int)MemberId;

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

        public string FullName { get => Firstname + " " + Lastname;  }

        //private AdminModel admin;
        //public AdminModel Admin { get => admin; set => SetValue(ref admin, value); }

        public UserModel(int userId)
        {
            UserId = userId;
        }
    }
}
