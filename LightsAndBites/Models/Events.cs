using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LightsAndBites.Models
{
    public class Events
    {
        [Key]
<<<<<<< HEAD
        public int EventId { get; set; }
        public string Type { get; set; }
        [ForeignKey("Category")]
        public string CategoryId { get; set; }
        public double Longitude { get;set }
=======
        public int Id { get; set; }
        public string Type { get; set; }
        [ForeignKey("Category")]
        public string Category { get; set; }
        public double Longitude { get; set; }
>>>>>>> 284377327c044ab1e543c1b008669aae40ccf914
        public double Latitude { get; set; }
        [ForeignKey("CommentId")]
        public int CommentId { get; set; }
        [ForeignKey("CityId")]
        public int CityId { get; set; }
<<<<<<< HEAD

        public string Website { get; set; }
        public string StreetAddress { get; set; }
        public int Zipcode { get; set; }
        
=======
        public string Website { get; set; }
>>>>>>> 284377327c044ab1e543c1b008669aae40ccf914



    }
}
