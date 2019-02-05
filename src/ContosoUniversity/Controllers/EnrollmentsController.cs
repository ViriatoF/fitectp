using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ContosoUniversity.DAL;
using ContosoUniversity.Models;

namespace ContosoUniversity.Controllers
{
    public class EnrollmentsController : Controller
    {
        private SchoolContext db = new SchoolContext();

        // GET: Enrollments
        public ActionResult Index()
        {
            var enrollments = db.Enrollments.Include(e => e.Course).Include(e => e.Student);
            return View(enrollments.ToList());
        }

        // GET: Enrollments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Enrollment enrollment = db.Enrollments.Find(id);
            if (enrollment == null)
            {
                return HttpNotFound();
            }
            return View(enrollment);
        }

        // GET: Enrollments/Create
        public ActionResult Create(int? id)
        {
            ViewBag.CourseID = new SelectList(db.Courses, "CourseID", "Title");
            ViewBag.StudentID = new SelectList(db.People, "ID", "LastName");
            //ViewBag.CurrentStudent = db.Students.FirstOrDefault(e => e.ID == id);
            return View();
        }

        // POST: Enrollments/Create
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(int? id,[Bind(Include = "EnrollmentID,CourseID,StudentID,Grade")] Enrollment enrollment)
        {
           // On verifie si les champs sont remplis
            if (ModelState.IsValid)
            {
                try
                {
                    //On vérifie si l'enrollment existe déjà dans la base de donnée
                    if (!db.Enrollments.Where(o => o.StudentID == enrollment.StudentID && o.CourseID == enrollment.CourseID).Any())
                    {
                        //Si elle n'existe pas, on l'ajoute à la BDD
                        db.Enrollments.Add(enrollment);
                        db.SaveChanges();
                        return RedirectToAction("Index", "Student");
                    }
                    //Si elle existe déjà, on renvoie un message d'erreur dans la vue
                    else
                    {
                        //Message d'erreur si l'enrollment existe déjà dans la BDD
                        ViewBag.SuscribeError = "You already subscribed to this lesson";
                    }
                }
                catch (Exception)
                {
                    
                }
                
                
            }
                ViewBag.CourseID = new SelectList(db.Courses, "CourseID", "Title", enrollment.CourseID);
                //ViewBag.CurrentStudent = db.Students.FirstOrDefault(e => e.ID == id);
                ViewBag.StudentID = new SelectList(db.People, "ID", "LastName", enrollment.StudentID);
                return View(enrollment);
            
            
        }

        // GET: Enrollments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Enrollment enrollment = db.Enrollments.Find(id);
            if (enrollment == null)
            {
                return HttpNotFound();
            }
            ViewBag.CourseID = new SelectList(db.Courses, "CourseID", "Title", enrollment.CourseID);
            ViewBag.StudentID = new SelectList(db.People, "ID", "LastName", enrollment.StudentID);
            return View(enrollment);
        }

        // POST: Enrollments/Edit/5
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "EnrollmentID,CourseID,StudentID,Grade")] Enrollment enrollment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(enrollment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CourseID = new SelectList(db.Courses, "CourseID", "Title", enrollment.CourseID);
            ViewBag.StudentID = new SelectList(db.People, "ID", "LastName", enrollment.StudentID);
            return View(enrollment);
        }

        // GET: Enrollments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Enrollment enrollment = db.Enrollments.Find(id);
            if (enrollment == null)
            {
                return HttpNotFound();
            }
            return View(enrollment);
        }

        // POST: Enrollments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Enrollment enrollment = db.Enrollments.Find(id);
            db.Enrollments.Remove(enrollment);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
