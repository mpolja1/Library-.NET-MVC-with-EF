using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Library_admin.Models.CustomAttributes
{
    public class IsAuthorized : FilterAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.Session["Employee"] == null)
            {
                filterContext.Result = new RedirectResult("/Home/Login");
            }
        }
    }
}