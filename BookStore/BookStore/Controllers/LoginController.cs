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
        // GET: User
        public ActionResult Index()
        {
            ViewBag.Title = "Login User - BookStore";
            return View();
        }
        public ActionResult Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                // check valid user login
                if (UserDAO.Instance.IsUser(model.userName))
                {
                    String password = Encryptor.MD5Hash(Constancs.SALT + model.password);
                    // check valid control login
                    if (Convert.ToInt32(SystemDAO.Instance.GetValueConfigById(1)) == 0)
                    {
                        //check locked of user
                        if (UserDAO.Instance.CheckNumLoginFail(model.userName, Convert.ToInt32(SystemDAO.Instance.GetValueConfigById(2)), Convert.ToInt32(SystemDAO.Instance.GetValueConfigById(3))))
                        {
                            ModelState.AddModelError("", "Acount đang bị lock Hãy đợi một lác rồi login lại nhé");
                        }
                        else
                        {
                            if (_Login(model.userName, password))
                            {
                                // reset cnt_login_error. date_login_error
                                UserDAO.Instance.ResetCountLoginAndDateLoginError(model.userName);
                                // redirect to Home page
                                return RedirectToAction("Index", "Home");
                            }
                            else
                            {
                                ModelState.AddModelError("", "LoginID hoặc password không chính xác");
                                UserDAO.Instance.UpdateCountLoginAndDateLoginError(model.userName);
                                return View("Index");
                            }
                        }
                    }
                    else
                    {
                        if(_Login(model.userName, password))
                            return RedirectToAction("Index", "Home");
                        return View("Index");
                    }
                }

            }
            return View("Index");
        }
        public bool _Login(String name, String password)
        {
            if(UserDAO.Instance.Login(name, password))
            {
                //create cookie
                String timeNow = DateTime.Now.ToString("yyyy-mm-dd hh:mi:ss.mmm");
                String cookieString = Encryptor.MD5Hash(name + password + timeNow);

                // Save cookie to browser.
                int configValue = Convert.ToInt32(SystemDAO.Instance.GetValueConfigById(21));
                SaveCookieToBrowser("m", cookieString, configValue);

                // save cookie to db
                UserDAO.Instance.SaveCookieToDB(name, cookieString);
                
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

    }
}