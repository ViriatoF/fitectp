using ContosoUniversity.DAL;
using ContosoUniversity.Models;
using ContosoUniversity.ViewModels;
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
            var errMsg = TempData["ErrorMessage"] as string; //To import an error message with redirectoaction 
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public ActionResult Index([Bind(Include = "Email,Password")]Student student)
        {
            //string a = student.Email;
            if (!ModelState.IsValid)
            {

           // Type a =db.People.Where(x => x.Email == student.Email).GetType();
            
                // TODO : Extract predicates for authentication to  avoid duplication above
                //s'il a un etudiant correspondant 
                int validEmailS = db.Students.Where(x => x.Email == student.Email && x.Password.Equals(student.Password)).Count(); //|| db.Instructors.Any(x => x.Email == aEmail);
                
                //s'il a un Instructor correspondant 
                int validEmailI = db.Instructors.Where(x => x.Email == student.Email && x.Password.Equals(student.Password)).Count(); //|| db.Instructors.Any(x => x.Email == aEmail);


                int validEmailT = validEmailI + validEmailS;
                if (validEmailT ==0)
                {
                    //return HttpNotFound("Email Non Correcte");
                    TempData["ErrorMessage"] = "Invalid Email or Password";
                    return RedirectToAction(nameof(Index));
                }
                if (validEmailS !=0)
                {
                    Student st = new Student();
                    Session["User"] = st;
                    if (Session["User"] != null)
                    {
                        return RedirectToAction(actionName: "Index", controllerName: "Student");
                    }
                }

                if (validEmailI != 0)
                {
                    Instructor inst = new Instructor();
                    Session["User"] = inst;
                    if (Session["User"] != null)
                    {
                        return RedirectToAction(actionName: "Index", controllerName: "Instructor");
                    }
                    
                }
                // return RedirectToAction(actionName: "Index", controllerName: "Student");
               // return RedirectToAction(actionName: "Index", controllerName: "Home"); 
            }
            return View();
        }
        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction(actionName: "Index", controllerName: "Home");
        }

        [HttpGet]
        public ActionResult Register()
        {
            //List<string> ListeStudentInstructor = new List<string>();
            //ListeStudentInstructor.Add("Student");
            //ListeStudentInstructor.Add("Instructor");
           // ViewBag.liste = ListeStudentInstructor;

            // Add some elements to the dictionary. There are no 
            // duplicate keys, but some of the values are duplicates.
            //liste.Add("ID", "Type");
            //liste.Add("1", "Student");
            //liste.Add("2", "Instructor");
            //ViewBag.liste = liste;
            //ViewBag.key = liste.Keys;

            List<SelectListItem> myList = new List<SelectListItem>();
            var data = new[]{
                 new SelectListItem{ Value="1",Text="Student"},
                 new SelectListItem{ Value="2",Text="Instructor"},
             };
            myList.Add(new SelectListItem { Value = "1", Text = "Student" });
            myList.Add(new SelectListItem { Value = "2", Text = "Instructor" });

            //myList = data.ToList();

            ViewBag.liste = myList;
            return View();
        }
        [HttpPost]
        public ActionResult Register(PersonRegisterVM User)
        {
            if (db.People.Any(m=>m.Email==User.Email))
            {
                TempData["errorMail"] = "Email exist already!";
                return RedirectToAction("Register");
            }

            if (User.Role =="Student")
            {
                Student st = new Student();
                st.Email = User.Email;
                st.LastName = User.LastName;
                st.FirstMidName = User.FirstMidName;
                st.Password = User.Password;
                st.EnrollmentDate = DateTime.Now;
                db.Students.Add(st);

            }
            if (User.Role == "Instructor")
            {
                Instructor ins = new Instructor();
                ins.Email = User.Email;
                ins.LastName = User.LastName;
                ins.FirstMidName = User.FirstMidName;
                ins.Password = User.Password;
                ins.HireDate = DateTime.Now;
                db.Instructors.Add(ins);

            }
            db.SaveChanges();
            return RedirectToAction("Register");
        }
    }
}