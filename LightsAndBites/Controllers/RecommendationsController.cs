﻿using System;
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
        private readonly Object thisLock = new Object();

        public RecommendationsController(ApplicationDbContext context)
        {
            _context = context;
        }
        public ActionResult AddComment(string type, int locationId, string commentData)
        {
            if (type.ToLower() == "bar")
            {
                Bar thisBar;
                Comment comment = new Comment();
                lock (thisLock)
                {
                    thisBar = _context.Bars.Where(b => b.Id == locationId).Single();
                }
                comment.BarId = locationId;
                comment.UserEmail = User.Identity.Name;
                comment.UserComment = commentData;
                comment.CommentDate = DateTime.Now;
                lock (thisLock)
                {
                    _context.Comments.Add(comment);
                    _context.SaveChanges();
                }
                return RedirectToAction("Details", "BarsView", new { id = locationId });
            }
            else if (type.ToLower() == "restaurant")
            {
                Restaurant thisRestaurant;
                Comment comment = new Comment();
                lock (thisLock)
                {
                    thisRestaurant = _context.Restaurants.Where(r => r.Id == locationId).Single();
                }
                comment.RestaurantId = locationId;
                comment.UserEmail = User.Identity.Name;
                comment.UserComment = commentData;
                comment.CommentDate = DateTime.Now;
                lock (thisLock)
                {
                    _context.Comments.Add(comment);
                    _context.SaveChanges();
                }
                return RedirectToAction("Details", "RestaurantsView", new { id = locationId });
            }
            return View();
        }
        public ActionResult AddLike(string type, int locationId, bool isPositive)
        {
            if (type.ToLower() == "bar")
            {
                Bar thisBar;
                var userRating = _context.Rating.Where(r => r.UserEmail == User.Identity.Name).Where(r => r.BarId == locationId).SingleOrDefault();
                if (userRating != null)
                {
                    userRating.IsPositive = isPositive;
                    _context.SaveChanges();
                }
                else
                {
                    Rating rating = new Rating();
                    lock (thisLock)
                    {
                        thisBar = _context.Bars.Where(b => b.Id == locationId).Single();
                    }
                    rating.IsPositive = isPositive;
                    if (rating.IsPositive == true)
                    {
                        thisBar.Likes += 1;
                    }
                    else
                    {
                        thisBar.Dislikes += 1;
                    }
                    rating.BarId = locationId;
                    rating.UserEmail = User.Identity.Name;
                    lock (thisLock)
                    {
                        _context.Rating.Add(rating);
                        _context.SaveChanges();
                    }
                }
                return RedirectToAction("Details", "BarsView", new {id = locationId });
            }
            else if (type.ToLower() == "restaurant")
            {
                Restaurant thisRestaurant;
                var userRating = _context.Rating.Where(r => r.RestaurantId == locationId).Where(r => r.UserEmail == User.Identity.Name).SingleOrDefault();
                if (userRating != null)
                {
                    userRating.IsPositive = isPositive;
                    _context.SaveChanges();
                }
                else
                {
                    Rating rating = new Rating();
                    lock (thisLock)
                    {
                        thisRestaurant = _context.Restaurants.Where(r => r.Id == locationId).Single();
                    }
                    rating.IsPositive = isPositive;
                    if (rating.IsPositive == true)
                    {
                        thisRestaurant.Likes += 1;
                    }
                    else
                    {
                        thisRestaurant.Dislikes += 1;
                    }
                    rating.RestaurantId = locationId;
                    rating.UserEmail = User.Identity.Name;
                    lock (thisLock)
                    {
                        _context.Rating.Add(rating);
                        _context.SaveChanges();
                    }
                }
                return RedirectToAction("Details", "RestaurantsView", new { id = locationId });
            }
            return View();
        }
        // GET: Recommendations
        public async Task<ActionResult> Index()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            var currentUser = User.Identity.Name;

            UserProfile selectedUser;
            //GetDailyQuote();
            lock (thisLock)
            {
                selectedUser = _context.UserProfile.Where(u => u.Email == User.Identity.Name).Single();

            }

            List<Recommendation>[] passedValues = new List<Recommendation>[2];

            Task<List<Category>> restaurantCategoriesTask = GetRestaurantCategories(selectedUser);
            Task<List<Category>> barCategoriesTask = GetBarCategories(selectedUser);
            Task<List<Category>> eventCategoriesTask = GetEventCategories(selectedUser);
            Task<List<Recommendation>> newGemsTask = GetNewGems(selectedUser.Id);

            List<Category> restaurantCategories = await restaurantCategoriesTask;
            List<Category> barCategories = await barCategoriesTask;
            List<Category> eventCategories = await eventCategoriesTask;

            List<Recommendation> recommendations = new List<Recommendation>();

            Task<List<Bar>> barsTask = GetBars(barCategories, selectedUser.Id);
            Task<List<Restaurant>> restaurantsTask = GetRestaurants(restaurantCategories, selectedUser.Id);
            Task<List<Events>> eventsTask = GetEvents(eventCategories, selectedUser.Id);

            List<Bar> bars = await barsTask;
            List<Restaurant> restaurants = await restaurantsTask;
            List<Events> events = await eventsTask;

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

            passedValues[1] = await newGemsTask;

            return View(passedValues);
        }

        private Task<List<Bar>> GetBars(List<Category> categories, int Id)
        {
            return Task.Run(() =>
            {
                List<Rating> userRatings;
                lock(thisLock)
                {
                    userRatings = _context.Rating.Where(r => r.BarId != null).Where(r => r.UserEmail == User.Identity.Name).ToList();
                }
                List<Bar> allBarsMatching = new List<Bar>();
                foreach (Category category in categories)
                {
                    List<Bar> allBarsMatchingSingle;
                    lock (thisLock)
                    {
                        allBarsMatchingSingle = _context.Bars.Where(b => b.Category.CategoryType == category.CategoryType).ToList();
                    }
                    foreach (Bar bar in allBarsMatchingSingle)
                    {
                        bool foundMatchingRating = false;
                        foreach(Rating r in userRatings)
                        {
                            if (r.BarId == bar.Id)
                            {
                                foundMatchingRating = true;
                                break;
                            }
                        }
                        if (foundMatchingRating == false)
                        {
                            allBarsMatching.Add(bar);
                        }
                    }
                }
                UserProfile selectedUserCity;
                lock (thisLock)
                {
                    selectedUserCity = _context.UserProfile.Where(u => u.Id == Id).Single();
                }
                Bar linkBar = new Bar();
                lock (thisLock)
                {
                    linkBar.CityId = _context.Cities.Where(c => c.CityName == selectedUserCity.Hometown).Select(c => c.Id).Single();
                }
                List<Bar> sortedBars = allBarsMatching.Where(b => (b.Likes != 0) || (b.Dislikes != 0)).OrderBy(b => (b.Likes / (b.Likes + b.Dislikes))).ToList();
                List<Bar> unrankedBars = allBarsMatching.Where(b => (b.CityId == linkBar.CityId) && (b.Likes == 0) && (b.Dislikes == 0)).ToList();

                foreach (Bar b in unrankedBars)
                {
                    sortedBars.Add(b);
                }

                return sortedBars;
            });
        }

        private Task<List<Restaurant>> GetRestaurants(List<Category> categories, int Id)
        {
            return Task.Run(() =>
            {
                List<Rating> userRatings;
                lock (thisLock)
                {
                    userRatings = _context.Rating.Where(r => r.RestaurantId != null).Where(r => r.UserEmail == User.Identity.Name).ToList();
                }
                List<Restaurant> allRestaurantsMatching = new List<Restaurant>();
                foreach (Category category in categories)
                {
                    List<Restaurant> allRestaurantsMatchingSingle;
                    lock (thisLock)
                    {
                        allRestaurantsMatchingSingle = _context.Restaurants.Where(b => b.Category.CategoryType == category.CategoryType).ToList();
                    }
                    foreach (Restaurant restaurant in allRestaurantsMatchingSingle)
                    {
                        bool foundMatchingRating = false;
                        foreach (Rating r in userRatings)
                        {
                            if (r.RestaurantId == restaurant.Id)
                            {
                                foundMatchingRating = true;
                                break;
                            }
                        }
                        if (foundMatchingRating == false)
                        {
                            allRestaurantsMatching.Add(restaurant);
                        }
                    }
                }
                UserProfile selectedUserCity;
                lock (thisLock)
                {
                    selectedUserCity = _context.UserProfile.Where(u => u.Id == Id).Single();
                }
                Bar linkBar = new Bar();
                lock (thisLock)
                {
                    linkBar.CityId = _context.Cities.Where(c => c.CityName == selectedUserCity.Hometown).Select(c => c.Id).Single();
                }
                List<Restaurant> sortedRestaurants = allRestaurantsMatching.Where(b => (b.Likes != 0) || (b.Dislikes != 0)).OrderBy(b => (b.Likes / (b.Likes + b.Dislikes))).ToList();
                List<Restaurant> unrankedRestaurants = allRestaurantsMatching.Where(b => (b.CityId == linkBar.CityId) && (b.Likes == 0) && (b.Dislikes == 0)).ToList();

                foreach (Restaurant r in unrankedRestaurants)
                {
                    sortedRestaurants.Add(r);
                }

                return sortedRestaurants;
            });
        }

        private Task<List<Events>> GetEvents(List<Category> categories, int userId)
        {
            return Task.Run(() =>
            {
                UserProfile foundUser;
                lock (thisLock)
                {
                    foundUser = _context.UserProfile.Where(u => u.Id == userId).Single();
                }
                List<Events> allEventsMatching = new List<Events>();
                foreach (Category category in categories)
                {
                    List<Events> allEventsMatchingSingle;
                    lock (thisLock)
                    {
                        allEventsMatchingSingle = _context.Events.Where(b => b.Category.CategoryType == category.CategoryType).Where(c => c.Category.CategoryName == category.CategoryName).Where(e => e.City.CityName == foundUser.Hometown).ToList();
                    }
                    foreach (Events eventItem in allEventsMatchingSingle)
                    {
                        allEventsMatching.Add(eventItem);
                    }
                }

                return allEventsMatching;
            });
        }

        private Task<List<Recommendation>> GetNewGems(int Id)
        {
            return Task.Run(() =>
            {
                List<Recommendation> gems = new List<Recommendation>();
                UserProfile selectedUserCity;
                List<Bar> bars;
                List<Restaurant> restaurants;
                Restaurant linkedRestaurant = new Restaurant();
                List<Bar> unrankedBars;
                Bar linkedBar = new Bar();
                List<Restaurant> unrankedRestaurants;
                lock (thisLock)
                {
                    selectedUserCity = _context.UserProfile.Where(u => u.Id == Id).Single();
                    bars = _context.Bars.Where(b => (b.Likes != 0) || (b.Dislikes != 0)).OrderBy(b => (b.Likes / (b.Likes + b.Dislikes))).ToList();
                    restaurants = _context.Restaurants.Where(r => (r.Likes != 0) || (r.Dislikes != 0)).OrderBy(b => (b.Likes / (b.Likes + b.Dislikes))).ToList();
                    linkedRestaurant.CityId = _context.Cities.Where(c => c.CityName == selectedUserCity.Hometown).Select(c => c.Id).Single();
                    linkedBar.CityId = _context.Cities.Where(c => c.CityName == selectedUserCity.Hometown).Select(c => c.Id).Single();
                    unrankedBars = _context.Bars.Where(b => (b.CityId == linkedBar.CityId) && (b.Likes == 0) && (b.Dislikes == 0)).ToList();
                    unrankedRestaurants = _context.Restaurants.Where(r => (r.CityId == linkedRestaurant.CityId) && (r.Likes == 0) && (r.Dislikes == 0)).ToList();
                }

                foreach (Bar b in unrankedBars)
                {
                    lock (thisLock)
                    {
                        b.Category = _context.Categories.Where(c => c.Id == b.CategoryId).Single();
                    }
                    bars.Add(b);
                }
                foreach (Restaurant r in unrankedRestaurants)
                {
                    lock (thisLock)
                    {
                        r.Category = _context.Categories.Where(c => c.Id == r.CategoryId).Single();
                    }
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
            });
        }

        private Task<List<Category>> GetBarCategories(UserProfile user)
        {
            return Task.Run(() =>
            {
                List<Category> barCategories = new List<Category>();
                lock (thisLock)
                {
                    barCategories.Add(_context.Categories.Where(c => c.Id == user.BarCategoryIdOne).Single());
                    barCategories.Add(_context.Categories.Where(c => c.Id == user.BarCategoryIdTwo).Single());
                }
                return barCategories;
            });
        }

        private Task<List<Category>> GetRestaurantCategories(UserProfile user)
        {
            return Task.Run(() =>
            {
                List<Category> restaurantCategories = new List<Category>();
                lock (thisLock)
                {
                    restaurantCategories.Add(_context.Categories.Where(c => c.Id == user.RestaurantCategoryIdOne).Single());
                    restaurantCategories.Add(_context.Categories.Where(c => c.Id == user.RestaurantCategoryIdTwo).Single());
                    restaurantCategories.Add(_context.Categories.Where(c => c.Id == user.RestaurantCategoryIdThree).Single());
                }
                return restaurantCategories;
            });
        }

        private Task<List<Category>> GetEventCategories(UserProfile user)
        {
            return Task.Run(() =>
            {
                List<Category> eventCategories = new List<Category>();
                lock (thisLock)
                {
                    eventCategories.Add(_context.Categories.Where(c => c.Id == user.EventCategoryIdOne).Single());
                    eventCategories.Add(_context.Categories.Where(c => c.Id == user.EventCategoryIdTwo).Single());
                    eventCategories.Add(_context.Categories.Where(c => c.Id == user.EventCategoryIdThree).Single());
                }
                return eventCategories;
            });
        }
        public static Task<string> GetDailyQuote()
        {
            return Task.Run(() =>
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
            });
        }

        public ActionResult PassBar(int id)
        {
            Bar bar = new Bar();
            bar = _context.Bars.Where(b => b.Id == id).FirstOrDefault();
            return View();
        }
    }
}