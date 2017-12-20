using DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Filters;
using System.Web.Routing;
namespace BookStore.Commons

{
    public class LoginSessionAttribute: AuthorizeAttribute//System.Attribute, IAuthorizationFilter
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            // get session from browser
            var session = (UserSession)filterContext.HttpContext.Session[Constants.USER_SESSION_KEY];
            //throw new NotImplementedException();
            if (session == null)
            {
                // get cookie from browser
                var tempCookie = filterContext.HttpContext.Request.Cookies[Constants.COOKIE_NAME];
                if(tempCookie !=null)
                {
                    var cookie = tempCookie.Value;
                    if (cookie == null || (cookie != "" && !UserDao.Instance.IsUserByCookie(cookie)))
                    {
                        filterContext.Result = new RedirectToRouteResult(
                           new RouteValueDictionary(new { controller = "Login", action = "Index" })
                       );
                    }
                }
                else
                {
                    {
                        filterContext.Result = new RedirectToRouteResult(
                           new RouteValueDictionary(new { controller = "Login", action = "Index" })
                       );
                    }
                }
            }
        }
    }
}