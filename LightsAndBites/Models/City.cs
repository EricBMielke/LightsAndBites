using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LightsAndBites.Models
{
    public class City
    {
        [Key]
        public int Id { get; set; }
        [Display(Name = "City name")]
        public string CityName { get; set; }
        [Display(Name = "State")]
        public string State { get; set; }

    }
}
