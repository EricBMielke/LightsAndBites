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

            List<Recommendation>[] passedValues = new List<Recommendation>[2];

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

            passedValues[0] = recommendations;

            passedValues[1] = GetNewGems();

            return View(passedValues);
        }

        private List<Bar> GetBars(List<Category> categories)
        {
            List<Bar> allBarsMatching = new List<Bar>();
            foreach (Category category in categories)
            {
                List<Bar> allBarsMatchingSingle = _context.Bars.Where(b => b.Category == category.CategoryName).ToList();
                foreach (Bar bar in allBarsMatchingSingle)
                {
                    allBarsMatching.Add(bar);
                }
            }

            return allBarsMatching;
        }

        private List<Restaurant> GetRestaurants(List<Category> categories)
        {
            List<Restaurant> allRestaurantsMatching = new List<Restaurant>();
            foreach (Category category in categories)
            {
                List<Restaurant> allRestaurantsMatchingSingle = _context.Restaurants.Where(b => b.Category == category.CategoryName).ToList();
                foreach (Restaurant restaurant in allRestaurantsMatchingSingle)
                {
                    allRestaurantsMatching.Add(restaurant);
                }
            }

            return allRestaurantsMatching;
        }

        private List<Events> GetEvents(List<Category> categories)
        {
            List<Events> allEventsMatching = new List<Events>();
            foreach (Category category in categories)
            {
                List<Events> allEventsMatchingSingle = _context.Events.Where(b => b.CategoryId == category.CategoryName).ToList();
                foreach (Events eventItem in allEventsMatchingSingle)
                {
                    allEventsMatching.Add(eventItem);
                }
            }

            return allEventsMatching;
        }

        private List<Recommendation> GetNewGems()
        {
            List<Recommendation> gems = new List<Recommendation>();

            List<Bar> bars = _context.Bars.OrderBy(b => (b.Likes / (b.Likes + b.Dislikes))).ToList();
            List<Restaurant> restaurants = _context.Restaurants.OrderBy(b => (b.Likes / (b.Likes + b.Dislikes))).ToList();

            if (bars.Count == 0)
            {
                for (int i = 0; i < 4; i++)
                {
                    gems.Add(restaurants[i]);
                }
            }
            else if (restaurants.Count == 0)
            {
                for (int i = 0; i < 4; i++)
                {
                    gems.Add(bars[i]);
                }
            }
            else
            {
                for (int i = 0; i < 2; i++)
                {
                    gems.Add(bars[i]);
                }
                for (int i = 0; i < 2; i++)
                {
                    gems.Add(restaurants[i]);
                }
            }
            return gems;
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
    }
}