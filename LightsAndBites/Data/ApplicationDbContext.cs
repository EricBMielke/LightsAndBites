using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using LightsAndBites.Models;

namespace LightsAndBites.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }

      
        public DbSet<LightsAndBites.Models.UserProfile> UserProfile { get; set; }
        public DbSet<LightsAndBites.Models.Restaurant> Restaurants { get; set; }
        public DbSet<LightsAndBites.Models.Rating> Rating { get; set; }
        public DbSet<LightsAndBites.Models.Events> Events { get; set; }
        public DbSet<LightsAndBites.Models.Comment> Comments { get; set; }
        public DbSet<LightsAndBites.Models.City> Cities { get; set; }
        public DbSet<LightsAndBites.Models.Category> Categories { get; set; }
        public DbSet<LightsAndBites.Models.Bar> Bars { get; set; }

        public DbSet<LightsAndBites.Models.ApplicationUser> ApplicationUsers { get; set; }

        public DbSet<LightsAndBites.Models.ApprovedSuperUser> ApprovedSuperUsers { get; set; }
    }
}
