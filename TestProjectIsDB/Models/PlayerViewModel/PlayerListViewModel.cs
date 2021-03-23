using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TestProjectIsDB.Models.Classes;

namespace TestProjectIsDB.Models.PlayerViewModel
{
    public class PlayerListViewModel
    {
        public int PlayerID { get; set; }
        [Required(ErrorMessage = "Name Is Required")]
        public string Name { get; set; }
        public DateTime DoB { get; set; }
        [Required(ErrorMessage = "Team Is Required")]
        public string Team { get; set; }
        [Required(ErrorMessage = "Email Is Required")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Phone Is Required")]
        public string Phone { get; set; }
        public decimal Salary { get; set; }
        public string ImageName { get; set; }
        public string ImageUrl { get; set; }
        public int GradeID { get; set; }
        public string GradeName { get; set; }
        public virtual Grade Grade { get; set; }
    }
}
