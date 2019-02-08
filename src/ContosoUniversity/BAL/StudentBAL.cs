using ContosoUniversity.DAL;
using ContosoUniversity.Models;
using ContosoUniversity.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ContosoUniversity.BAL
{
    public class StudentBAL
    {
        // private SchoolContext db = new SchoolContext();
         public Student TestRegisteringStudent(PersonRegisterVM TestAccount, SchoolContext db)

        {
            Student st = new Student();

            st.Email = TestAccount.Email;
            st.LastName = TestAccount.LastName;
            st.FirstMidName = TestAccount.FirstName;
            st.Password = TestAccount.Password;
            st.EnrollmentDate = DateTime.Now;
            //st.FilePaths = new List<FilePath>();
            //st.FilePaths.Add(photo);

            db.Students.Add(st);
            db.SaveChanges();


            return st;
        }

        //public ActionResult TestRegisteringStudentExist(PersonRegisterVM TestAccount, SchoolContext db)
        //{
        //    if (db.People.Any(m => m.Email == TestAccount.Email))
        //    {
        //        TempData["errorMail"] = "Email exist already!";
        //        return RedirectToAction("Register");
        //    }

        //}
    }
}
    
