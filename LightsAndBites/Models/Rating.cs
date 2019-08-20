using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LightsAndBites.Models
{
    public class Rating
    {
        [Key]
        public int Id { get; set; }
        public bool IsPositive { get; set; }
        [Display(Name = "User email")]
        [ForeignKey("UserId")]
        public string UserEmail { get; set; }

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
