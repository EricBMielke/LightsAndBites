using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using LightsAndBites.Classes;

namespace LightsAndBites.Models
{
    public class Restaurant : Recommendation
    {
        [Key]
        public int Id { get; set; }
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Display(Name = "Category")]
        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public Category Category { get; set; }
        [Display(Name = "Longitude")]
        public double Longitude { get; set; }
        [Display(Name = "Latitude")]
        public double Latitude { get; set; }
        [Display(Name = "Likes")]
        public int Likes { get; set; }
        [Display(Name = "Dislikes")]
        public int Dislikes { get; set; }
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
        [Display(Name = "Photo")]
        public string CardPhoto { get; set; }
    }
}
