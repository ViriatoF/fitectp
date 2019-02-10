using ContosoUniversity.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ContosoUniversity.Models
{
    public class FilePath
    {
        public int FilePathId { get; set; }
        [StringLength(255)]
        public string FileName { get; set; }
        public string FileType { get; set; }
        public int PersonID { get; set; }
        public virtual Person Person { get; set; }
    }
}