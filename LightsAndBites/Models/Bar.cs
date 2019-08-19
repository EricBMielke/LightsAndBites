using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LightsAndBites.Models
{
    public class Bar
    {
        [Key]
        public int Id { get; set; }
        public string type { get; set; }
        [ForeignKey("Category")]
        public string category { get; set; }
        public double longitude { get; set; }
        public double latitude { get; set; }
        public int likes { get; set; }
        public int dislikes { get; set; }
        [ForeignKey("CommentId")]
        public int? commentId { get; set; }
        [ForeignKey("CityId")]
        public int cityId { get; set; }
        public string website { get; set; }
    }
}
