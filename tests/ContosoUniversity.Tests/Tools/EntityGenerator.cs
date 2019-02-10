using ContosoUniversity.DAL;
using ContosoUniversity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContosoUniversity.Tests.Tools
{
    public class EntityGenerator
    {
        private readonly SchoolContext dbContext;

        public EntityGenerator(SchoolContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public Student CreateStudent(string lastname, string firstname, string email, string password,DateTime aDate)
        {
            var student = new Student()
            {
                LastName = lastname,
                FirstMidName = firstname,
                Email = email,
                Password = password,
                EnrollmentDate = aDate
                 
            };
            
            this.dbContext.Students.Add(student);
            this.dbContext.SaveChanges();
           
            return student;
        }
    }
}
