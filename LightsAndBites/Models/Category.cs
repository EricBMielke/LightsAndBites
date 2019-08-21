using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LightsAndBites.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [Display(Name = "Category name")]
        public string CategoryName { get; set; }
        [Display(Name = "Category type")]
        public string CategoryType { get; set; }
    }
}
