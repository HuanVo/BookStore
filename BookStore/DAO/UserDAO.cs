using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using DAL;

namespace DAO
{
    public class UserDAO
    {
        BookStoreDbContext db = null;
        private static UserDAO instance;
        public static UserDAO Instance
        {
            get
            {
                if (instance == null) instance = new UserDAO();
                return UserDAO.instance;
            }
            private set { UserDAO.instance = value; }
        }

        private UserDAO()
        {
            db = new BookStoreDbContext();
        }

        /// <summary>
        /// function login for user
        /// </summary>
        /// <param name="userName">email login</param>
        /// <param name="password">password login</param>
        /// <returns>return a bool type</returns>
        public bool Login(String userName, String password)
        {
            var result = (from a in db.USERS
                          join b in db.CUSTOMERs on a.code_cst equals b.code_cst
                          where a.login_id.Contains(userName) && a.login_pass.Contains(password)
                          select a.code_cst).Count();
            if (result>0)
                return true;
            return false;
                    
        }
    }
}
