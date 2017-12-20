using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookStore.Commons
{
    public class UserSession
    {
        string loginId;
        public string LoginId { get => loginId; set => loginId = value; }
        public string CookieUser{ get => cookieUser; set => cookieUser = value; }
        string cookieUser;
        
        public void GetUserSession(string loginId, string cookieUser)
        {
            this.LoginId = loginId;
            this.CookieUser = cookieUser;
        }
    }
}