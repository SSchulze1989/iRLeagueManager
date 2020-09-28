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

using iRLeagueManager.Interfaces;
using iRLeagueManager.Enums;

namespace iRLeagueManager.Models.Database
{
    public class DatabaseUserModel : MappableModel, IAdminData, IAdmin
    {
        private int userId;
        public int UserId { get => userId; internal set => SetValue(ref userId, value); }

        private long? memberId;
        public long? MemberId { get => memberId; set => SetValue(ref memberId, value); }

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
