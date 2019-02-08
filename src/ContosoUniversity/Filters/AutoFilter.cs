using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using System.Web.Routing;
using System.Web.Mvc.Filters;

namespace ContosoUniversity.Filters
{
    public class AutoFilter : System.Web.Http.AuthorizeAttribute
    {

        public void OnAuthentication(AuthenticationContext filterContext)
        {
            //Check Session is Empty Then set as Result is HttpUnauthorizedResult 
            if (HttpContext.Current.Session["User"] is null)
            {
                //filterContext.Result = new HttpUnauthorizedResult();
            }
        }

    }
}