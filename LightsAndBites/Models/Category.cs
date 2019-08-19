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
        int CategoryId { get; set; }
        string CategoryName { get; set; }
        string CateogryType { get; set; }
    }
}
