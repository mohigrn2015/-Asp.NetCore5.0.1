using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TestProjectIsDB.Models.PlayerViewModel
{
    public class CreateGradeViewModel
    {
        public int GradeID { get; set; }
        [Required(ErrorMessage = "Grade Is Required")]
        public string GradeName { get; set; }
    }
}
