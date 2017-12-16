using BookStore.Models;
using DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BookStore.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        public ActionResult Index()
        {
            ViewBag.Title = "This is Login User";
            return View();
        }

        public ActionResult Login(LoginModel model)
        {
            if(ModelState.IsValid)
            {
                
                if (UserDAO.Instance.Login(model.UserName, model.Password))
                {
                    Session.Add("m", model.UserName);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("","Đăng nhập không đúng");
                }
            }
            return View("Index");
           
        }

        //public ActionResult ChangePassword(LoginModel model)
        //{
        //    if(ModelState.IsValid)
        //    {

        //    }
        //}
    }
}