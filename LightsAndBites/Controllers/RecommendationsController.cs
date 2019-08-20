using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using LightsAndBites.Models;
using LightsAndBites.Classes;
using LightsAndBites.Data;
using System.Net;
using System.IO;
using Newtonsoft.Json.Linq;

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

            List<Category> restaurantCategories = GetRestaurantCategories(selectedUser);
            List<Category> barCategories = GetBarCategories(selectedUser);
            List<Category> eventCategories = GetEventCategories(selectedUser);

            List<Recommendation> recommendations = new List<Recommendation>();

            List<Bar> bars = GetBars(barCategories);
            List<Restaurant> restaurants = GetRestaurants(restaurantCategories);
            List<Events> events = GetEvents(eventCategories);

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

        private List<Category> GetBarCategories(UserProfile user)
        {
            List<Category> barCategories = new List<Category>();
            barCategories.Add(_context.Categories.Where(c => c.Id == user.BarCategoryIdOne).Single());
            barCategories.Add(_context.Categories.Where(c => c.Id == user.BarCategoryIdTwo).Single());
            return barCategories;
        }

        private List<Category> GetRestaurantCategories(UserProfile user)
        {
            List<Category> restaurantCategories = new List<Category>();
            restaurantCategories.Add(_context.Categories.Where(c => c.Id == user.RestaurantCategoryIdOne).Single());
            restaurantCategories.Add(_context.Categories.Where(c => c.Id == user.RestaurantCategoryIdTwo).Single());
            restaurantCategories.Add(_context.Categories.Where(c => c.Id == user.RestaurantCategoryIdThree).Single());

            return restaurantCategories;
        }

        private List<Category> GetEventCategories(UserProfile user)
        {
            List<Category> eventCategories = new List<Category>();
            eventCategories.Add(_context.Categories.Where(c => c.Id == user.EventCategoryIdOne).Single());
            eventCategories.Add(_context.Categories.Where(c => c.Id == user.EventCategoryIdTwo).Single());
            eventCategories.Add(_context.Categories.Where(c => c.Id == user.EventCategoryIdThree).Single());

            return eventCategories;
        }

        private List<JObject> GetGoogleData(string key, string keyWord, double latitude, double longitude)
        {
            string data = string.Empty;
            string url = @"https://maps.googleapis.com/maps/api/place/nearbysearch/json?key=" + key + @"&location=" + latitude + @"," + longitude + @"&keyword=" + keyWord + @"&type=bar&radius=5000";

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.AutomaticDecompression = DecompressionMethods.GZip;

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                data = reader.ReadToEnd();
            }

            JObject returnData = JObject.Parse(data);
            List<JObject> returnList = new List<JObject>();

            foreach(JObject j in returnData["results"])
            {
                returnList.Add(j);
            }
            return returnList;
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