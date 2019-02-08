using ContosoUniversity.DAL;
using ContosoUniversity.Enums;
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
        

            List<SelectListItem> myList = new List<SelectListItem>();
            //var data = new[]{
            //     new SelectListItem{ Value="1",Text="Student"},
            //     new SelectListItem{ Value="2",Text="Instructor"},
            // };
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

            //My Register method to register a new account
            aStudent.TestRegisteringStudent(User, db);
            if (ModelState.IsValid)
            {
                //prepraing  a test
               // aStudent.TestRegisteringStudentExist(User, db);

                if (db.People.Any(m => m.Email == User.Email))
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
                       // BAL.StudentBAL aStudent = new BAL.StudentBAL();

                        //My Register method to register a new account
                        aStudent.TestRegisteringStudent(User,db);
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