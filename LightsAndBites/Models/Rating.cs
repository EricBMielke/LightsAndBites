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
        int RatingId { get; set; }
        bool IsPositive { get; set; }
        [ForeignKey("UserId")]
        string UserEmail { get; set; }

        [ForeignKey("BarId")]
        int? BarId { get; set; }

        [ForeignKey("RestaurantId")]
        int? RestaurantId { get; set; }

        [ForeignKey("EventId")]
        int? EventId { get; set; }
    }
}
