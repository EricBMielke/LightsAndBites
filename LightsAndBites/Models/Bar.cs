using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using LightsAndBites.Classes;

namespace LightsAndBites.Models
{
    public class Bar : Recommendation
    {
        [Key]
        public int Id { get; set; }
        [Display(Name = "Type")]
        public string Type { get; set; }
        [Display(Name = "Category")]
        [ForeignKey("Category")]
        public string Category { get; set; }
        [Display(Name = "Longitude")]
        public double Longitude { get; set; }
        [Display(Name = "Latitude")]
        public double Latitude { get; set; }
        [Display(Name = "Likes")]
        public int Likes { get; set; }
        [Display(Name = "Dislikes")]
        public int Dislikes { get; set; }
        [Display(Name = "Comment Id")]
        [ForeignKey("CommentId")]
        public int? CommentId { get; set; }
        [Display(Name = "City")]
        [ForeignKey("CityId")]
        public int CityId { get; set; }
        [Display(Name = "Website")]
        public string Website { get; set; }
        [Display(Name = "Photo")]
        public string CardPhoto { get; set; }
    }
}
