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
using ContosoUniversity.Models;

namespace ContosoUniversity.Filters
{

    //specified the type of application of filter: class or method

    //1) Method
    // [AttributeUsage(AttributeTargets.Method)] 


    //2) Class 
    [AttributeUsage(AttributeTargets.Class)]
    public class AutoAttribute : System.Web.Mvc.AuthorizeAttribute
    {
        //argument to the filter
        public string Role { get; set; }

        //Jsut an  example to give the access only for the Instructor 
        //Techinical point: overriden AuthorizeCore function to give the acces
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
           if (HttpContext.Current.Session["User"] == null)
           {
                return false;
           }
            else if (Role =="Student" && HttpContext.Current.Session["User"] is Student)
            {
                return false;
            }
            else
            {
                return true;
            }
           
        }



    }
}