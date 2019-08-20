using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace LightsAndBites.Models
{
    public class LightsAndBitesContext : DbContext
    {
        public LightsAndBitesContext (DbContextOptions<LightsAndBitesContext> options)
            : base(options)
        {
        }

        public DbSet<LightsAndBites.Models.Restaurant> Restaurant { get; set; }
    }
}
