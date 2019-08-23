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
using LightsAndBites.ApiKeys;

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
 //           GetDailyQuote();
            UserProfile selectedUser = _context.UserProfile.Where(u => u.Id == userId).Single();

            List<Recommendation>[] passedValues = new List<Recommendation>[2];

            List<Category> restaurantCategories = GetRestaurantCategories(selectedUser);
            List<Category> barCategories = GetBarCategories(selectedUser);
            List<Category> eventCategories = GetEventCategories(selectedUser);

            List<Recommendation> recommendations = new List<Recommendation>();

            List<Bar> bars = GetBars(barCategories, userId);
            List<Restaurant> restaurants = GetRestaurants(restaurantCategories, userId);
            List<Events> events = GetEvents(eventCategories, selectedUser.Id);

            if (bars.Count <= 2)
            {
                foreach (Bar b in bars)
                {
                    recommendations.Add(b);
                }
            }
            else
            {
                for (int i = 0; i < 2; i++)
                {
                    recommendations.Add(bars[i]);
                }
            }

            if (restaurants.Count <= 2)
            {
                foreach (Restaurant r in restaurants)
                {
                    recommendations.Add(r);
                }
            }
            else
            {
                for (int i = 0; i < 2; i++)
                {
                    recommendations.Add(restaurants[i]);
                }
            }

            if (events.Count <= 2)
            {
                foreach (Events e in events)
                {
                    recommendations.Add(e);
                }
            }
            else
            {
                for (int i = 0; i < 2; i++)
                {
                    recommendations.Add(events[i]);
                }
            }

            passedValues[0] = recommendations;

            passedValues[1] = GetNewGems(userId);

            return View(passedValues);
        }

        private List<Bar> GetBars(List<Category> categories, int Id)
        {
            List<Bar> allBarsMatching = new List<Bar>();
            foreach (Category category in categories)
            {
                List<Bar> allBarsMatchingSingle = _context.Bars.Where(b => b.Category.CategoryType == category.CategoryType).ToList();
                foreach (Bar bar in allBarsMatchingSingle)
                {
                    allBarsMatching.Add(bar);
                }
            }
            UserProfile selectedUserCity = _context.UserProfile.Where(u => u.Id == Id).Single();
            Bar linkBar = new Bar();
            linkBar.CityId = _context.Cities.Where(c => c.CityName == selectedUserCity.Hometown).Select(c => c.Id).Single();
            List<Bar> sortedBars = allBarsMatching.Where(b => (b.Likes != 0) || (b.Dislikes != 0)).OrderBy(b => (b.Likes / (b.Likes + b.Dislikes))).ToList();
            List<Bar> unrankedBars = allBarsMatching.Where(b => (b.CityId == linkBar.CityId) && (b.Likes == 0) && (b.Dislikes == 0)).ToList();

            foreach(Bar b in unrankedBars)
            {
                sortedBars.Add(b);
            }

            return sortedBars;
        }

        private List<Restaurant> GetRestaurants(List<Category> categories, int Id)
        {
            List<Restaurant> allRestaurantsMatching = new List<Restaurant>();
            foreach (Category category in categories)
            {
                List<Restaurant> allRestaurantsMatchingSingle = _context.Restaurants.Where(r => r.Category.CategoryType == category.CategoryType).ToList();
                foreach (Restaurant restaurant in allRestaurantsMatchingSingle)
                {
                    allRestaurantsMatching.Add(restaurant);
                }
            }
            UserProfile selectedUserCity = _context.UserProfile.Where(u => u.Id == Id).Single();
            Restaurant linkRestaurant = new Restaurant();
            linkRestaurant.CityId = _context.Cities.Where(c => c.CityName == selectedUserCity.Hometown).Select(c => c.Id).Single();
            List<Restaurant> sortedRestaurants = allRestaurantsMatching.Where(b => (b.Likes != 0) || (b.Dislikes != 0)).OrderBy(b => (b.Likes / (b.Likes + b.Dislikes))).ToList();
            List<Restaurant> unrankedRestaurants = allRestaurantsMatching.Where(b => (b.CityId == linkRestaurant.CityId) && (b.Likes == 0) && (b.Dislikes == 0)).ToList();

            foreach (Restaurant r in unrankedRestaurants)
            {
                sortedRestaurants.Add(r);
            }

            return sortedRestaurants;
        }

        private List<Events> GetEvents(List<Category> categories, int userId)
        {
            UserProfile foundUser = _context.UserProfile.Where(u => u.Id == userId).Single();
            List<Events> allEventsMatching = new List<Events>();
            foreach (Category category in categories)
            {
                List<Events> allEventsMatchingSingle = _context.Events.Where(b => b.Category.CategoryName == category.CategoryName).Where(e => e.City.CityName == foundUser.Hometown).ToList();
                foreach (Events eventItem in allEventsMatchingSingle)
                {
                    allEventsMatching.Add(eventItem);
                }
            }

            return allEventsMatching;
        }

        private List<Recommendation> GetNewGems(int Id)
        {
            List<Recommendation> gems = new List<Recommendation>();
            UserProfile selectedUserCity = _context.UserProfile.Where(u => u.Id == Id).Single();

            List<Bar> bars = _context.Bars.Where(b => (b.Likes != 0) || (b.Dislikes != 0)).OrderBy(b => (b.Likes / (b.Likes + b.Dislikes))).ToList();
            List<Restaurant> restaurants = _context.Restaurants.Where(r => (r.Likes !=0) || (r.Dislikes != 0)).OrderBy(b => (b.Likes / (b.Likes + b.Dislikes))).ToList();
            Restaurant linkedRestaurant = new Restaurant();
            linkedRestaurant.CityId = _context.Cities.Where(c => c.CityName == selectedUserCity.Hometown).Select(c => c.Id).Single();
            Bar linkedBar = new Bar();
            linkedBar.CityId = _context.Cities.Where(c => c.CityName == selectedUserCity.Hometown).Select(c => c.Id).Single();
            List<Bar> unrankedBars = _context.Bars.Where(b => (b.CityId == linkedBar.CityId)&&(b.Likes == 0) && (b.Dislikes == 0)).ToList();
            List<Restaurant> unrankedRestaurants = _context.Restaurants.Where(r => (r.CityId == linkedRestaurant.CityId) && (r.Likes == 0) && (r.Dislikes == 0)).ToList();

            foreach (Bar b in unrankedBars)
            {
                b.Category = _context.Categories.Where(c => c.Id == b.CategoryId).Single();
                bars.Add(b);
            }
            foreach (Restaurant r in unrankedRestaurants)
            {
                r.Category = _context.Categories.Where(c => c.Id == r.CategoryId).Single();
                restaurants.Add(r);
            }

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
        public static string GetDailyQuote()
        {
            //IF USING THIS FUNCTION = WE MUST ADD CREDIT TO QUOTES API like it states in https://theysaidso.com/api/ 
            string inspirationalQuoteOfDay = string.Empty;
            string html = string.Empty;
            string url = @"http://quotes.rest/qod.json?category=inspire";

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.AutomaticDecompression = DecompressionMethods.GZip;

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                html = reader.ReadToEnd();
            }

            JObject o = JObject.Parse(html);

            foreach (JObject j in o["contents"]["quotes"])
            {
                inspirationalQuoteOfDay = (j["quote"]).ToString();
            }

            return inspirationalQuoteOfDay;
        }

        public ActionResult PassBar(int id)
        {
            Bar bar = new Bar();
            bar = _context.Bars.Where(b => b.Id == id).FirstOrDefault();
            return View();
        }
    }
}