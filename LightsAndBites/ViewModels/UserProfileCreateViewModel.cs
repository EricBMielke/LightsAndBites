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
        public SelectList BarCategoryList { get; set; }
        public SelectList RestaurantCategoryList { get; set; }
        public SelectList EventsCategoryList { get; set; }



        public UserProfileCreateViewModel(ApplicationDbContext context)
        {
            _context = context;

            List<Category> barCategoryList = _context.Categories.Where(b => b.CategoryName == "Bar").ToList();
            BarCategoryList = new SelectList(barCategoryList,"Id","CategoryType");

            List<Category> eventList = _context.Categories.Where(e => e.CategoryName == "Events").ToList();
            EventsCategoryList = new SelectList(eventList, "Id", "CategoryType");

            List<Category> restaurantList = _context.Categories.Where(r => r.CategoryName == "Restaurant").ToList();
            RestaurantCategoryList = new SelectList(restaurantList, "Id", "CategoryType");

        }
    }
}
