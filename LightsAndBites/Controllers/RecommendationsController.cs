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
            UserProfile selectedUser = _context.UserProfile.Where(u => u.Id == userId).Single();

            List<Recommendation>[] passedValues = new List<Recommendation>[2];

            List<Category> restaurantCategories = GetRestaurantCategories(selectedUser);
            List<Category> barCategories = GetBarCategories(selectedUser);
            List<Category> eventCategories = GetEventCategories(selectedUser);

            List<Recommendation> recommendations = new List<Recommendation>();

            List<Bar> bars = GetBars(barCategories);
            List<Restaurant> restaurants = GetRestaurants(restaurantCategories);
            List<Events> events = GetEvents(eventCategories);

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

            passedValues[1] = GetNewGems();

            return View(passedValues);
        }

        private List<Bar> GetBars(List<Category> categories)
        {
            List<Bar> allBarsMatching = new List<Bar>();
            foreach (Category category in categories)
            {
                
                List<JObject> data = GetGoogleData(category.CategoryType, 43.0580569, -88.1075128, "bar");
                foreach (JObject j in data)
                {
                    Bar newBar = new Bar();
                    newBar.CategoryId = _context.Categories.Where(c => c.CategoryType == category.CategoryType).Where(c => c.CategoryName == category.CategoryName).Select(c => c.Id).Single();
                    newBar.Category = _context.Categories.Where(c => c.Id == newBar.CategoryId).Single();
                    newBar.Latitude = Convert.ToDouble(j["geometry"]["location"]["lat"]);
                    newBar.Longitude = Convert.ToDouble(j["geometry"]["location"]["lng"]);
                    newBar.Likes = 0;
                    newBar.Dislikes = 0;
                    newBar.Name = j["name"].ToString();
                    newBar.CityId = _context.Cities.Where(c => c.CityName == "Milwaukee").Select(c => c.Id).Single();
                    newBar.City = _context.Cities.Where(c => c.Id == newBar.CityId).Single();
                    try
                    {
                        newBar.CardPhoto = j["photos"][0]["photo_reference"].ToString();
                    }
                    catch
                    {
                        newBar.CardPhoto = null;
                    }

                    var foundMatchingBar = _context.Bars.Where(b => b.Latitude == newBar.Latitude).Where(b => b.Longitude == newBar.Longitude).Where(b => b.Name == newBar.Name).FirstOrDefault();
                    if (foundMatchingBar == null)
                    {
                        _context.Bars.Add(newBar);
                    }
                }
                _context.SaveChanges();
                List<Bar> allBarsMatchingSingle = _context.Bars.Where(b => b.Category.CategoryType == category.CategoryType).ToList();
                foreach (Bar bar in allBarsMatchingSingle)
                {
                    allBarsMatching.Add(bar);
                }
            }
            List<Bar> sortedBars = allBarsMatching.Where(b => (b.Likes != 0) || (b.Dislikes != 0)).OrderBy(b => (b.Likes / (b.Likes + b.Dislikes))).ToList();
            List<Bar> unrankedBars = allBarsMatching.Where(b => (b.Likes == 0) && (b.Dislikes == 0)).ToList();

            foreach(Bar b in unrankedBars)
            {
                sortedBars.Add(b);
            }

            return sortedBars;
        }

        private List<Restaurant> GetRestaurants(List<Category> categories)
        {
            List<Restaurant> allRestaurantsMatching = new List<Restaurant>();
            foreach (Category category in categories)
            {
                
                List<JObject> data = GetGoogleData(category.CategoryType, 43.0580569, -88.1075128, "restaurant");
                foreach (JObject j in data)
                {
                    Restaurant newRestaurant = new Restaurant();
                    newRestaurant.CategoryId = _context.Categories.Where(c => c.CategoryType == category.CategoryType).Where(c => c.CategoryName == category.CategoryName).Select(c => c.Id).Single();
                    newRestaurant.Category = _context.Categories.Where(c => c.Id == newRestaurant.CategoryId).Single();
                    newRestaurant.Latitude = Convert.ToDouble(j["geometry"]["location"]["lat"]);
                    newRestaurant.Longitude = Convert.ToDouble(j["geometry"]["location"]["lng"]);
                    newRestaurant.Likes = 0;
                    newRestaurant.Dislikes = 0;
                    newRestaurant.Name = j["name"].ToString();
                    newRestaurant.CityId = _context.Cities.Where(c => c.CityName == "Milwaukee").Select(c => c.Id).Single();
                    newRestaurant.City = _context.Cities.Where(c => c.Id == newRestaurant.CityId).Single();
                    try
                    {
                        newRestaurant.CardPhoto = j["photos"][0]["photo_reference"].ToString();
                    }
                    catch
                    {
                        newRestaurant.CardPhoto = null;
                    }

                    var foundMatchingRestaurant = _context.Restaurants.Where(r => r.Latitude == newRestaurant.Latitude).Where(r => r.Longitude == newRestaurant.Longitude).Where(r => r.Name == newRestaurant.Name).FirstOrDefault();
                    if (foundMatchingRestaurant == null)
                    {
                        _context.Restaurants.Add(newRestaurant);
                    }
                }
                _context.SaveChanges();
                List<Restaurant> allRestaurantsMatchingSingle = _context.Restaurants.Where(r => r.Category.CategoryType == category.CategoryType).ToList();
                foreach (Restaurant restaurant in allRestaurantsMatchingSingle)
                {
                    allRestaurantsMatching.Add(restaurant);
                }
            }

            List<Restaurant> sortedRestaurants = allRestaurantsMatching.Where(b => (b.Likes != 0) || (b.Dislikes != 0)).OrderBy(b => (b.Likes / (b.Likes + b.Dislikes))).ToList();
            List<Restaurant> unrankedRestaurants = allRestaurantsMatching.Where(b => (b.Likes == 0) && (b.Dislikes == 0)).ToList();

            foreach (Restaurant r in unrankedRestaurants)
            {
                sortedRestaurants.Add(r);
            }

            return sortedRestaurants;
        }

        private List<Events> GetEvents(List<Category> categories)
        {
            List<Events> allEventsMatching = new List<Events>();
            foreach (Category category in categories)
            {
                List<Events> allEventsMatchingSingle = _context.Events.Where(b => b.Category.CategoryName == category.CategoryName).ToList();
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

            List<Bar> bars = _context.Bars.Where(b => (b.Likes != 0) || (b.Dislikes != 0)).OrderBy(b => (b.Likes / (b.Likes + b.Dislikes))).ToList();
            List<Restaurant> restaurants = _context.Restaurants.Where(r => (r.Likes !=0) || (r.Dislikes != 0)).OrderBy(b => (b.Likes / (b.Likes + b.Dislikes))).ToList();

            List<Bar> unrankedBars = _context.Bars.Where(b => (b.Likes == 0) && (b.Dislikes == 0)).ToList();
            List<Restaurant> unrankedRestaurants = _context.Restaurants.Where(r => (r.Likes == 0) && (r.Dislikes == 0)).ToList();

            foreach (Bar b in unrankedBars)
            {
                bars.Add(b);
            }
            foreach (Restaurant r in unrankedRestaurants)
            {
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

        private List<JObject> GetGoogleData(string keyWord, double latitude, double longitude, string type)
        {
            string data = string.Empty;
            string url = @"https://maps.googleapis.com/maps/api/place/nearbysearch/json?key=" + ApiKey.mapsKey + @"&location=" + latitude + @"," + longitude + @"&keyword=" + keyWord + @"&type=" + type +"&radius=5000";

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

        public ActionResult PassBar(int id)
        {
            Recommendation bar = new Recommendation();
            bar = _context.Bars.Where(b => b.Id == id).FirstOrDefault();
            return PartialView("_CardDetails", bar);

        }
        public ActionResult PassRestaurant(int id)
        {
            Recommendation restaurant = new Recommendation();
            restaurant = _context.Restaurants.Where(r => r.Id == id).FirstOrDefault();
            return PartialView("_CardDetails", restaurant);
        }
        public ActionResult PassEvents(int id)
        {
            Recommendation events = new Recommendation();
            events = _context.Events.Where(e => e.EventId == id).FirstOrDefault();
            return PartialView("_CardDetails", events);
        }

       
    }
}