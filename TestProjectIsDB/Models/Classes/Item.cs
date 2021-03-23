using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TestProjectIsDB.Models.Classes
{
    public class Item
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ID { get; set; }

        [Required, Display(Name = "Product Name")]
        public string Name { get; set; }

        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        //[Column(TypeName = "image")]
        public string Image { get; set; }

        //[Required, Today, DataType(DataType.Date), Column(TypeName = "date"), Display(Name = "Entry Date")]
        public DateTime EntryDate { get; set; }

        [Required]
        public long Quantity { get; set; }

        [ForeignKey("Category")]
        public long CategoryID { get; set; }

        //[ForeignKey("CategoryID")] -- You can reference foreign key here
        public virtual Category Category { get; set; }
        [NotMapped]
        public IFormFile ImageFile { get; set; }
    }
}
