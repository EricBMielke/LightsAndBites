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
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Display(Name = "First name")]
        public string FirstName { get; set; }
        [Display(Name = "Last name")]
        public string LastName { get; set; }
        [Display(Name = "Hometown")]
        public string Hometown { get; set; }
        [Display(Name = "Bar preference 1")]
        [ForeignKey("BarId")]
        public int BarCategoryIdOne { get; set; }
        [Display(Name = "Bar preference 2")]
        [ForeignKey("BarId")]
        public int BarCategoryIdTwo { get; set; }
        [Display(Name = "Restaurant preference 1")]
        [ForeignKey("RestaurantId")]
        public int RestaurantCategoryIdOne { get; set; }
        [Display(Name = "Restaurant preference 2")]
        [ForeignKey("RestaurantId")]
        public int RestaurantCategoryIdTwo { get; set; }
        [Display(Name = "Restaurant preference 3")]
        [ForeignKey("RestaurantId")]
        public int RestaurantCategoryIdThree { get; set; }
        [Display(Name = "Event preference 1")]
        [ForeignKey("EventId")]
        public int EventCategoryIdOne { get; set; }
        [Display(Name = "Event preference 2")]
        [ForeignKey("EventId")]
        public int EventCategoryIdTwo { get; set; }
        [Display(Name = "Event preference 3")]
        [ForeignKey("EventId")]
        public int EventCategoryIdThree { get; set; }

        
    }
}
