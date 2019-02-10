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
        private SchoolContext db = new SchoolContext();

        public StudentBAL(SchoolContext _db)
        {
            this.db = _db;
        }

        public Person RegisteringNewAccount(PersonRegisterVM TestAccount)

        {
            var photo = new FilePath
            {
                FileName = TestAccount.ImagePath,
                FileType = TestAccount.ImageType
            };

            if (TestAccount.Role == "1")
            {
               

                Student st = new Student();

                st.Email = TestAccount.Email;
                st.LastName = TestAccount.LastName;
                st.FirstMidName = TestAccount.FirstName;
                st.Password = TestAccount.Password;
                st.EnrollmentDate = DateTime.Now;
                st.FilePaths = new List<FilePath>();
                st.FilePaths.Add(photo);

                db.Students.Add(st);
                db.SaveChanges();


                return st;
            }
            else                                                             //(TestAccount.Role == "2")
            {
                Instructor ins = new Instructor();
                ins.Email = TestAccount.Email;
                ins.LastName = TestAccount.LastName;
                ins.FirstMidName = TestAccount.FirstName;
                ins.Password = TestAccount.Password;
                ins.HireDate = DateTime.Now;

                ins.FilePaths = new List<FilePath>();
                ins.FilePaths.Add(photo);

                db.Instructors.Add(ins);
                db.SaveChanges();

                return ins;
            }

          
        }




        public bool AccountExist(PersonRegisterVM TestAccount)
        {
            if (db.People.Any(m => m.Email == TestAccount.Email))
            {
                return true;
            }
            return false;
        }





        public bool RegisteringImage(string aPath, PersonRegisterVM TestAccount)
        {

            return (aPath == System.IO.Path.GetFileName(TestAccount.ImagePath)) ? true : false;
        }
    }
}
    
