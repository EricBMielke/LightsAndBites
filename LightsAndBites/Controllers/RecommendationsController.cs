using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using LightsAndBites.Models;
using LightsAndBites.Classes;
using LightsAndBites.Data;

namespace LightsAndBites.Controllers
{
    public class RecommendationsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RecommendationsController(ApplicationDbContext context)
        {
            _context = context;
        }
        // GET: Recommendations
        public ActionResult Index(int userId)
        {
            UserProfile selectedUser = _context.UserProfile.Where(u => u.Id == userId).Single();

            List<Category> barCategories = new List<Category>();
            barCategories.Add(_context.Categories.Where(c => c.Id == selectedUser.BarCategoryIdOne).Single());
            barCategories.Add(_context.Categories.Where(c => c.Id == selectedUser.BarCategoryIdTwo).Single());

            List<Category> eventCategories = new List<Category>();
            eventCategories.Add(_context.Categories.Where(c => c.Id == selectedUser.EventCategoryIdOne).Single());
            eventCategories.Add(_context.Categories.Where(c => c.Id == selectedUser.EventCategoryIdTwo).Single());
            eventCategories.Add(_context.Categories.Where(c => c.Id == selectedUser.EventCategoryIdThree).Single());

            List<Category> restaurantCategories = new List<Category>();
            restaurantCategories.Add(_context.Categories.Where(c => c.Id == selectedUser.RestaurantCategoryIdOne).Single());
            restaurantCategories.Add(_context.Categories.Where(c => c.Id == selectedUser.RestaurantCategoryIdTwo).Single());
            restaurantCategories.Add(_context.Categories.Where(c => c.Id == selectedUser.RestaurantCategoryIdThree).Single());

            List<Recommendation> recommendations = new List<Recommendation>();
            List<Bar> bars = new List<Bar>();
            List<Restaurant> restaurants = new List<Restaurant>();
            List<Events> events = new List<Events>();
            foreach (Bar b in bars)
            {
                recommendations.Add(b);
            }
            foreach (Restaurant r in restaurants)
            {
                recommendations.Add(r);
            }
            foreach (Events e in events)
            {
                recommendations.Add(e);
            }
            return View();
        }

        private List<Bar> GetBars(List<Category> categories)
        {

        }

        private List<Restaurant> GetRestaurants(List<Category> categories)
        {

        }

        private List<Events> GetEvents(List<Category> categories)
        {

        }

        private List<Recommendation> GetNewGems()
        {

        }

        // GET: Recommendations/Details/5
        //public ActionResult Details(int id)
        //{
        //    return View();
        //}

        //// GET: Recommendations/Create
        //public ActionResult Create()
        //{
        //    return View();
        //}

        //// POST: Recommendations/Create
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create(IFormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add insert logic here

        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        //// GET: Recommendations/Edit/5
        //public ActionResult Edit(int id)
        //{
        //    return View();
        //}

        //// POST: Recommendations/Edit/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit(int id, IFormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add update logic here

        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        //// GET: Recommendations/Delete/5
        //public ActionResult Delete(int id)
        //{
        //    return View();
        //}

        //// POST: Recommendations/Delete/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Delete(int id, IFormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add delete logic here

        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}
    }
}