using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Routing;
using ContosoUniversity.Models;
using ContosoUniversity.DAL;

namespace ContosoUniversity.API
{
    public class StudentApiController : ApiController
    {
        private SchoolContext db = new SchoolContext();

        //Get API/<controller>/id
        [HttpGet]
        public IHttpActionResult GetStudent(int id)
        {
            //On récupère le student en fonction de l'id entré en paramètre
            var student = db.Students.Find(id);
            
            Dictionary<string, object> listProp = new Dictionary<string, object>();
            
            //On créé une liste qu'on passera dans le dictionnaire pour les CourseId
            List<string> listCourseId = new List<string>();

            listProp.Add("Id : ", student.ID);
            listProp.Add("Last name : ", student.LastName);
            listProp.Add("First name : ", student.FirstMidName);
            listProp.Add("Enrollment date : ", student.EnrollmentDate.ToString());
            listProp.Add("Enrollments : ", listCourseId);
            
            //On ajoute à la liste chaque courseId trouvée
            foreach (var item in student.Enrollments)
            {
                listCourseId.Add("CourseId : " + item.CourseID);
            }
            
            //On retourne le dictionnaire pour l'affichage
            return Ok(listProp);
        }
    }
}
