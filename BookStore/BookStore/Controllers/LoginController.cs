using BookStore.Commons;
using BookStore.Models;
using DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BookStore.Controllers
{
    public class LoginController : Controller
    {
        #region Login

        // GET: User
        public ActionResult Index()
        {
            ViewBag.Title = "Login User - BookStore";
            return View();
        }
        [HttpPost]
        public ActionResult Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                // check valid user login
                if (UserDao.Instance.IsUserByUserName(model.userName))
                {
                    String password = Services.Md5Hash(Constants.SALT + model.password);
                    // check valid control login
                    if (Convert.ToInt32(SystemDao.Instance.GetValueConfigById(Constants.SYSTEM_CONFIG_STATUS_LOCK)) == 0)
                    {
                        //check locked of user
                        if (UserDao.Instance.CheckNumLoginFail(model.userName, Convert.ToInt32(SystemDao.Instance.GetValueConfigById(Constants.SYSTEM_CONFIG_COUNT_LOCK)), Convert.ToInt32(SystemDao.Instance.GetValueConfigById(Constants.SYSTEM_CONFIG_TIME_LOCK))))
                        {
                            ModelState.AddModelError("", "Acount đang bị lock Hãy đợi một lác rồi login lại nhé");
                            return View("Index");
                        }
                        else
                        {
                            if (_Login(model.userName, password))
                            {
                                // reset cnt_login_error. date_login_error
                                UserDao.Instance.ResetCountLoginAndDateLoginError(model.userName);
                                // redirect to Home page
                                return RedirectToAction("Index", "Home");
                            }
                            else
                            {
                                ModelState.AddModelError("", "LoginID hoặc password không chính xác");
                                UserDao.Instance.UpdateCountLoginAndDateLoginError(model.userName);
                                return View("Index");
                            }
                        }
                    }
                    else
                    {
                        if (_Login(model.userName, password))
                            return RedirectToAction("Index", "Home");
                        return View("Index");
                    }
                }
            }
            ModelState.AddModelError("", "LoginID hoặc password không chính xác");
            return View("Index");
        }

        #endregion

        #region Logout
        public ActionResult Logout()
        {
            if (Request.Cookies[Constants.COOKIE_NAME] != null)
            {

                String cookie = GetCookieFromBowser(Constants.COOKIE_NAME);
                // delete cookie from db
                if (UserDao.Instance.DelCookie(cookie))
                {
                    DelCookieFromBrowser(Constants.COOKIE_NAME);
                }
            }
            return RedirectToAction("Index", "Login");
        }
        #endregion



        #region Method

        public bool _Login(String name, String password)
        {
            if (UserDao.Instance.Login(name, password))
            {
                //create cookie
                String timeNow = DateTime.Now.ToString("yyyy-mm-dd hh:mi:ss.mmm");
                String cookieString = Services.Md5Hash(name + password + timeNow);
                // Save cookie to browser.
                int configValue = Convert.ToInt32(SystemDao.Instance.GetValueConfigById(Constants.SYSTEM_CONFIG_EXPIRES_COOKIE));
                SaveCookieToBrowser(Constants.COOKIE_NAME, cookieString, configValue);
                // save cookie to db
                UserDao.Instance.SaveCookieToDb(name, cookieString);
                // Save session
                UserSession userSession = new UserSession();
                userSession.GetUserSession(name, cookieString);
                Session.Add(Constants.USER_SESSION_KEY, userSession);
                return true;
            }
            return false;
        }
        /// <summary>
        /// save cookie to browser
        /// </summary>
        /// <param name="cookieName">cookie name</param>
        /// <param name="cookieValue">cookie value</param>
        /// <param name="expires">time expires - day value</param>
        private void SaveCookieToBrowser(String cookieName, String cookieValue, int expires)
        {
            HttpCookie cookieLogin = new HttpCookie(cookieName);
            cookieLogin.Value = cookieValue;
            cookieLogin.Expires = DateTime.Now.AddDays(expires);
            Response.Cookies.Add(cookieLogin);
        }

        private void DelCookieFromBrowser(String cookieName)
        {
            if (Request.Cookies[cookieName] != null)
            {
                Response.Cookies[cookieName].Expires = DateTime.Now.AddDays(-1);
            }
            
           
        }

        private String GetCookieFromBowser(String cookieName)
        {
            //Fetch the Cookie using its Key.
            HttpCookie nameCookie = Request.Cookies[cookieName];
            //If Cookie exists fetch its value.
            string values = nameCookie != null ? nameCookie.Value : "";
            return values;
        }

        #endregion

    }
}