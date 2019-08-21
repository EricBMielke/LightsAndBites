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
        public int CategoryId { get; set; }
        [Display(Name = "Category")]
        [ForeignKey("CategoryId")]
        public Category Category { get; set; }
        [Display(Name = "Longitude")]
        public double Longitude { get; set; }
        [Display(Name = "Latitude")]
        public double Latitude { get; set; }
        [Display(Name = "Comment Id")]
        public int? CommentId { get; set; }
        [ForeignKey("CommentId")]
        public Comment Comment { get; set; }
        [Display(Name = "City")]
        public int CityId { get; set; }
        [ForeignKey("CityId")]
        public City City { get; set; }

        [Display(Name = "Website")]
        public string Website { get; set; }
        [Display(Name = "Street Address")]
        public string StreetAddress { get; set; }
        [Display(Name = "Zip code")]
        public int ZipCode { get; set; }
        



    }
}
