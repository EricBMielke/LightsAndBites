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
        public int Id;
        public string type;
        [ForeignKey("Category")]
        public string category;
        public double longitude;
        public double latitude;
        public int likes;
        public int dislikes;
        [ForeignKey("CommentId")]
        public int? commentId;
        [ForeignKey("CityId")]
        public int cityId;
        public string website;
    }
}
