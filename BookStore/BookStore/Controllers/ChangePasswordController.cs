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
            return View();
        }
        public ActionResult ChangePass()
        {

            if(ModelState.IsValid)
            {

            }
            
            return View("Index");
        }


    }
}