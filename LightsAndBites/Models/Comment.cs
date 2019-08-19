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
        int CommentId { get; set; }
        string UserComment { get; set; }
        DateTime CommentDate { get; set; }

        [ForeignKey("BarId")]
        int? BarId { get; set; }

        [ForeignKey("RestaurantId")]
        int? RestaurantId { get; set; }

        [ForeignKey("EventId")]
        int? EventId { get; set; }



    }
}
