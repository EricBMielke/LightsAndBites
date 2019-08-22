using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace LightsAndBites.Models
{
    public class ApprovedSuperUser
    {
        [Key]
        public int Id { get; set; }

        public string Email { get; set; }
    }
}
