using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestProjectIsDB.Models.Classes
{
    public class Grade
    {
        public int GradeID { get; set; }
        public string GradeName { get; set; }
        public Nullable<int> Status { get; set; }

        public virtual ICollection<Player> Players { get; set; }
    }
}
