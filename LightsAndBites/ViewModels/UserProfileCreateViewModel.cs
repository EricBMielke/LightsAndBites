using LightsAndBites.Data;
using LightsAndBites.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LightsAndBites.ViewModels
{ 
       
    public class UserProfileCreateViewModel
    {
        private readonly ApplicationDbContext _context;

        public UserProfile UserProfile { get; set; }
        public Category Categories { get; set; }
        public List<SelectListItem> BarCategoryList { get; set; }


        public UserProfileCreateViewModel(ApplicationDbContext context)
        {
            _context = context;

            var BarCategoryList = _context.Categories.Where(b => b.CategoryName == "Bar").ToList();
            foreach (Category bar in BarCategoryList.ToList())
            {
                BarCategoryList.Add(bar);
            }
            
            var eventList = _context.Categories.Where(e => e.CategoryName == "Events");
            var restaurantList = _context.Categories.Where(r => r.CategoryName == "Restaurant");
        }







    }
}
