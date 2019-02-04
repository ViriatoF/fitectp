using ContosoUniversity.DAL;
using ContosoUniversity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace ContosoUniversity.Controllers
{
    public class AccountController : Controller
    {
        private SchoolContext db = new SchoolContext();
        // GET: Account
        [HttpGet]
        public ActionResult Index()
        {
            var errMsg = TempData["ErrorMessage"] as string;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index([Bind(Include = "Email,Password")]Student student)
        {
            //string a = student.Email;
            if (!ModelState.IsValid)
            {

           // Type a =db.People.Where(x => x.Email == student.Email).GetType();
            

           
                int validEmail = db.Students.Where(x => x.Email == student.Email && x.Password.Equals(student.Password)).Count(); //|| db.Instructors.Any(x => x.Email == aEmail);

                if (validEmail==0)
                {
                    //return HttpNotFound("Email Non Correcte");
                    TempData["ErrorMessage"] = "This is the message";
                    return RedirectToAction(nameof(Index));
                }

                return RedirectToAction(actionName: "Index", controllerName: "Student");
            }
            return View();
        }
    }
}