﻿using System;
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
                // Save error to a file log
                ErrorLog.WriteLog(ex.Message);
            }
        }
        /// <summary>
        /// Check status number login fail
        /// </summary>
        /// <param name="loginId">login id</param>
        /// <param name="cntLoginConfig">count login System config</param>
        /// <param name="expiresTimeConfig">Time expires lock</param>
        /// <returns>Get bool value status Login fail</returns>
        public bool CheckNumLoginFail(String loginId, int cntLoginConfig, int expiresTimeConfig)
        {
            try
            {
                // chua hoan thanh so sanh gio
                DateTime otherDate = DateTime.Now.AddMinutes(-expiresTimeConfig);
                var result = (from a in db.USERS
                              join b in db.CUSTOMERs on a.code_cst equals b.code_cst
                              where b.dest_flg == 0 && a.dest_flg == 0 &&  a.login_id == loginId && a.date_login_error != null && a.cnt_login_error<cntLoginConfig ||  a.date_login_error > otherDate 
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

        /// <summary>
        /// Update count login and date login error 
        /// </summary>
        /// <param name="loginId">id user login</param>
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
                // save error to a file log
                ErrorLog.WriteLog(ex.Message);
            }
        }
        /// <summary>
        /// reset date login error and count login error to default value
        /// </summary>
        /// <param name="loginId">id user login</param>
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
        /// <summary>
        /// Check isset user login
        /// </summary>
        /// <param name="userName">id login user</param>
        /// <returnsget bool value></returns>
        public bool IsUser(String userName)
        {
            if (db.USERS.Where(x => x.login_id == userName).Count() > 0)
                return true;
            return false;
        }

        public bool IsUserWithCookie(String userName, String cookieString, String oldPass)
        {
            try
            {
                var result = (from a in db.USERS
                              join b in db.CUSTOMERs on a.code_cst equals b.code_cst
                              where a.login_id == userName && a.dest_flg == 0 && b.dest_flg == 0 && a.login_pass == oldPass && a.loginkey == cookieString
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
        /// Update pass
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="newPassword"></param>
        /// <returns></returns>
        public bool UpdateNewPassword(String userName, String newPassword)
        {
            try
            {
                var check = db.USERS.Find(userName);
                if (check != null)
                {
                    check.login_pass = newPassword;
                    db.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                ErrorLog.WriteLog(ex.Message);
            }
            return false;
        }

        /// <summary>
        /// Get email by id user login
        /// </summary>
        /// <param name="userId">id user login</param>
        /// <returns>Get a String value  email</returns>
        public String GetMailById(String userId)
        {
            String mail = "";
            try
            {
                mail = db.USERS.Find(userId).mail;
            }
            catch(Exception ex)
            {
                ErrorLog.WriteLog(ex.Message);
            }
            return mail;
        }

        /// <summary>
        /// Check email to send email reset password
        /// </summary>
        /// <param name="email">email input</param>
        /// <returns>get bool value true if is email else false if not email</returns>
        public bool IsEmail(String email)
        {
            try
            {
                var result = (from a in db.USERS
                              join b in db.CUSTOMERs on a.code_cst equals b.code_cst
                              where a.dest_flg == 0 && b.dest_flg == 0 && a.mail == email
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
      
    }
}
