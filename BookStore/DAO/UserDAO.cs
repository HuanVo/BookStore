using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using DAL;
using BookStore.Commons;

namespace DAO
{
    public class UserDAO
    {
        BookStoreDbContext db = null;
        /// <summary>
        /// create instance for UserDAO
        /// </summary>
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

        /// <summary>
        /// Contrustor
        /// </summary>
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
            try
            {
                var result = (from a in db.USERS
                              join b in db.CUSTOMERs on a.code_cst equals b.code_cst
                              where a.login_id == userName && a.login_pass == password && b.dest_flg == 0 && a.dest_flg == 0 && a.use_flg ==0 
                              select a.code_cst).Count();
                if (result > 0)
                    return true;
            }
            catch(Exception ex)
            {
                ErrorLog.WriteLog(ex.Message);
            }
            return false;
        }
        
        /// <summary>
        /// save cookie to database
        /// </summary>
        /// <param name="loginId">Login id</param>
        /// <param name="cookieString">cookie</param>
        public void SaveCookieToDB(String loginId, String cookieString)
        {
            try
            {
                var check = db.USERS.Find(loginId);
                if(check !=null)
                {
                    check.loginkey = cookieString;
                    db.SaveChanges();
                }

            }catch(Exception ex)
            {
                ErrorLog.WriteLog(ex.Message);
            }
        }

        public bool CheckNumLoginFail(String loginId, int cntLoginConfig, int expiresTimeConfig)
        {
            try
            {
                expiresTimeConfig = 0;
                var result = (from a in db.USERS
                              join b in db.CUSTOMERs on a.code_cst equals b.code_cst
                              where b.dest_flg == 0 && a.dest_flg == 0 &&  a.login_id == loginId && a.cnt_login_error>cntLoginConfig 
                              select a.code_cst).Count();
                if (result > 0)
                    return true;
            }
            catch (Exception ex)
            {
                ErrorLog.WriteLog(ex.Message);
            }
            return false;
        }

        public void UpdateCountLoginAndDateLoginError(String loginId)
        {
            try
            {
                var check = db.USERS.Find(loginId);
                if (check != null)
                {
                    check.date_login_error = DateTime.Now;
                    check.cnt_login_error = check.cnt_login_error + 1;
                    db.SaveChanges();
                }

            }
            catch (Exception ex)
            {
                ErrorLog.WriteLog(ex.Message);
            }
        }

        public void ResetCountLoginAndDateLoginError(String loginId)
        {
            try
            {
                var check = db.USERS.Find(loginId);
                if (check != null)
                {
                    check.date_login_error = null;
                    check.cnt_login_error = 0;
                    db.SaveChanges();
                }

            }
            catch (Exception ex)
            {
                ErrorLog.WriteLog(ex.Message);
            }
        }

        public bool IsUser(String userName)
        {
            if (db.USERS.Where(x => x.login_id == userName).Count() > 0)
                return true;
            return false;
        }
      
    }
}
