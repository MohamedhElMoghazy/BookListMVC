using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BookListMVC.Models
{
    public class Book
    {
        [Key] // data annotation to set the key 
        public int Id { get; set; }
        [Required] // data annotation to mark the name filed is required
        public String Name { get; set; }

        public String Author { get; set; }
        public String ISBN { get; set; }



    }
}
