using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using iRLeagueManager.Interfaces;
using iRLeagueManager.Enums;

namespace iRLeagueManager.Models.Database
{
    public class DatabaseUserModel : ModelBase, IAdminData, IAdmin
    {
        private int userId;
        public int UserId { get => userId; internal set => SetValue(ref userId, value); }

        private long memberId;
        public long MemberId { get => memberId; set => SetValue(ref memberId, value); }

        public override long[] ModelId => new long[] { UserId };

        private AdminRights rights;
        public AdminRights Rights { get => rights; set => SetValue(ref rights, value); }

        private bool isOwner;
        public bool IsOwner { get => isOwner; set => SetValue(ref isOwner, value); }

        public string email;
        public string Email { get => email; set => SetValue(ref email, value); }

        //internal string AuthorizationToken { get; set; }

        private string firstname;
        public string Firstname
        {
            get => firstname;
            set
            {
                if (SetValue(ref firstname, value))
                {
                    OnPropertyChanged(nameof(Fullname));
                }
            }
        }

        private string lastname;
        public string Lastname
        {
            get => lastname;
            set
            {
                if (SetValue(ref lastname, value)){
                    OnPropertyChanged(nameof(Fullname));
                }
            }
        }

        public string Fullname => Firstname + " " + Lastname;

        //private string PwSalt { get; set; }
        //private string PwHash { get; set; }

        public DatabaseUserModel()
        {
            Firstname = "Firstname";
            Lastname = "Lastname";
        }



        //internal DatabaseUserModel(string pwHash, string pwSalt) : base()
        //{
        //    PwSalt = pwSalt;
        //    PwHash = pwHash;
        //}

        //public string GetAuthorizationToken(string auth)
        //{
            
        //}
    }
}
