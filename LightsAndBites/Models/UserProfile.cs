using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LightsAndBites.Models
{
    public class UserProfile
    {
        [Key]
        public int Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Hometown { get; set; }

        [ForeignKey("BarId")]
        public int BarCategoryIdOne { get; set; }
        [ForeignKey("BarId")]
        public int BarCategoryIdTwo { get; set; }

        [ForeignKey("RestaurantId")]
        public int RestaurantCategoryIdOne { get; set; }
        [ForeignKey("RestaurantId")]
        public int RestaurantCategoryIdTwo { get; set; }
        [ForeignKey("RestaurantId")]
        public int RestaurantCategoryIdThree { get; set; }

        [ForeignKey("EventId")]
        public int EventCategoryIdOne { get; set; }
        [ForeignKey("EventId")]
        public int EventCategoryIdTwo { get; set; }
        [ForeignKey("EventId")]
        public int EventCategoryIdThree { get; set; }

        
    }
}
