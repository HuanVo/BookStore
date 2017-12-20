using BookStore.Commons;
using BookStore.Models;
using DAL;
using DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Web;
using System.Web.Mvc;
using BookStore.ViewModels;
using System.Web.Configuration;

namespace BookStore.Controllers
{




    [LoginSession]
    public class ChangePasswordController : Controller
    {
        // GET: ChangePassword

        //[LoginSession]
        public ActionResult Index()
        {
            ViewBag.Title = "Change password";
            return View();
        }
        public ActionResult FishHome()
        {
            ViewBag.Title = "Change password";
            return View();
        }
        
        public ActionResult ChangePass(ChangePassMode model)
        {
            if (ModelState.IsValid)
            {
                // get cookie from browser
                String cookieString = GetCookieFromBowser(Constants.COOKIE_NAME);

                String oldPass = Services.Md5Hash(Constants.SALT + model.oldPass);
                if (UserDao.Instance.IsUserWithCookie(cookieString, oldPass))
                {
                    if (model.newPass == model.cfmPass)
                    {
                        String password = Services.Md5Hash(Constants.SALT + model.newPass);
                        // save new password
                        if (UserDao.Instance.UpdateNewPasswordWithCookie(cookieString, password))
                        {
                            // sent mail
                            USER user = UserDao.Instance.GetUserByCookie(cookieString);
                            MAIL mail = MailDao.Instance.GetInfo(Constants.MAIL_ID_CHANGE_PASSWORD);
                            String[] bodyTemp = mail.body.Split('-');
                            String body = bodyTemp[0] + " " + user.login_id + "<br>" + bodyTemp[1] + " " + model.newPass + "<br>Xin cám ơn bạn đã sử dụng dịch vụ của chúng tôi!";
                            string key = Constants.KEY_ENCRYPT;
                            string pass = WebConfigurationManager.AppSettings["KeyMail"];
                            Services.SendMail(mail.from_address, user.mail, mail.subjects, body, Services.DecryptMessage(pass, key));

                            // delete cookie and session
                            Session.Clear();
                            if (Request.Cookies[Constants.COOKIE_NAME] != null)
                            {

                                String cookie = GetCookieFromBowser(Constants.COOKIE_NAME);
                                // delete cookie from db
                                if (UserDao.Instance.DelCookie(cookie))
                                {
                                    DelCookieFromBrowser(Constants.COOKIE_NAME);
                                }
                            }
                            // Redirect to finish memnitor
                            return RedirectToAction("FishHome", "ChangePassword");
                        }
                        else
                        {
                            ModelState.AddModelError("", "Không thể đổi mật khẩu");
                            return View("Index");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "Mật khẩu xác nhận không đúng");
                        return View("Index");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Truy cập trái phép");
                }
            }
            return View("Index");
        }
        /// <summary>
        /// Get cookie from browser
        /// </summary>
        /// <param name="cookieName">cookie name</param>
        /// <returns>Get a String Value cookie</returns>
        private String GetCookieFromBowser(String cookieName)
        {
            //Fetch the Cookie using its Key.
            HttpCookie nameCookie = Request.Cookies[cookieName];

            //If Cookie exists fetch its value.
            string values = nameCookie != null ? nameCookie.Value : "";
            return values;
        }

        private void DelCookieFromBrowser(String cookieName)
        {
            if (Request.Cookies[cookieName] != null)
            {
                Response.Cookies[cookieName].Expires = DateTime.Now.AddDays(-1);
            }


        }
    }
}