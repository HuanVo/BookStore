using BookStore.Commons;
using BookStore.Models;
using DAL;
using DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BookStore.Controllers
{
    public class ChangePasswordController : Controller
    {
        // GET: ChangePassword
        public ActionResult Index()
        {
            ViewBag.Title = "Change password - BookStore";
            return View();
        }

        public ActionResult FishHome()
        {
            ViewBag.Title = "Change password - BookStore";
            return View();
        }

        public ActionResult ChangePass(ChangePassMode model )
        { 
            if(ModelState.IsValid)
            {
                // get cookie from browser
                String cookieString = GetCookieFromBowser(Constancs.COOKIE_NAME);
                String userName = (String)Session[Constancs.USER_SESSION];
                if(userName != "")
                {
                    String oldMail = Services.MD5Hash(Constancs.SALT + model.oldPass);
                    if (UserDAO.Instance.IsUserWithCookie(userName, cookieString, oldMail))
                    {
                        if (model.newPass == model.cfmPass)
                        {
                            String password = Services.MD5Hash(Constancs.SALT + model.newPass);
                            // save new password 
                            if (UserDAO.Instance.UpdateNewPassword(userName, password))
                            {
                                // sent mail 
                                MAIL mail = MailDAO.Instance.GetInfo(2);
                                String[] bodyTemp = mail.body.Split('-');
                                String body = bodyTemp[0] + " " + userName + "<br>" + bodyTemp[1] + " " + model.newPass + "<br>Xin cám ơn bạn đã sử dụng dịch vụ của chúng tôi!";
                                Services.SendMail(mail.from_address, UserDAO.Instance.GetMailById(userName), mail.subjects, body, "huanit1237");
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
            string values = nameCookie != null ? nameCookie.Value : "undefined";
            return values;
        }
    }
}