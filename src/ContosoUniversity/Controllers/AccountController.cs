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
                    return RedirectToAction(actionName: "Index", controllerName: "Student");
                    
                }

                if (aPerson is Instructor)
                {
                    Instructor inst = new Instructor();
                    Session["User"] = inst;
                    return RedirectToAction(actionName: "Index", controllerName: "Instructor");
                    
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
        public ActionResult Register(PersonRegisterVM User, HttpPostedFileBase file)
        {
            //an object to have access to the tests methodes
            BAL.StudentBAL aStudent = new BAL.StudentBAL();

          
            if (ModelState.IsValid)
            {
                //prepraing  a test 
               // aStudent.TestRegisteringStudentExist(User, db);

                if (aStudent.TestRegisteringStudentExist(User, db))
                {
                    TempData["errorMail"] = "Email exist already!";
                    return RedirectToAction("Register");
                }

                //ulpoading image png and jpg

                //supporting an image
                if (file != null && 0 < file.ContentLength)
                {

                    var photo = new FilePath
                    {
                        FileName = System.IO.Path.GetFileName(file.FileName),
                        FileType = FileType.png
                    };

                    //only format png and jpg
                    if (file.ContentType != "image/" + FileType.png.ToString() && file.ContentType != "image/" + FileType.jpeg.ToString())
                    {
                        photo.FileType = FileType.png;
                        TempData["ErrorUploading"] = "Format Image not supported!";
                        return RedirectToAction("Register");
                    }

                    //MAxByteValue 100kb;
                    if (file.ContentLength > 100 * 1000)
                    {
                        TempData["ErrorUploadingTaille"] = "Size Image not supported!";
                        return RedirectToAction("Register");
                    }



                    if (User.Role == "1")
                    {

                        //st.FilePaths = new List<FilePath>();
                        //st.FilePaths.Add(photo);


                        //My Register method for a new account
                        //aStudent.TestRegisteringStudent(User,db).FilePaths.Add(photo);
                        aStudent.TestRegisteringStudent(User, db);

                        //db.Entry(aStudent.TestRegisteringStudent(User, db)).State = EntityState.Modified;
                        //db.SaveChanges();

                        return RedirectToAction("Index", "Student");

                    }

                  


                    if (User.Role == "2")
                    {
                        Instructor ins = new Instructor();
                        ins.Email = User.Email;
                        ins.LastName = User.LastName;
                        ins.FirstMidName = User.FirstName;
                        ins.Password = User.Password;
                        ins.HireDate = DateTime.Now;

                        ins.FilePaths = new List<FilePath>();
                        ins.FilePaths.Add(photo);

                        db.Instructors.Add(ins);
                        db.SaveChanges();
                        return RedirectToAction("Index", "Instructor");


                    }
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