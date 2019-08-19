using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LightsAndBites.Models
{
    public class User
    {
        [Key]
        string Email { get; set; }
        string FirstName { get; set; }
        string LastName { get; set; }
        string Hometown { get; set; }

        [ForeignKey("BarId")]
        int BarCategoryIdOne { get; set; }
        [ForeignKey("BarId")]
        int BarCategoryIdTwo { get; set; }

        [ForeignKey("RestaurantId")]
        int RestaurantCategoryIdOne { get; set; }
        [ForeignKey("RestaurantId")]
        int RestaurantCategoryIdTwo { get; set; }
        [ForeignKey("RestaurantId")]
        int RestaurantCategoryIdThree { get; set; }

        [ForeignKey("EventId")]
        int EventCategoryIdOne { get; set; }
        [ForeignKey("EventId")]
        int EventCategoryIdTwo { get; set; }
        [ForeignKey("EventId")]
        int EventCategoryIdThree { get; set; }

        
    }
}
