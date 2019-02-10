using ContosoUniversity.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ContosoUniversity.ViewModels
{
    public class StudentsApi
    {

        public int id { get; set; }
        public string lastname { get; set; }
        public string firstname { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime enrollmentdate { get; set; }
        
        public EnrollmentVM[] enrollments { get; set; }
    }
}