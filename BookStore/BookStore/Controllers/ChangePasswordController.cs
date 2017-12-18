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



namespace BookStore.Controllers
{
    public class ChangePasswordController : Controller
    {
        // GET: ChangePassword
        public ActionResult Index()
        {
            String cookieString = GetCookieFromBowser(Constancs.COOKIE_NAME);
            if (cookieString != "")
            {

                ViewBag.Title = "Change password";
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
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
                String cookieString = GetCookieFromBowser(Constancs.COOKIE_NAME);

                String oldMail = Services.Md5Hash(Constancs.SALT + model.oldPass);
                if (UserDao.Instance.IsUserWithCookie(cookieString, oldMail))
                {
                    if (model.newPass == model.cfmPass)
                    {
                        String password = Services.Md5Hash(Constancs.SALT + model.newPass);
                        // save new password
                        if (UserDao.Instance.UpdateNewPasswordWithCookie(cookieString, password))
                        {
                            // sent mail
                            USER user = UserDao.Instance.GetUserByCookie(cookieString);
                            MAIL mail = MailDao.Instance.GetInfo(Constancs.MAIL_ID_CHANGE_PASSWORD);
                            String[] bodyTemp = mail.body.Split('-');
                            String body = bodyTemp[0] + " " + user.login_id + "<br>" + bodyTemp[1] + " " + model.newPass + "<br>Xin cám ơn bạn đã sử dụng dịch vụ của chúng tôi!";
                            Services.SendMail(mail.from_address, user.mail, mail.subjects, body, "huanit1237");
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
    }
}