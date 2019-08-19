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
        public string UserComment { get; set; }
        public DateTime CommentDate { get; set; }

        [ForeignKey("BarId")]
        public int? BarId { get; set; }

        [ForeignKey("RestaurantId")]
        public int? RestaurantId { get; set; }

        [ForeignKey("EventId")]
        public int? EventId { get; set; }



    }
}
