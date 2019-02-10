using ContosoUniversity.DAL;
using ContosoUniversity.Enums;
using ContosoUniversity.Models;
using ContosoUniversity.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace ContosoUniversity.Controllers
{
    public class AccountController : Controller
    {
        private SchoolContext db = new SchoolContext();

        public SchoolContext DbContext
        {
            get { return db; }
            set { db = value; }
        }

        // GET: Account where Index is the Login Action
        [HttpGet]
        public ActionResult Index()
        {
           // var errMsg = TempData["ErrorMessage"] as string; //To import an error message with redirectoaction 
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public ActionResult Index(PersonRegisterVM UserLog)
        {
            //string a = student.Email;
            if (!ModelState.IsValid)
            {

           
            
                // TODO : Extract predicates for authentication to  avoid duplication above
                


                Person aPerson = db.People.FirstOrDefault(x => x.Email == UserLog.Email && x.Password ==UserLog.Password);

           
                if (aPerson == null)
                {
                    //return HttpNotFound("Email Non Correcte");
                    TempData["ErrorMessage"] = "Invalid Email or Password";
                    return RedirectToAction(nameof(Index));
                }
                if (aPerson is Student)
                {
                    Student st = new Student();
                    Session["User"] = st;
                    return RedirectToAction(actionName: "Index", controllerName: "Home");
                    
                }

                if (aPerson is Instructor)
                {
                    Instructor inst = new Instructor();
                    Session["User"] = inst;
                    return RedirectToAction(actionName: "Index", controllerName: "Home");
                    
                }
                
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
        

            List<SelectListItem> myList = new List<SelectListItem>();
         
            myList.Add(new SelectListItem { Value = "1", Text = "Student" });
            myList.Add(new SelectListItem { Value = "2", Text = "Instructor" });

            //myList = data.ToList();

            ViewBag.liste = myList;
            return View();
        }

        [HttpPost]
        public ActionResult Register(PersonRegisterVM User, HttpPostedFileBase Image)
        {
            //an object to have access to the tests methodes
            BAL.StudentBAL aStudent = new BAL.StudentBAL(db);

            //proprities of image to give of view model
            User.ImagePath = System.IO.Path.GetFileName(Image.FileName);
            User.ImageType = Image.ContentType;


            if (ModelState.IsValid)
            {
                //prepraing  a test 
               // aStudent.TestRegisteringStudentExist(User, db);

                if (aStudent.AccountExist(User))
                {
                    TempData["errorMail"] = "Email exist already!";
                    return RedirectToAction("Register");
                }

                //ulpoading image png and jpg

                //supporting an image
                if (Image != null && 0 < Image.ContentLength)
                {

                    //accessible image format

                    var validImageTypes = new List<string>() {"image/png","image/jpeg" };

                    //only format png and jpg

                    //old way with enum
                    if (!validImageTypes.Contains(Image.ContentType))
                    {
                                         
                        TempData["ErrorUploading"] = "Format Image not supported!";
                        return RedirectToAction("Register");
                    }

                    //MAxByteValue 100kb;
                    if (Image.ContentLength > 100 * 1000)
                    {
                        TempData["ErrorUploadingTaille"] = "Size Image not supported!";
                        return RedirectToAction("Register");
                    }
                    //My Register method for a new student account and adding the person to a session to keep it Login
                    
                    Session["User"] =  aStudent.RegisteringNewAccount(User);

                    

                    return RedirectToAction(nameof(Index),"Student");
                   
                }
            }
            
            

            List<SelectListItem> myList = new List<SelectListItem>();
            
            myList.Add(new SelectListItem { Value = "1", Text = "Student" });
            myList.Add(new SelectListItem { Value = "2", Text = "Instructor" });

            //myList = data.ToList();

            ViewBag.liste = myList;
            return View();
            
            //return RedirectToAction("Register");
        }

    }
}