using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LightsAndBites.Models
{
    public class Comment
    {
        [Key]
        public int Id { get; set; }
        [Display(Name = "Comment")]
        public string UserComment { get; set; }
        [Display(Name = "Date")]
        public DateTime CommentDate { get; set; }

        [Display(Name = "Bar")]
        [ForeignKey("BarId")]
        public int? BarId { get; set; }
        [Display(Name = "Restaurant")]
        [ForeignKey("RestaurantId")]
        public int? RestaurantId { get; set; }
        [Display(Name = "Event")]
        [ForeignKey("EventId")]
        public int? EventId { get; set; }



    }
}
