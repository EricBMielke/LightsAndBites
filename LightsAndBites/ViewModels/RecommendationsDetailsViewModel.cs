using LightsAndBites.Data;
using LightsAndBites.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LightsAndBites.ViewModels
{
    public class RecommendationsDetailsViewModel
    {
        private readonly ApplicationDbContext _context;

        public Bar Bars { get; set; }
        public Restaurant Restaurants { get; set; }
        public Events Events { get; set; }
               
    }
}
