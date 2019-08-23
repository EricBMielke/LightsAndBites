using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LightsAndBites.Models;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using LightsAndBites.ApiKeys;
using System.Net;
using System.IO;
using Newtonsoft.Json.Linq;
using LightsAndBites.Data;


namespace LightsAndBites.Controllers
{
    public class HomeController : Controller
    {

        private string EventToken = ApiKey.eventKey;
        private readonly Object thisLock = new Object();
        public string Token => EventToken;
        public string userProfileHometown = "Milwaukee";


        private ApplicationDbContext _context;
        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        public async Task<IActionResult> Contact()
        {
            ViewData["Message"] = "Your contact page.";

            Task<bool> dataSeedTask = IsDataSeeded();
            bool successfulSeed = await dataSeedTask;
            if (successfulSeed == true)
            {
                return View();
            }
            else
            {
                return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }
        }

        private async Task<bool> IsDataSeeded()
        {
            List<City> cities;
            lock (thisLock)
            {
                cities = _context.Cities.ToList();
            }
            foreach (City c in cities)
            {
                Task eventsTask = GetAllDailyEvents(c);
                Task locationsTask = GetAllBarsAndRestaurantsDaily(c);
                await eventsTask;
                await locationsTask;
            }

            return true;
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        private async Task GetAllDailyEvents(City city)
        {
               await GetDailyEvents("music", city);
               await GetDailyEvents("conference", city);
               await GetDailyEvents("comedy", city);
               await GetDailyEvents("festivals", city);
               await GetDailyEvents("family_fun_kids", city);
               await GetDailyEvents("community", city);
               await GetDailyEvents("observances", city);
               await GetDailyEvents("performing_arts", city);
        }

        private async Task GetAllBarsAndRestaurantsDaily(City city)
        {
               await GetGoogleData("Sports", city, "Bar");
               await GetGoogleData("College", city, "Bar");
               await GetGoogleData("Specialty", city, "Bar");
               await GetGoogleData("Irish Pub", city, "Bar");
               await GetGoogleData("Dive", city, "Bar");

               await GetGoogleData("Chinese", city, "Restaurant");
               await GetGoogleData("Italian", city, "Restaurant");
               await GetGoogleData("American", city, "Restaurant");
               await GetGoogleData("Fine Dining", city, "Restaurant");
               await GetGoogleData("Mexican", city, "Restaurant");
        }
        private Task GetDailyEvents(string eventType, City city)
        {
            return Task.Run(() =>
            {
                string html = string.Empty;

                string url = @"http://api.eventful.com/json/events/search?app_key=" + Token + "&category=" + eventType + "&location=" + city.CityName + "&sort_order=popularity&page_number=1";

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.AutomaticDecompression = DecompressionMethods.GZip;

                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                using (Stream stream = response.GetResponseStream())
                using (StreamReader reader = new StreamReader(stream))
                {
                    html = reader.ReadToEnd();
                }

                JObject o = JObject.Parse(html);

                foreach (JObject j in o["events"]["event"])
                {
                    Events eventt = new Events();
                    eventt.Name = (j["title"]).ToString();
                    eventt.Latitude = Convert.ToDouble((j["latitude"]));
                    eventt.Longitude = Convert.ToDouble((j["longitude"]));
                    eventt.Type = eventType;
                    eventt.StreetAddress = ((j["venue_address"]).ToString());
                    try
                    {
                        eventt.PictureUrl = ((j["image"]["medium"]["url"]).ToString());
                    }
                    catch
                    {
                        eventt.PictureUrl = "Default";
                    }

                    switch (eventType)
                    {
                        case "music":
                            eventt.CategoryId = 11;
                            break;
                        case "festivals":
                            eventt.CategoryId = 12;
                            break;
                        case "comedy":
                            eventt.CategoryId = 13;
                            break;
                        case "observances":
                            eventt.CategoryId = 14;
                            break;
                        case "performing_arts":
                            eventt.CategoryId = 18;
                            break;
                        case "family_fun_kids":
                            eventt.CategoryId = 15;
                            break;
                        case "community":
                            eventt.CategoryId = 16;
                            break;
                        case "conference":
                            eventt.CategoryId = 17;
                            break;
                    }

                    eventt.CityId = city.Id;
                    if ((j["url"]) == null)
                    {
                        eventt.Website = ((j["venue_url"]).ToString());
                    }
                    else
                    {
                        eventt.Website = ((j["url"]).ToString());
                    }
                    lock (thisLock)
                    {
                        _context.Events.Add(eventt);
                    }
                }
                lock (thisLock)
                {
                    _context.SaveChanges();
                }
            });
        }

        private Task GetGoogleData(string keyWord, City city, string type)
        {
            return Task.Run(() =>
            {
                string data = string.Empty;
                string url = @"https://maps.googleapis.com/maps/api/place/nearbysearch/json?key=" + ApiKey.mapsKey + @"&location=" + city.Latitude + @"," + city.Longitude + @"&keyword=" + keyWord + @"&type=" + type.ToLower() + "&radius=5000";

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

                foreach (JObject j in returnData["results"])
                {
                    if (type == "Bar")
                    {
                        Bar newBar = new Bar();
                        lock (thisLock)
                        {
                            newBar.CategoryId = _context.Categories.Where(c => c.CategoryType == keyWord).Where(c => c.CategoryName == type).Select(c => c.Id).Single();
                            newBar.Category = _context.Categories.Where(c => c.Id == newBar.CategoryId).Single();
                            newBar.CityId = _context.Cities.Where(c => c.CityName == city.CityName).Select(c => c.Id).Single();
                            newBar.City = _context.Cities.Where(c => c.Id == newBar.CityId).Single();
                        }
                        newBar.Latitude = Convert.ToDouble(j["geometry"]["location"]["lat"]);
                        newBar.Longitude = Convert.ToDouble(j["geometry"]["location"]["lng"]);
                        newBar.Likes = 0;
                        newBar.Dislikes = 0;
                        newBar.Name = j["name"].ToString();
                        
                        try
                        {
                            newBar.CardPhoto = j["photos"][0]["photo_reference"].ToString();
                        }
                        catch
                        {
                            newBar.CardPhoto = null;
                        }

                        Bar foundMatchingBar;
                        lock (thisLock)
                        {
                            foundMatchingBar = _context.Bars.Where(r => r.Latitude == newBar.Latitude).Where(r => r.Longitude == newBar.Longitude).Where(r => r.Name == newBar.Name).FirstOrDefault();
                        }
                        if (foundMatchingBar == null)
                        {
                            lock (thisLock)
                            {
                                _context.Bars.Add(newBar);
                            }
                        }
                    }
                    else if (type == "Restaurant")
                    {
                        Restaurant newRestaurant = new Restaurant();
                        lock (thisLock)
                        {
                            newRestaurant.CategoryId = _context.Categories.Where(c => c.CategoryType == keyWord).Where(c => c.CategoryName == type).Select(c => c.Id).Single();
                            newRestaurant.Category = _context.Categories.Where(c => c.Id == newRestaurant.CategoryId).Single();
                            newRestaurant.CityId = _context.Cities.Where(c => c.CityName == city.CityName).Select(c => c.Id).Single();
                            newRestaurant.City = _context.Cities.Where(c => c.Id == newRestaurant.CityId).Single();
                        }
                        newRestaurant.Latitude = Convert.ToDouble(j["geometry"]["location"]["lat"]);
                        newRestaurant.Longitude = Convert.ToDouble(j["geometry"]["location"]["lng"]);
                        newRestaurant.Likes = 0;
                        newRestaurant.Dislikes = 0;
                        newRestaurant.Name = j["name"].ToString();
                        
                        try
                        {
                            newRestaurant.CardPhoto = j["photos"][0]["photo_reference"].ToString();
                        }
                        catch
                        {
                            newRestaurant.CardPhoto = null;
                        }

                        Restaurant foundMatchingRestaurant;
                        lock (thisLock)
                        {
                            foundMatchingRestaurant = _context.Restaurants.Where(r => r.Latitude == newRestaurant.Latitude).Where(r => r.Longitude == newRestaurant.Longitude).Where(r => r.Name == newRestaurant.Name).FirstOrDefault();
                        }
                        if (foundMatchingRestaurant == null)
                        {
                            lock (thisLock)
                            {
                                _context.Restaurants.Add(newRestaurant);
                            }
                        }
                    }

                    else
                    {
                        throw new ArgumentException("Must be searching either a bar or restaurant.");
                    }
                    lock (thisLock)
                    {
                        _context.SaveChanges();
                    }
                }
            });
        }


    }
}

