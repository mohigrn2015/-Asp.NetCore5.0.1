using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TestProjectIsDB.Models.Classes
{
    public class Category
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ID { get; set; }

        [Required, Display(Name = "Category")]
        public string Name { get; set; }

        //[ForeignKey("CategoryID")] -- You can reference foreign key here
        public virtual IList<Item> Items { get; set; }
    }
}
