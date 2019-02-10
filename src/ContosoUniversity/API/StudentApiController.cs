using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Routing;
using ContosoUniversity.Models;
using ContosoUniversity.DAL;
using ContosoUniversity.ViewModels;

namespace ContosoUniversity.API
{
    public class StudentsController : ApiController
    {
           
        private SchoolContext db = new SchoolContext();

        //Get API/<controller>/id
        [HttpGet]
        public StudentsApi GetStudent( int id)
        {
            //On créé une nouvelle instance de StudentsApi avec lequel on va travailler
            StudentsApi StudentApi = new StudentsApi();
            EnrollmentVM EnrApi = new EnrollmentVM();

            //On récupère le student en fonction de l'id entré en paramètre
            Student aStudent = db.Students.Find(id);

            //On remplit le viewmodel avec les données du student
            StudentApi.id = aStudent.ID;
            StudentApi.lastname = aStudent.LastName;
            StudentApi.firstname = aStudent.FirstMidName;
            StudentApi.enrollmentdate = aStudent.EnrollmentDate;

            //On créé une variable pour récupérer  le nombre d'enrollments du student
            var cpt = aStudent.Enrollments.Count();

            //On créé un nouveau tableau pour stocker les informations qu'on va récupérer ensuite
            EnrollmentVM[] aEnrollments = new EnrollmentVM[cpt];

            for (int i = 0; i < cpt; i++)
            {
                //On récupère les données dont on a besoin et on les rajoute au tableau précédemment créé
                aEnrollments[i] = new EnrollmentVM { courseId = aStudent.Enrollments.ElementAt(i).CourseID };
            }
            
            StudentApi.enrollments = aEnrollments;
            
            //On retourne l'objet pour l'affichage
            return StudentApi;
        }
    }
}
