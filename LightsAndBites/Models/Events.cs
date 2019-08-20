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
        public string Type { get; set; }
        [ForeignKey("Category")]
        public string CategoryId { get; set; }
        public double Longitude { get; set; }

        public double Latitude { get; set; }
        [ForeignKey("CommentId")]
        public int CommentId { get; set; }
        [ForeignKey("CityId")]
        public int CityId { get; set; }


        public string Website { get; set; }
        public string StreetAddress { get; set; }
        public int ZipCode { get; set; }
        



    }
}
