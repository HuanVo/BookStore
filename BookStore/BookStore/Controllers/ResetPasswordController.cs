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
            return View();
        }
        
        public ActionResult FinishHome()
        {
            return View();
        }
        public ActionResult ResetPass(ResetPasswordModel model)
        {
            if(ModelState.IsValid)
            {
                String email = model.email;
                //// creates a 8 digit random
                if(UserDAO.Instance.IsEmail(email))
                {
                    Random rnd = new Random();
                    int randPass = rnd.Next(10000000, 99999999);
                    //hashing new password
                    String newPassword = Services.MD5Hash(Constancs.SALT + randPass.ToString());
                    // update to db
                    if (UserDAO.Instance.UpdatePassReset(email, newPassword))
                    {
                        // send email
                        MAIL mail = MailDAO.Instance.GetInfo(1);
                        String[] bodyTemp = mail.body.Split('-');
                        String body = bodyTemp[0] + " " + email + "<br>" + bodyTemp[1] + " " + randPass + "<br>Xin cám ơn bạn đã sử dụng dịch vụ của chúng tôi!";
                        Services.SendMail(mail.from_address, email, mail.subjects, body, "huanit1237");
                        // Redirect to finish memnitor
                        return RedirectToAction("FinishHome", "ResetPassword");
                        // redirect to interface finish
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