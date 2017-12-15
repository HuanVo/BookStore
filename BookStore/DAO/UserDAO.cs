using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;

namespace DAO
{
    public class UserDAO
    {
        BookStoreDbContext db = null;
        private static UserDAO instance;
        public static UserDAO Instance {
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
        

    }
}
