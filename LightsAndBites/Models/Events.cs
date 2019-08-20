using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using LightsAndBites.Classes;

namespace LightsAndBites.Models
{
    public class Events : Recommendation
    {
        [Key]

        public int EventId { get; set; }
        [Display(Name = "Type")]
        public string Name { get; set; }
        [Display(Name = "Name")]
        public string Type { get; set; }
        [Display(Name = "Category")]
        [ForeignKey("Category")]
        public string CategoryId { get; set; }
        [Display(Name = "Longitude")]
        public double Longitude { get; set; }
        [Display(Name = "Latitude")]
        public double Latitude { get; set; }
        [Display(Name = "Comment")]
        [ForeignKey("CommentId")]
        public int CommentId { get; set; }
        [Display(Name = "City")]
        [ForeignKey("CityId")]
        public int CityId { get; set; }

        [Display(Name = "Website")]
        public string Website { get; set; }
        [Display(Name = "Street Address")]
        public string StreetAddress { get; set; }
        [Display(Name = "Zip code")]
        public int ZipCode { get; set; }
        



    }
}
