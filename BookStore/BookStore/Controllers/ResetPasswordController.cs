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
    public class ResetPasswordController : Controller
    {
        // GET: ResetPassword
        public ActionResult Index()
        {
            ViewBag.title = "Reset password";
            return View();
        }
        
        public ActionResult FinishHome()
        {

            ViewBag.title = "finish home";
            return View();
        }
        public ActionResult ResetPass(ResetPasswordModel model)
        {
            if(ModelState.IsValid)
            {
                String email = model.email;
                //// creates a 8 digit random
                if(UserDao.Instance.IsEmail(email))
                {
                    Random rnd = new Random();
                    int randPass = rnd.Next(10000000, 99999999);
                    //hashing new password
                    String newPassword = Services.Md5Hash(Constancs.SALT + randPass.ToString());
                    // update to db
                    if (UserDao.Instance.UpdateNewPassword(email, newPassword))
                    {
                        // send email
                        MAIL mail = MailDao.Instance.GetInfo(Constancs.MAIL_ID_RESET_PASSWORD);
                        String[] bodyTemp = mail.body.Split('-');
                        String body = bodyTemp[0] + " " + email + "<br>" + bodyTemp[1] + " " + randPass + "<br>Xin cám ơn bạn đã sử dụng dịch vụ của chúng tôi!";
                        Services.SendMail(mail.from_address, email, mail.subjects, body, "huanit1237");
                        // Redirect to finish memnitor
                        return RedirectToAction("Index", "Login");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Không thể gửi mail");
                        return View("Index");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Mail không tồn tại");
                    return View("Index");
                }
            }
            return View("Index");
        }
    }
}