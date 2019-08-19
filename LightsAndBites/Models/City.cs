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
        int CityId { get; set; }
        string CityName { get; set; }
        string State { get; set; }

    }
}
