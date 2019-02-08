using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ContosoUniversity.Models
{
    public enum Day
    {
        Monday, Tuesday, Wednesday, Thursday, Friday
    }
    
    public class Lesson
    {
        public int LessonId { get; set; }
        public int InstructorID { get; set; }

        [Range(7,20, ErrorMessage = "Please, rest between 7 and 20")]
        public int HourStart { get; set; }

        [Range(7, 20, ErrorMessage = "Please, rest between 7 and 20")]
        public int HourEnd { get; set; }
        public DateTime DateFirstCourse { get; set; }
        
        public virtual Instructor Instructor { get; set; }
        public virtual ICollection<Day> Days { get; set; }
        public virtual ICollection<Enrollment> Enrollments{ get; set; }

       
    }
}